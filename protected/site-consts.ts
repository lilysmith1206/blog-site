export const mainRoutes = [
    { url: '/', page: '/index.post' },
    { url: '/403', page: '/403.post' },
    { url: '/404', page: '/404.post' },
];

export const styleRoutes = [
    { url: '/site_style', page: '/style-dir/site_style.css' },
    { url: '/mobile_style', page: '/style-dir/mobile_style.css' },
    { url: '/table_style', page: '/style-dir/table_style.css'},
    { url: '/monospacetypewriter', page: '/style-dir/fonts-dir/monospacetypewriter.ttf'},
    { url: '/lylink_icon', page: '/style-dir/lylink_icon.ico' }
];

export const port = 5000;
export const sendFileOptions = () => { return { root: '../public'} };