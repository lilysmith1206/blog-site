import { Database } from "./database";
import { Post } from "./site-types";
import { WebHost } from "./web-host";

// Database.createPost(parsePostFile());

WebHost.startHosting();

function parsePostFile(): Post {
    const post: Post = {
        slug: 'maiden', // You might derive the slug from the filename or some other logic
        title: `Maiden Voyage`,
        parentId: '8875a2ce-d368-4f63-9fdf-018e085bcf95', // Assuming 'index' means no parentId
        dateModified: new Date(),
        name: 'Maiden Voyage',
        keywords: 'lylink, writing, drafts, unnamed essence-rune world, first chapter',
        description: `Lylink - Maiden Voyage`,
        body: `<p>As the <em>Shanty</em> crested a wave, force glyphs shot the boat forward, letting it fall the 10 feet before it slammed onto the ocean surface. The ocean waves slapped against the metallic sheet covering the boat, glinting faintly as it repelled the essence-charged waters. The boat continued pushing its way through the ocean, none the worse for the wear. This had been the fifth time the boat had launched off the top of a wave, and Sae was of the opinion that it was five times too many. Everyone on the ship was expected to do their part to get to the mainland, given their lack of Sailors, and these sudden drops into the waves were not doing her stomach well.</p><p>She finished attaching the essence sheet to the side of the <em>Shanty</em>, grimacing at the sight of it tearing. It had already taken a few too many strikes. They didn't have a backup, so it was a moot point if it did tear. She gathered the bag of carabiners and lifted her hand to her mouth to form a faux-megaphone while she clambered to a standing position. "CAPTAIN!", she shouted, waving at the man manning the helm. "LEFT SIDE SHEET ATTACHMENT, FINISHED!" She gave him a finger gun with her other hand, and grinned as he shook his head. The other girl on the right side was checking the last two attachments for the sheet. They had a vitally important role, given what could happen without the sheet present. The captain waved her over, and she crouched to get through what used to be the windshield to get to his position.</p><p>"Sadie," he started. "I don't have that doohickey you use for detecting essence density, but my <strong>Seaworthiness</strong> is giving me an antsy feeling in the general direction we're going. Mind checkin' for me?"</p><p>She nodded. His <strong>Seaworthiness</strong> hadn't led them wrong yet, though it could be frustratingly vague. She reached into her back pocket and pulled out a small, metallic device. The screen on it was cracked, and the only button it had was one to turn it on. She hesitated; when this died, they'd be sailing blind. Captain knows best. She pressed the button, and felt the glyphs on the back stutter as it sent a pulse out into the ambient essence. She waited for the screen to update. Another pulse was sent out, feeling the boat stutter again as the force glyphs had their essence intake disrupted before she remembered to smack the thing so it would update its screen. When she did, it showed a light purple circle in the center, surrounded by dark purple. The purple was definitely darker in the direction they were going. She hit it again to make sure. "Captain, it looks like it's getting rougher. Your sense was on point"</p><p>"Always easier to go out than in, ain't it?" He motioned for her to go to the back of the ship. "I think it's about time you light up those extra glyphs. We're closing in on the mainland, so my Title tells me. Faster we get out of the essence storms, faster we deliver the haul and get paid." He turned away from her, a clear dismissal. She nodded and took out her lighter, examining the glyph that made up the internals. It still has some juice left in it, the glyph base didn't look that cracked. She'd probably need a new one when they arrived at the city, just in case. Being stranded on the sea without a lighter was not in the plan for getting the fish, getting back the city, and getting paid. She saw the other esser finishing up with the last essence sheet attachment point, and waved her over. The other girl, whose name escaped Sae at the moment, sidled past the cockpit of the boat to her position.</p><p>"Captain gave the go-ahead on speeding us up, but if I try to light these glyphs by myself, he's gonna get thrown off by the tilt. That's a distraction he doesn't need right now. Need you to light the other side." She waved her lighter for emphasis, hoping she could hear her instructions over the wind and the raging sea. The other girl frowned: she had not grown used to the sound of the ocean and wind obscuring instructions yet, but she'd learn. The other girl eventually nodded, taking out her essence lighter.</p><p>"I'll take the right side. You do the left." She murmured, and it was only by the grace of the Captain that Sae could read lips. The storm took the sound of the words away. She just went to the stern and held up three fingers. She positioned her lighter on the intake rune of the left-most glyph, and started counting down. She struck the intake with the lighter as soon as her 'one' finger dropped off her hand, and grabbed ahold on the stern to steady herself from the sudden jolt of speed coming from the newly activated force glyph. Out of the corner of her eye, she saw the other girl - Geneviah, that was it - pull her lighter back from her intake rune of her glyph. <em>Oh, she braced herself before lighting it. She's got some instinct for this.</em> They moved in tandem, priming each glyph across the very rear of the boat.</p><p>It was several copies of the same simple glyph. All it did was channel the ambient essence through a kinetic force rune. If they weren't made out of essence stone, low quality as it was, it's likely they would've given out a half hour earlier.</p><p>She and Geneviah watched as the sheet surrounded the boat whipped and folded over itself in the accelerated wind. It wasn't a tight fit; the only sheet they could find was for a house, it's not like seafaring vessels were very common anymore. The extra material might do them good if it tore. Shaking herself loose of watching the hyponetic motion, she pulled the other girl into the cockpit and settled on the couch. "Captain, we got the extra glyphs up and running. I'm sure you felt it." She checked her essence detector, and watched as the circle around the boat made imprints on their path through the ocean. Each glyph was drawing part of the ambient essence, lessening the power the storms above them could draw, and the effects it had were plainly visible on the screen as the purple surrounding them lightened. Geneviah heaved a tired sigh and curled up next to her, exhausted from the basic work they did. Sae's attention wasn't on the detector or Geneviah, though. It was on that tear forming in the essence sheet protecting them from the lightning. She watched as the tear widened, revealing the roiling sea and the dark, storm-filled skyline.</p>`
    };

    return post;
}

