import { Database } from "./database";
import { generatePage } from "./shared/page-generation-common";
import { Page, Post } from "./site-types";

export async function generatePost(slug: string) {
    const dbPost: Post = await Database.GetPostFromDb(slug);
    
    let page: Page = {
        dateModified: new Date(dbPost.dateModified),
        description: dbPost.description,
        keywords: dbPost.keywords,
        title: dbPost.title,
        parentId: dbPost.parentId,
        body: dbPost.body,
        nameOf: dbPost.name,
    }

    return await generatePage(page);
}