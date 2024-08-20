import { Database } from "./database";
import { generatePage } from "./shared/page-generation-common";
import { GenericPage, Page, Post, PostCategory } from "./site-types";

export async function generateCategoryPage(slug: string): Promise<string> {
    const postCategory: PostCategory = await Database.GetCategoryFromSlug(slug);
    const posts: Post[] = await Database.GetAllPostsWithParentCategory(postCategory.categoryId);
    const childCategories: PostCategory[] = (await Database.getChildCategories(postCategory.categoryId))
        .filter(async postCategory => (await Database.GetAllPostsWithParentCategory(postCategory.categoryId)).length > 0)

    const page: Page = {
        dateModified: null,
        description: postCategory.description,
        keywords: postCategory.keywords,
        title: postCategory.title,
        parentId: postCategory.parentId,
        body: slug === '' ? await createIndexBody(posts, childCategories, postCategory.body) : createCategoryBody(posts, childCategories, postCategory.body),
        nameOf: postCategory.name,
    }

    return await generatePage(page);
}

function createCategoryBody(posts: Post[], postCategories: PostCategory[], categoryBody: string): string {
    return `
    ${categoryBody}
    ${postCategories.length === 0 ? '' : `<h3>Sub-categories</h3>`}
    ${createListOfPosts(postCategories)}
    ${posts.length === 0 ? '' : `<h3>Posts</h3>`}
    ${createListOfPosts(posts)}`;
}

async function createIndexBody(posts: Post[], postCategories: PostCategory[], categoryBody: string): Promise<string> {
    const mostRecentUpdatedPosts = (await Database.GetMostRecentUpdatedPosts(10))
        .filter(post => post.slug.match(/\d{3}/) === null);

    return `
    ${createCategoryBody(posts, postCategories, categoryBody)}
    <h3>Most recently updated posts</h2>
    ${createListOfPosts(mostRecentUpdatedPosts)}`;
}


function createListOfPosts(posts: GenericPage[]) {
    if (posts === undefined || posts.length === 0) {
        return '';
    }

    let postListItems: string = '';

    posts.map(post => `<li><a href="${post.slug}">${post.name}</a></li>`)
        .forEach(postListItem => postListItems += postListItem);

    return `
    <ul>
        ${postListItems}
    </ul>`
}
