export const mainRoutes = [
    { url: '/', page: '/index.post' },
    { url: '/403', page: '/403.post' },
    { url: '/404', page: '/404.post' },
];

export const styleRoutes = [
    { url: '/site_style', page: '/style-dir/site_style.css' },
    { url: '/mobile_style', page: '/style-dir/mobile_style.css' },
    { url: '/table_style', page: '/style-dir/table_style.css'},
    { url: '/dark_mode', page: '/style-dir/dark_mode.css'},
    { url: '/light_mode', page: '/style-dir/light_mode.css'},
    { url: '/slider_style', page: '/style-dir/slider_style.css'},
    { url: '/monospacetypewriter', page: '/style-dir/fonts-dir/monospacetypewriter.ttf'},
    { url: '/lylink_icon', page: '/style-dir/lylink_icon.ico' }
];

export const scriptRoutes = [
    { url: '/color_slider', page: '/javascript-dir/color_slider.js' }
];

export const port = 5000;
export const sendFileOptions = () => { return { root: '../public'} };