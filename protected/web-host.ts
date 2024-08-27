import * as express from 'express';
import * as fs from 'fs';
import * as path from 'path';
// import * as crypto from 'crypto';
import { generatePost } from './post-generation';
import { sendFileOptions, port } from './site-consts';
import { AdminAccessToken, Post, PostCategory, RemotePost } from './site-types';
import { Database } from './database';
import { generateCategoryPage } from './category-page-generation';

export class WebHost {
    static app = express();
    static hosting = false;
    static managementAccessTokens: AdminAccessToken[] = [];

    private constructor() {
    }

    public static hostWebsiteDown() {
        this.app.use((req, res) => {
            const acceptHeader = req.headers['accept'] || '';
            
            // Check if the request is expecting HTML
            if (acceptHeader.includes('text/html')) {
                res.status(503).send("The website is down right now. Apologies.");
            } else {
                // Handle non-HTML requests differently, or send a 404 status without redirect
                res.status(404).send('Not Found');
            }
        });
        
        this.app.listen(port, () => {
            console.log(`Server listening on port ${port}`);
        });
    }

    public static async startHosting() {
        if (WebHost.hosting) {
            return;
        }

        WebHost.hosting = true;
        
        this.app.use((req, res, next) => {
            res.setHeader('Cache-Control', 'no-store, no-cache, must-revalidate, proxy-revalidate, max-age=0');
            next();
        });

        this.app.use(express.static(path.resolve('../public/javascript-dir')));
        this.app.use(express.static(path.resolve('../public/style-dir')));
        this.app.use(express.json()); // Middleware to parse JSON bodies

        const allPostSlugsFromDb: string[] = await Database.GetAllPostSlugs();

        // Step 1: Handle defined routes (dynamic routes from your database)
        allPostSlugsFromDb.forEach(slug => {
            this.app.get(`/${slug}`, sendPage(slug));
        });

        const allCategoryPageSlugsFromDb: string[] = await Database.GetAllCategorySlugs();

        allCategoryPageSlugsFromDb.forEach(slug => {
            this.app.get(`/${slug}`, async (req, res) => {
                const postContent = await generateCategoryPage(slug);
                
                if (postContent) {
                    res.send(postContent);
                } else {
                    res.redirect('/404');  // Or return res.status(404).send('Not Found');
                }
            });
        });
                
        this.createSiteManagementRouting();
        this.createRequestRouting();

        // Step 2: Catch-all 404 handler for any undefined routes
        this.app.use((req, res) => {
            const acceptHeader = req.headers['accept'] || '';
            
            // Check if the request is expecting HTML
            if (acceptHeader.includes('text/html')) {
                res.redirect('/404');
            } else {
                // Handle non-HTML requests differently, or send a 404 status without redirect
                res.status(404).send('Not Found');
            }
        });
        
        this.app.listen(port, () => {
            console.log(`Server listening on port ${port}`);
        });
    }

    private static createSiteManagementRouting() {
        this.app.get("/login", (req, res) => {
            const password = "v1p0v5h7LsqnHhGnC88mPgmE06xfD57bK5xLagPiiRDg3dx3Wh"

            if (req?.query?.password !== null && req.query.password === password) {
                const guidToken = crypto.randomUUID();
                const expirationDate = new Date(new Date().getTime() + 86400000);

                this.managementAccessTokens.push({
                    guid: guidToken,
                    expiration: expirationDate 
                });

                res.redirect(`/management?accessToken=${guidToken}`);
            }
            else {
                res.sendFile("/site-management-dir/login.html", sendFileOptions);
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

    static createRequestRouting() {
        this.app.post("/ping", (req, res) => {
            console.log(req.body);

            res.status(200).send("yeah?");
        });

        this.app.get("/getAllSlugs", async (req, res) => {
            const accessToken: string = req.query.accessToken.toString();

            if (accessToken == null || this.managementAccessTokens.find(tokens => tokens.guid === accessToken) === null) {
                res.status(403).send();
            }

            res.status(200).send(JSON.stringify(await Database.GetAllPostSlugs()));
        });

        this.app.get("/getAllCategoryNames", async (req, res) => {
            const accessToken: string = req.query.accessToken.toString();

            if (accessToken == null || this.managementAccessTokens.find(tokens => tokens.guid === accessToken) === null) {
                res.status(403).send();
            }

            res.status(200).send(JSON.stringify(await Database.GetAllCategorySlugs()));
        });

        this.app.get("/getSlugPost", async (req, res) => {
            const accessToken: string = req.query.accessToken.toString();

            if (accessToken == null || this.managementAccessTokens.find(tokens => tokens.guid === accessToken) === null) {
                res.status(403).send();
            }

            const slug: string = req.query.slug.toString();

            const post: Post = await Database.GetPostFromDb(slug);
            const postCategory: PostCategory = await Database.GetCategoryFromId(post.parentId);

            const remotePost: RemotePost = {
                slug: post.slug,
                title: post.title,
                dateModifiedISO: new Date(post.dateModified).toISOString(),
                parentSlug: postCategory?.slug,
                name: post.name,
                keywords: post.keywords,
                description: post.description,
                body: post.body
            };

            res.status(200).send(JSON.stringify(remotePost));
        });

        this.app.post("/sendPost", async (req, res) => {
            const accessToken: string = req.query.accessToken.toString();

            if (accessToken == null || this.managementAccessTokens.find(tokens => tokens.guid === accessToken) === null) {
                res.status(403).send();
            }

            const remotePost: RemotePost = req.body;

            let parentSlug;

            if (remotePost.parentSlug === 'none') {
                parentSlug = null;
            }
            else if (remotePost.parentSlug === '') {
                parentSlug = '';
            }
            else {
                parentSlug = remotePost.parentSlug;
            }

            const post: Post = {
                slug: remotePost.slug,
                dateModified: toMariaDbDateTime(new Date(remotePost.dateModifiedISO)),
                name: remotePost.name,
                title: remotePost.title,
                parentId: (await Database.GetCategoryFromSlug(parentSlug)).categoryId,
                keywords: remotePost.keywords,
                description: remotePost.description,
                body: remotePost.body,
            }

            if (post.dateModified.includes("NaN")) {
                post.dateModified = toMariaDbDateTime(new Date());
            }

            try {
                if (await Database.GetPostFromDb(post.slug) !== undefined) {
                    console.log(await Database.GetPostFromDb(post.slug));

                    Database.updatePost(post);
                }
                else {
                    await Database.createPost(post);

                    this.app.get(`/${post.slug}`, sendPage);
                }

                res.status(200).send("Successful addition/update of post " + post.name);
            }
            catch {
                res.status(500).send("Issue adding/updating post" + post.name);
            }
        })
    }
}

function sendPage(slug: string) {
    return async (req, res) => {
        const postContent = await generatePost(slug);

        if (postContent) {
            res.send(postContent);
        } else {
            res.redirect('/404');
        }
    };
}

function toMariaDbDateTime(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
}