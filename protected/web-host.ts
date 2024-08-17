import * as express from 'express';
import * as fs from 'fs';
import * as path from 'path';
import * as crypto from 'crypto';
import { generatePost } from './post-generation';
import { styleRoutes, mainRoutes, sendFileOptions, port } from './site-consts';
import { AdminAccessToken, BlogRoute, CsvBlogRoute, PostType, PostGenre } from './site-types';

export class WebHost {
    static app = express();
    static hosting = false;
    static managementAccessTokens: AdminAccessToken[] = [];
    static dirPaths: PostGenre[] = [
        { postType: PostType.Blog, blogDir: "blog-dir" },
        { postType: PostType.Code, blogDir: "code-dir" },
        // { postType: PostType.Stories, blogDir: "stories-dir" }
    ]

    private constructor() {
    }

    public static startHosting() {
        if (WebHost.hosting) {
            return;
        }

        WebHost.hosting = true;

        mainRoutes.forEach(({url, page}) => {
            this.app.get(url, (req, res) => {
                res.sendFile(page, sendFileOptions());
            });
        })
        
        this.createPostRouting();
        this.createStoryRouting();
        // this.createSiteManagementRouting();
        this.createStyleRouting();
        
        this.app.get('*', (req, res) => {
            res.redirect('/404');
        });
        
        this.app.listen(port, () => {
            console.log(`Server listening on port ${port}`);
        });
    }

    public static createStoryRouting() {
        this.app.get("/ocean", (req, res) => {
            res.sendFile("/stories-dir/ocean.html", sendFileOptions())
        })
    }
/*
    private static createSiteManagementRouting() {
        this.app.get("/login", (req, res) => {
            const password = "v1p0v5h7LsqnHhGnC88mPgmE06xfD57bK5xLagPiiRDg3dx3Wh"

            if (req.query.password !== null && req.query.password === password) {
                const guidToken = crypto.randomUUID();
                const expirationDate = new Date(new Date().getTime() + 86400000);

                this.managementAccessTokens.push({
                    guid: guidToken,
                    expiration: expirationDate 
                });

                res.redirect(`/management?accessToken=${guidToken}`);
            }
            else {
                res.sendFile("/site-management-dir/login.html", sendFileOptions());
            }
        });

        this.app.get("/management", (req, res) => {
            const accessToken = this.managementAccessTokens.find(accessToken => accessToken.guid === req.query.accessToken);

            if (accessToken !== undefined && accessToken.expiration !== null && accessToken.expiration > new Date()) {
                let file: string = fs.readFileSync(path.resolve("../public/site-management-dir/management.html"), "utf-8");
                
                file = file.replace("ACCESSTOKENREPLACEHERE", accessToken.guid);

                res.send(file);
            }
            else {
                res.redirect("/403");
            }
        });

        this.app.get("/publisher", (req, res) => {
            const accessToken = this.managementAccessTokens.find(accessToken => accessToken.guid === req.query.accessToken);

            if (accessToken !== undefined && accessToken.expiration !== null && accessToken.expiration > new Date()) {
                let file: string = fs.readFileSync(path.resolve("../public/site-management-dir/publisher.html"), "utf-8");
                
                file = file.replace("ACCESSTOKENREPLACEHERE", accessToken.guid);

                res.send(file);
            }
            else {
                res.redirect("/403");
            }
        })
    }
    */
    private static createStyleRouting() {
        styleRoutes.forEach(({ url, page }) => {
            this.app.get(url, (req, res) => {
                res.sendFile(page, sendFileOptions());
            });
        });

        this.app.get('/site_color', (req, res) => {
            let currentTime: number = new Date().getHours();

            if (currentTime >= 19 || currentTime < 7) {
                res.sendFile('/style-dir/dark_mode.css', sendFileOptions());
            }
            else {
                res.sendFile('/style-dir/light_mode.css', sendFileOptions());
            }
        });
    }

    private static createPostRouting() {
        const blogRoutes: CsvBlogRoute[] = this.readBlogPostCsv();

        blogRoutes.map(route => {
            const stats = fs.statSync(route.filePath);
            const modificationDate = new Date(stats.mtime);

            const updateTime = `${modificationDate.getMinutes()}:${modificationDate.getHours()}`;
            const updateDate = `${modificationDate.getMonth() + 1}/${modificationDate.getDate() + 1}/${modificationDate.getFullYear()}`;

            const post = generatePost(fs.readFileSync(route.filePath, "utf8"), updateTime, updateDate);

            return { url: route.url, content: post } as BlogRoute;

        }).forEach(({ url, content }) => {
            this.app.get(`/${url}`, (_req, res) => {
                res.send(content);
            });
        });
    }

    private static readBlogPostCsv(): CsvBlogRoute[] {
        return fs
            .readFileSync("./blog-posts.csv", "utf-8").split("\n")
            .map(blogCsvRow => {
                const rowData: string[] = blogCsvRow.split(",");

                return { url: rowData[0], filePath: path.resolve(`../public/${rowData[1]}`) } as CsvBlogRoute;
            });
    }
}