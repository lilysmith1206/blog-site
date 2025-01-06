const editor = new MediumEditor('#rendered', {
    extensions: { table: new MediumEditorTable() },
    toolbar: {
        buttons: ['bold', 'italic', 'underline', 'strikethrough', 'anchor', 'h2', 'h3', 'quote', 'table']
    },
    placeholder: false,
});

document.getElementById("rendered")["data-placeholder"] = "";

const options = {
    indent_size: 4,
    max_char: 0
};

const beautify = SimplyBeautiful();

function showHtmlView() {
    let htmlView = document.getElementById('html');
    let renderedView = document.getElementById('rendered');

    if (htmlView.style.display === 'none') {
        htmlView.innerText = beautify.html(renderedView.innerHTML, options);

        showTabContent(event, 'html');
    }
}

function showRenderedView() {
    let htmlView = document.getElementById('html');
    let renderedView = document.getElementById('rendered');

    if (renderedView.style.display === 'none') {
        renderedView.innerHTML = htmlView.innerText;

        showTabContent(event, 'rendered');
    }
}

document.getElementById('html').addEventListener('keydown', function (event) {
    if (event.key === 'Tab') {
        event.preventDefault();

        const selection = window.getSelection();
        const range = selection.getRangeAt(0);

        const spaceNode = document.createTextNode('    ');

        range.insertNode(spaceNode);

        range.setStartAfter(spaceNode);
        range.setEndAfter(spaceNode);

        selection.removeAllRanges();
        selection.addRange(range);
    }

    setTimeout(() => document.getElementById("body").value = event.target.innerText, 50);
});

document.getElementById("rendered").addEventListener("keydown", (event) => {
    document.getElementById("body").value = event.target.innerHTML;
})

const checkbox = document.getElementById('isDraft');

checkbox.addEventListener('change', function () {
    if (checkbox.checked) {
        checkbox.value = "true";
    } else {
        checkbox.value = "false";
    }
});

function getSlugBody(slugBox) {
    fetch(`/getPostFromSlug?slug=${slugBox.value}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json' // Assuming you're sending JSON
        },
    })
    .then(async response => {
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }

        response = await response.text();

        return JSON.parse(response);
    })
    .then(data => {
        document.getElementById('title').value = data.title;
        document.getElementById('keywords').value = data.keywords;
        document.getElementById('description').value = data.description;
        document.getElementById('name').value = data.name;
        document.getElementById('slug').value = data.slug;
        document.getElementById('isDraft').checked = data.isDraft == true;
        document.getElementById('isDraft').value = data.isDraft == true;

        document.getElementById('html').textContent = beautify.html(data.body, options);
        document.getElementById('rendered').innerHTML = data.body;
        document.getElementById("body").value = beautify.html(data.body, options);

        if (data.parentSlug === undefined) {
            document.getElementById('categoryBox').value = 'none';
        }
        else {
            document.getElementById('categoryBox').value = data.parentSlug;
        }
    })
    .catch(error => {
        console.error('There was a problem with the fetch operation:', error);
    });
}
