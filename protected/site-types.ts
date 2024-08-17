export type CsvBlogRoute = {
    url: string,
    filePath: string
}

export type BlogRoute = {
    url: string,
    content: string
}

export type PostGenre = {
    postType: PostType,
    blogDir: string
}

export type AdminAccessToken = {
    guid: string,
    expiration: Date
};

export type BlogPost = {
    metadata: { key: string, data: string }[],
    directory: PostType,
    title: string,
    updateTime: string,
    updateDate: string,
    postContent: string
}

export enum PostType {
    Blog = "BLOG",
    Code = "CODE",
    Stories = "WRITINGS"
}