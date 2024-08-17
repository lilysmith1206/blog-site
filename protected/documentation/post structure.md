### Documentation for Post Storage Format

#### Overview
The `.post` file format is designed to store metadata, header information, and body content for generating HTML pages. The content of the file is divided into three main sections, separated by specific delimiters (`---` and `===`). The format is simple and easy to parse, making it ideal for dynamic web content generation.

#### File Structure

A `.post` file is structured as follows:

1. **Metadata Section**
2. **Header Section**
3. **Body Content**

Each of these sections is separated by a line containing the delimiter `---`.

#### 1. Metadata Section
The first section contains metadata related to the post. The metadata is separated by commas and consists of three parts:

- **Title**: The main title of the post.
- **Keywords**: A comma-separated list of keywords relevant to the post. These are used for SEO purposes.
- **Description**: A brief description of the post, also used for SEO.

**Example:**
```
Blog---lylink, blog, content, posts---Lylink's Blog - Stuff That Happens to Me
```

In this example:
- `Blog` is the title.
- `lylink, blog, content, posts` are the keywords.
- `Lylink's Blog - Stuff That Happens to Me` is the description.

#### 2. Header Section
The second section is the header, which consists of link-through information and the main title of the post. The header items are separated by the delimiter `===`.

- **Link-Through**: A list of links that represent the navigation path. This is useful for breadcrumb navigation on the generated HTML page.
- **Main Title**: The main title to be displayed on the webpage.

**Example:**
```
index---Blog
```

In this example:
- `index` indicates a link to the homepage.
- `Blog` is the main title to be displayed on the webpage.

#### 3. Body Content
The third section is the body of the post. This section contains the HTML content to be displayed on the webpage. This section starts after the second `---` delimiter and extends to the end of the file.

**Example:**
```html
<h2>
    What is this?
</h2>
<p>
    You know how, some stuff you do is interesting to you, but there's nobody immediately willing to be able to listen to it? That's what this is.
</p>
<p>
    This is where I put the stuff that no one gives a rat's ass about right now, but might later. Enjoy your browsing, netizen.
</p>

<h2 title="These are sorted chronologically, just so you know.">
    Posts
</h2>
<ul>
    <li><a href="mullvad">Switching from ExpressVPN to Mullvad VPN</a></li>
</ul>
```

#### Example `.post` File
Below is a full example of a `.post` file:

```
Blog---lylink, blog, content, posts---Lylink's Blog - Stuff That Happens to Me
index---Blog
<h2>
    What is this?
</h2>
<p>
    You know how, some stuff you do is interesting to you, but there's nobody immediately willing to be able to listen to it? That's what this is.
</p>
<p>
    This is where I put the stuff that no one gives a rat's ass about right now, but might later. Enjoy your browsing, netizen.
</p>

<h2 title="These are sorted chronologically, just so you know.">
    Posts
</h2>
<ul>
    <li><a href="mullvad">Switching from ExpressVPN to Mullvad VPN</a></li>
</ul>
```

#### Function Explanation
The provided function parses the `.post` file into a structured HTML document. Here’s a breakdown:

- **Metadata Parsing**: The first line is split by `---` to retrieve the title, keywords, and description.
- **Header Parsing**: The second line is split by `---` and `===` to extract link-through paths and the header title.
- **Body Parsing**: The rest of the file is treated as the body content.

This data is then compiled into an HTML template, which is returned as a complete webpage.

#### HTML Template
The HTML template uses the parsed data as follows:
- The metadata (title, keywords, description) is embedded in the `<head>` section.
- The link-through paths and header title are inserted into the `<header>`.
- The body content is placed within the `<main>` section.

This template ensures that the content is properly formatted and ready for web display.