

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

const asideToggleButton = document.getElementById('asideToggleButton');

let asideIsHidden = false;

asideToggleButton.addEventListener("click", () => {
    const asideContainer = document.querySelector("aside");
    const mainContainer = document.querySelector("main");

    if (asideIsHidden === true) {
        asideContainer.style.right = "0%";
        mainContainer.style.width = "70%";
        asideToggleButton.innerText = ">";

        asideIsHidden = false;
    }
    else {
        asideContainer.style.right = "-30%";
        mainContainer.style.width = "90%";
        asideToggleButton.innerText = "<";

        asideIsHidden = true;
    }
})

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