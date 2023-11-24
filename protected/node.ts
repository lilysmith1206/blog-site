import * as express from 'express';
import * as fs from 'fs';
import * as path from 'path';
import { generatePost } from './post-generation';

const app = express();
const port = 5000;
const sendFileOptions = () => { return { root: '../public'} };

const appRoutes = [
    { url: '/narcissism', page: 'narcissism.post', post: "" },
    { url: '/code', page: 'code-dir/code.post', post: "" },
    { url: '/hovering', page: 'code-dir/hovering.post', post: "" },
    { url: '/backend', page: 'code-dir/backend.post', post: "" },
    { url: '/blog', page: 'blog-dir/blog.post', post: "" },
    { url: '/mullvad', page: 'blog-dir/mullvad.post', post: "" },
];

const styleRoutes = [
    { url: '/site_style', page: '/style-dir/site_style.css' },
    { url: '/mobile_style', page: '/style-dir/mobile_style.css' },
    { url: '/table_style', page: '/style-dir/table_style.css'},
    { url: '/monospacetypewriter', page: '/style-dir/fonts-dir/monospacetypewriter.ttf'},
    { url: '/lylink_icon', page: '/style-dir/lylink_icon.ico' }
];

appRoutes.map((route) => {
    const stats = fs.statSync(path.resolve(`../public/${route.page}`));

    const modificationDate = new Date(stats.mtime);

    const updateTime = `${modificationDate.getMinutes()}:${modificationDate.getHours()}`;
    const updateDate = `${modificationDate.getMonth() + 1}/${modificationDate.getDate() + 1}/${modificationDate.getFullYear()}`

    route.post = generatePost(fs.readFileSync(path.resolve(`../public/${route.page}`), "utf8"), updateTime, updateDate);

    return route;

}).forEach(({url, post}) => {
    app.get(url, (req, res) => {
        console.log(url, "directed");

        res.send(post);
    });
})

app.get("/", (req, res) => {
    console.log("index directed");

    res.sendFile("/index.html", sendFileOptions());
});

app.get("/404", (req, res) => {
    console.log(404);

    res.status(404).sendFile("/404.html", sendFileOptions());
});

[...styleRoutes].forEach(({url, page}) => {
    app.get(url, (req, res) => {
        res.sendFile(page, sendFileOptions());
    });
});

app.get('/site_color', (req, res) => {
    let currentTime: number = new Date().getHours();

    if (currentTime >= 19 || currentTime < 7) {
        res.sendFile('/style-dir/dark_mode.css', sendFileOptions());
    }
    else {
        res.sendFile('/style-dir/light_mode.css', sendFileOptions());
    }
});

app.get('*', (req, res) => {
    res.redirect('/404');
});

app.listen(port, () => {
    console.log(`Server listening on port ${port}`);
});
