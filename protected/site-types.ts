export type CsvBlogRoute = {
    url: string,
    filePath: string
}

export type BlogRoute = {
    url: string,
    content: string
}

export type AdminAccessToken = {
    guid: string,
    expiration: Date
};

export enum PostType {
    Blog = "BLOG",
    Code = "CODE",
    Stories = "WRITINGS"
};

export type GenericPage = {
    slug: string,
    name: string
}

export type Post = {
    slug: string;
    title: string;
    parentId: string;
    dateModified: Date;
    name: string;
    keywords: string;
    description: string;
    body: string;
};

export type PostCategory = {
    categoryId: string,
    parentId: string,
    name: string,
    slug: string,
    title: string,
    keywords: string,
    description: string,
    body: string,
};

export type Page = {
    dateModified: Date,
    title: string,
    keywords: string,
    description: string,
    parentId: string,
    nameOf: string,
    body: string
}

export type RemotePost = {
    slug: string;
    title: string;
    parentSlug: string;
    name: string;
    keywords: string;
    description: string;
    body: string;
}