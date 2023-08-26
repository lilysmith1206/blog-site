import express from 'express';

const app = express();
const port = 5000;
const sendFileOptions = () => { return { root: '../public'} };

const appRoutes = [
    { url: '/', page: '/index.html' },
    { url: '/404', page: '/404.html' },
    { url: '/narcissism', page: '/narcissism.html' },
    { url: '/code', page: '/code-dir/code.html' },
    { url: '/hovering', page: '/code-dir/hovering.html' },
    { url: '/blog', page: '/blog-dir/blog.html' },
    { url: '/mullvad', page: '/blog-dir/mullvad.html' },
    { url: '/site_style', page: '/style-dir/site_style.css' },
    { url: '/mobile_style', page: '/style-dir/mobile_style.css' },
    { url: '/table_style', page: '/style-dir/table_style.css'},
    { url: '/monospacetypewriter', page: '/style-dir/fonts-dir/monospacetypewriter.ttf'},
    { url: '/lylink_icon', page: '/style-dir/lylink_icon.ico' }
];

appRoutes.forEach(({url, page}) => {
    app.get(url, (req, res) => {
        res.sendFile(page, sendFileOptions());
    });
});

app.get('*', (req, res) => {
    res.redirect('/404');
});

app.listen(port, () => {
    console.log(`Server listening on port ${port}`);
});