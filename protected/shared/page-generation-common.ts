import { Database } from "../database";
import { Page, PostCategory } from "../site-types";

export async function generatePage(page: Page) {
    const doesPostContainTable: boolean = checkPostBodyForTableElement(page.body);
    const { updateTime, updateDate } = GetDateModifiedValues(page.dateModified);
    
    return `
<!DOCTYPE html>
<html lang="en">
    <head>
        <script src="/adblocker_popup.js"></script>

        <title>${page.title}</title>
        
        <link id="siteStyle" rel="stylesheet" href="/site_style.css" onerror="showAdblockPopup()" />
        <link id="mobileStyle" rel="stylesheet" href="/mobile_style.css" onerror="showAdblockPopup()" />
        <link id="sliderStyle"rel="stylesheet" href="/slider_style.css" onerror="showAdblockPopup()" />
        <link id="adblockStyle"rel="stylesheet" href="/adblock_style.css" onerror="showAdblockPopup()" />

        <link id="lightStyle" rel="stylesheet" href="/light_mode.css" disabled onerror="showAdblockPopup()" />
        <link id="darkStyle" rel="stylesheet" href="/dark_mode.css" disabled onerror="showAdblockPopup()" />

        ${doesPostContainTable ? `<link id="tableStyle" rel="stylesheet" href="table_style.css" onerror="showAdblockPopup()" />` : ""}
        
        <link id="icon" rel="icon" type="image/x-icon" href="/lylink_icon.ico" onerror="showAdblockPopup()" />

        <script src="/color_slider.js" defer></script>

        <meta charset="UTF-8" />
        <meta name="keywords" content="${page.keywords}" />
        <meta name="description" content="${page.description}" />
        <meta name="author" content="Lylink">
        <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
        <script>
            /*to prevent Firefox FOUC, this must be here*/
            let FF_FOUC_FIX;
        </script>
    </head>
    <body>
        <div id="adblock-background">
        </div>
        <div id="adblock-popup">
            <p>Hey, it looks like a resource failed to load. If you have an ad-blocker, please turn it off.</p>
            <p>This site will never have ads. Please turn off your ad-blocker so my stylesheets load properly.</p>
            <p>Thank you.</p>
            <button onclick="closePopup()">Close</button>
        </div>
        <div id="page-container">
            <label id="colorStyleSwitch" class="switch">
                <input id="colorStyleSwitchCheckbox" type="checkbox">
                <span class="slider" onclick="handleColorStyleSwitching()"></span>
            </label>
            <div id="content-wrap">
                ${await generatePageHeader(page.parentId, page.nameOf, updateTime, updateDate)}
                ${generatePageBody(page.body)}
            </div>
            <footer>
                <p>
                    Copyright 2024, All Rights Reserved
                </p>
            </footer>
        </div>
    </body>
</html>`
}

async function generatePageHeader(parentId: string, pageName: string, updateTime: string, updateDate: string): Promise<string> {
    return `
<header>
    ${parentId === null ? '' : `<h3>${await getParentHeader(parentId)}</h3>`}
    <h1 id="website-header">
        ${pageName}
    </h1>
    ${updateTime === null || updateDate === null ? '' : `<h5>${`Updated ${updateTime} on ${updateDate}`}</h5>`}
</header>`
}

export function generatePageBody(pageBody: string): string {
    return `
<div class="container">
    <main>
        ${pageBody}
    </main>
</div>`;
}

function checkPostBodyForTableElement(postBody: string): boolean {
    // Use a regular expression to match <table> tags that are not within content strings
    const tableTagRegex = /<table\b[^>]*>/i;
    
    // Test if the postBody contains any <table> element
    return tableTagRegex.test(postBody);
}

function GetDateModifiedValues(date: Date) {
    if (date === null) {
        return {
            updateTime: null,
            updateDate: null
        }
    }

    let minutes: string;

    let minutesValue: number = date.getMinutes();

    if (minutesValue < 10) {
        minutes = `0${minutesValue}`;
    }
    else {
        minutes = minutesValue.toString();
    }

    const updateTime = `${date.getHours()}:${minutes}`;
    const updateDate = `${date.getMonth() + 1}/${date.getDate() + 1}/${date.getFullYear()}`;
    return { updateTime, updateDate };
}

async function getParentHeader(parentId: string) {
    const parents: PostCategory[] = (await Database.GetParentCategories(parentId, null));
    const firstParent = parents.pop();
    const indexSlugCheck = (slug: string) => slug === '' ? '/' : slug;

    parents.reverse();

    let parentsHeader = `<li><a href="${indexSlugCheck(firstParent.slug)}">${firstParent.name.toLowerCase()}</a>`;

    for (let i = 0; i < parents.length; i++) {
        parentsHeader += `<li><a href="${indexSlugCheck(parents[i].slug)}">${parents[i].name.toLowerCase()}</a></li>`;
    }
    return `<nav class="breadcrumb"><ul>${parentsHeader}</ul></nav>`;
}