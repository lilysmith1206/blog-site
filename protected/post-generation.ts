export function generatePost(post: string, timeUpdated: string, dateUpdated: string) {
    const postMetadata: string[] = post.split("\n")[0].split("---");
    const postHeader: string[] = post.split("\n")[1].split("---");
    const postBody: string = post.split("\n").slice(2).join("\n");

    let headerLinkThrough = "";

    postHeader[0].split("===").forEach(linkThrough => {
        if (linkThrough === "index") {
            headerLinkThrough += `<a href="/">index</a> →`;
        }
        else {
            headerLinkThrough += ` <a href="${linkThrough}">${linkThrough}</a> →`;
        }
    });

    let headerTitle = postHeader[1];
    let headerUpdatedOn = `Updated ${timeUpdated} on ${dateUpdated}`
    
    const doesPostContainTable = checkPostBodyForTableElement(postBody);

    return `<!DOCTYPE html>
                <html lang="en">
                    <head>
                        <title>Lylink - ${postMetadata[0]}</title>
                        
                        <link rel="stylesheet" href="site_style" />
                        <link rel="stylesheet" href="mobile_style">
                        <link rel="stylesheet" href="slider_style">

                        ${doesPostContainTable ? "<link rel=\"stylesheet\" href=\"table_style\" />" : ""}
                        
                        <link rel="icon" type="image/x-icon" href="lylink_icon">

                        <script src="color_slider" defer> </script>

                        <meta charset="UTF-8" />
                        <meta name="keywords" content="${postMetadata[1]}" />
                        <meta name="description" content="${postMetadata[2]}" />
                        <meta name="author" content="Lylink">
                        <meta name="viewport" content="width=device-width, initial-scale=1.0">
                        <script>
                            /*to prevent Firefox FOUC, this must be here*/
                            let FF_FOUC_FIX;
                        </script>
                    </head>
                    <body>
                        <div id="page-container">
                            <label id="colorStyleSwitch" class="switch">
                                <input id="colorStyleSwitchCheckbox" type="checkbox">
                                <span class="slider" onclick="handleColorStyleSwitching()"></span>
                            </label>
                            <div id="content-wrap">
                                <header>
                                    <h3>${headerLinkThrough}</h3>
                                    <h1 id="website-header">
                                        ${headerTitle}
                                    </h1>
                                    <h5>${headerUpdatedOn}</h5>
                                </header>
                                <div class="container">
                                    <main>
                                        ${postBody}
                                    </main>
                                </div>
                            </div>
                            <footer>
                                <p>
                                    Copyright 2024, All Rights Reserved
                                </p>
                            </footer>
                        </div>
                    </body>
                </html>`;
}

function checkPostBodyForTableElement(postBody: string): boolean {
    // Use a regular expression to match <table> tags that are not within content strings
    const tableTagRegex = /<table\b[^>]*>/i;
    
    // Test if the postBody contains any <table> element
    return tableTagRegex.test(postBody);
}