const editor = new MediumEditor('#rendered', {
    extensions: { table: new MediumEditorTable() },
    toolbar: {
        buttons: ['bold', 'italic', 'underline', 'strikethrough', 'anchor', 'h2', 'h3', 'quote', 'table']
    },
    placeholder: false,
});

const options = {
    indent_size: 4,
    max_char: 0
};

const beautify = SimplyBeautiful();

const htmlTextView = document.getElementById('html-text');
const renderedView = document.getElementById('rendered');
const bodyInputElement = document.getElementById('body');

let isHtmlViewOn = true;
let isRenderedViewOn = false;

document.getElementById('html-view-button').addEventListener("click", () => {
    htmlTextView.innerText = beautify.html(renderedView.innerHTML, options);

    isHtmlViewOn = true;
    isRenderedViewOn = false;
});

document.getElementById('rendered-view-button').addEventListener("click", () => {
    renderedView.innerHTML = htmlTextView.innerText;

    isHtmlViewOn = false;
    isRenderedViewOn = true;
});

setInterval(() => {
    if (isHtmlViewOn === true) {
        bodyInputElement.value = htmlTextView.innerText;
    }
    else {
        bodyInputElement.value = renderedView.innerHTML;
    }
}, 50)

htmlTextView.addEventListener('keydown', function (event) {
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