/*
Writing a Backend---lylink, code, content, posts, design, node, development---Writing a Backend
index===code---Writing a backend for my site
<h3>What's a Backend?</h3>
<p>
    A backend is the part of a website you don't get to directly access; it handles requests that you make to any URL on the page. So, for instance, this page is titled "backend", and you access it through [url].org/backend. The server sees that and sends you this page.
</p>
<p>
    It can also handle queries; say I wanted to build a "comments" feature, where anyone can leave a comment on any post I make. I can set up a request handler on a backend for a specific URL, say [url].org/comment?name={name}&body={body}, and it can handle that request, store it, and then attach it to my page so everyone else can see it!
</p>
<h3>What I Used</h3>
<p>
    I previously used an Apache .htaccess file to control the way the site was presented; this had several disadvantages, namely:
</p>
<ul>
    <li>I don't know Apache that well.</li>
    <li>It was cumbersome to adjust, and it didn't really work for what I had in mind.</li>
    <li>It wasn't set up to be a "true" backend; all I had was the .htaccess file, and the Apache documentation made my head spin.</li>
    <li>I couldn't find a good way to set up a testing environment for it.</li>
</ul>
<p>
    Given, a lot of that would have been minimized by things like.. "learning" "Apache", but who's got the time for that in this economy? So, I swapped to a node.js backend, and <em>that's</em> handling all my requests.
</p>
<p>
    It honestly wasn't that hard to set up. Just get <span title="Yeah, I use ChatGPT, yuk it up. It's easy to write out boilerplate code with it.">ChatGPT to write a basic template</span>, optimize it, test it locally, then set it up on the web hosting service I use. Easy peasy.
</p>
<h3>What's next?</h3>
<p>Honestly, I'm not sure. I think I need to rewrite the backend to serve up the CSS and other static content better, and then <em>maybe</em> work on a comment system? It probably isn't too hard. Probably. Then write more posts.</p>
<p>Thank you for reading.</p>
*/