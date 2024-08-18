export const mainRoutes = [
    { url: '/', page: '/index.post' },
    { url: '/403', page: '/403.post' },
    { url: '/404', page: '/404.post' },
];

export const port = 5000;
export const sendFileOptions = () => { return { root: '../public'} };