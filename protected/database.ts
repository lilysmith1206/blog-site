import * as mariadb from 'mariadb';
import { Post, PostCategory } from './site-types';

export class Database {
    private static pool: mariadb.Pool = mariadb.createPool({host: 'localhost', user: 'root', password: 'root', database: 'lylinkdb', connectionLimit: 5});


    static async GetAllCategorySlugs() {
        let conn: mariadb.PoolConnection;

        try {
            conn = await this.pool.getConnection();

            const postsFromDb: PostCategory[] = await conn.query('select * from post_hierarchy');
            
            return postsFromDb
                .map(category => category.slug);
        }
        catch (error) {
            console.log(`error with getting all slugs: ${error}`);
        }
        finally {
            conn.release();
        }
    }

    static async GetAllPostSlugs() {
        let conn: mariadb.PoolConnection;

        try {
            conn = await this.pool.getConnection();

            const postsFromDb: Post[] = await conn.query('select * from posts');

            return postsFromDb.map(dbPost => dbPost.slug);
        }
        catch (error) {
            console.log(`error with getting all slugs: ${error}`);
        }
        finally {
            conn.release();
        }
    }

    static async GetPostFromDb(slug: string) {
        let conn: mariadb.PoolConnection;
    
        try {
            conn = await this.pool.getConnection();
            
            const postFromDb: Post = (await conn.query(`select * from posts where slug = '${slug}'`))[0];

            return postFromDb;
        }
        catch (error) {
            console.log(`error with getting post: ${error}`);
        }
        finally {
            if (conn) conn.release(); //release to pool
        }

        return null;
    }
    
    static async GetAllPostsWithParentCategory(parentId: string): Promise<Post[]> {
        let conn: mariadb.PoolConnection;

        try {
            conn = await this.pool.getConnection();

            const posts: Post[] = await conn.query(`select * from posts where parentId = '${parentId}'`);

            return posts;
        }
        catch (error) {
            console.log(`error with getting posts from category id: ${error}`);
        }
        finally {
            conn.release();
        }
    
        return null;
    }

    static async GetMostRecentUpdatedPosts(amount: number) {
        let conn: mariadb.PoolConnection;

        try {
            conn = await this.pool.getConnection();

            const posts: Post[] = await conn.query(`select * from posts order by dateModified desc`);

            return posts.slice(0, amount);
        }
        catch (error) {
            console.log(`error with getting posts from category id: ${error}`);
        }
        finally {
            conn.release();
        }
    
        return [];
    }

    static async GetCategoryFromSlug(slug: string): Promise<PostCategory> {
        let conn: mariadb.PoolConnection;

        try {
            conn = await this.pool.getConnection();

            const postCategory: PostCategory = (await conn.query(`select * from post_hierarchy where slug = '${slug}'`))[0];

            return postCategory;
        }
        catch (error) {
            console.log(`error with getting post categories: ${error}`);
        }
        finally {
            conn.release();
        }
    
        return null;
    }

    static async GetCategoryFromId(id: string): Promise<PostCategory> {
        let conn: mariadb.PoolConnection;

        try {
            conn = await this.pool.getConnection();

            const postCategory: PostCategory = (await conn.query(`select * from post_hierarchy where categoryId = '${id}'`))[0];

            return postCategory;
        }
        catch (error) {
            console.log(`error with getting post categories: ${error}`);
        }
        finally {
            conn.release();
        }
    
        return null;
    }

    static async GetParentCategories(categoryId: string, conn: mariadb.PoolConnection): Promise<PostCategory[]> {
        if (conn === null) {
            conn = await Database.pool.getConnection();
        }

        const parents: PostCategory[] = [];
        let parent: PostCategory;

        try {
            const postCategories: PostCategory[] = await conn.query(`select * from post_hierarchy where categoryId = '${categoryId}'`);
        
            parent = postCategories.find(parent => parent.categoryId == categoryId);

            if (parent != null && parent.parentId != null) {
                return parents.concat(parent, await Database.GetParentCategories(parent.parentId, conn));
            }
            else if (parent === null) {
                return await conn.query(`select * from post_hierarchy where slug = ''`);
            }
            else {
                return [parent];
            }
    
        }
        catch (error) {
            console.log(`error with getting post categories: ${error}`);
        }
        finally {
            conn.release();
        }
    }

    static async getChildCategories(categoryId: string): Promise<PostCategory[]> {
        const conn: mariadb.PoolConnection = await this.pool.getConnection();

        try {
            const postCategories: PostCategory[] = await conn.query(`select * from post_hierarchy where parentId = '${categoryId}'`);
            
            return postCategories;
        }
        catch (error) {
            console.log(`error with getting post category child categories: ${error}`);
        }
        finally {
            conn.release();
        }
    }
    
    static async createPost(post: Post) {
        const conn: mariadb.PoolConnection = await this.pool.getConnection();
    
        try {
            console.log("post");
            console.log(post);

            const insertResult = await conn.query(
                `INSERT INTO posts (slug, title, parentId, dateModified, name, keywords, description, body) 
                VALUES (?, ?, ?, CURRENT_TIMESTAMP, ?, ?, ?, ?)`, 
                [post.slug, post.title, post.parentId, post.name, post.keywords, post.description, post.body]
            );

            console.log('Post inserted successfully:', insertResult);
        } 
        catch (error) {
            console.log(`Error with inserting post: ${error}`);
        } 
        finally {
            conn.release();
        }
    }

    static async updatePost(post: Post) {
        const conn: mariadb.PoolConnection = await this.pool.getConnection();
    
        try {
            const insertResult = await conn.query(
                `UPDATE posts 
                 SET slug = ?, title = ?, parentId = ?, dateModified = CURRENT_TIMESTAMP, 
                     name = ?, keywords = ?, description = ?, body = ? 
                 WHERE slug = ?`,
                [post.slug, post.title, post.parentId, post.name, post.keywords, post.description, post.body, post.slug]
            );
            

            console.log('Post updated successfully:', insertResult);
        } 
        catch (error) {
            console.log(`Error with inserting post: ${error}`);
        } 
        finally {
            conn.release();
        }
    }

    /*static async createPostCategory(postCategory: PostCategory) {
        const conn: mariadb.PoolConnection = await this.pool.getConnection();

        try {
            const insertResult = await conn.query(
                `INSERT INTO posts (slug, title, parentId, dateModified, name, keywords, description, body) 
                VALUES (?, ?, ?, ?, ?, ?, ?, ?)`, 
                [post.slug, post.title, post.parentId, post.dateModified, post.name, post.keywords, post.description, post.body]
            );

        }
    }*/
    
}