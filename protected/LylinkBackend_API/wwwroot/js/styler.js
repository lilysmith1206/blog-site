async function getStylesheet(name) {
    try {
        const response = await fetch(`/getStylesheetFromName?name=${encodeURIComponent(name)}`);

        if (response.ok === false) {
            throw new Error(`Failed to fetch stylesheet: ${response.status}`);
        }

        return await response.text();
    } catch (error) {
        console.error(error);
        return null;
    }
}

async function saveStylesheet(sheetName, styleData) {
    try {
        const response = await fetch(`/saveStylesheet?sheetName=${encodeURIComponent(sheetName)}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(styleData)
        });

        if (response.ok === false) {
            throw new Error(`Failed to save stylesheet: ${response.status}`);
        }

        return true;
    } catch (error) {
        console.error(error);
        return false;
    }
}

function removeElementFromParent(element, parent) {
    parent.removeChild(element);
}

function adjustHeight(element) {
    element.style.height = "";
    element.style.height = element.scrollHeight + "px";
}

const ruleContainer = document.getElementById('rule-container');
const ruleTemplate = document.getElementById('css-rule-template');

function addRule(selectors, declarations) {
    const card = ruleTemplate.content.cloneNode(true).firstElementChild;

    card.querySelector(".delete-rule-button").addEventListener("click", () => removeElementFromParent(card, ruleContainer));

    card.querySelector('.selectors-input').value = selectors.join(', ');

    let declarationsString = "";

    declarations.forEach(decl => {
        declarationsString += `${decl.property}: ${decl.value}\n`;
    });

    const declarationsInput = card.querySelector('.declarations-input');

    declarationsInput.value = declarationsString.substring(0, declarationsString.length - 1);

    declarationsInput.addEventListener("input", () => adjustHeight(declarationsInput));

    setTimeout(() => adjustHeight(declarationsInput), 10);

    ruleContainer.appendChild(card);
}

const stylesheetBox = document.getElementById("stylesheet-select");
const stylesheetNameInput = document.getElementById("file-name");

stylesheetBox.addEventListener("change", async () => {
    const stylesheetSlug = stylesheetBox.value;

    const stylesheetData = JSON.parse(await getStylesheet(stylesheetSlug));

    if (stylesheetData === null) {
        return;
    }

    ruleContainer.innerHTML = '';
    stylesheetNameInput.value = stylesheetBox.value;

    stylesheetData.forEach(rule => {
        addRule(rule.selectors, rule.declarations)
    });
});

const addRuleButton = document.getElementById('add-rule-button');

addRuleButton.addEventListener("click", () => {
    addRule([], []);
});

const submitCssButton = document.getElementById('submit-css-button');

submitCssButton.addEventListener("click", () => {
    const stylesheetFileName = document.getElementById('file-name').value;

    const ruleDetails = [...document.querySelectorAll("div.css-rule-card")].map(ruleCard => {
        const labelElementChildren = [...ruleCard.children].filter(element => element.tagName == "LABEL");

        return {
            selectors: labelElementChildren[0].children[0].value.split(","),
            declarations: labelElementChildren[1].children[0].value.split("\n").map(declaration => {
                let property = declaration.split(": ")[0];
                let value = declaration.split(": ")[1];

                return { property, value };
            })
        }
    });

    if (saveStylesheet(stylesheetFileName, ruleDetails) === true) {
        alert("Stylesheet saved.");
    }

    console.log(ruleDetails);
})