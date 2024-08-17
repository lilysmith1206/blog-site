let currentColorStatus = localStorage.getItem("currentColor");
let colorStyleSwitch = document.getElementById("colorStyleSwitchCheckbox");

console.log(currentColorStatus);

if (currentColorStatus !== null) {
    if (currentColorStatus === "dark") {
        setColorStyle("dark");
        colorStyleSwitch.checked = true;
    }
    else if (currentColorStatus === "light") {
        setColorStyle("light");
        colorStyleSwitch = false;
    }
    else {
        throw `${currentColorStatus} is not a valid color style.`;
    }
}
else {
    setColorStyle("light");
    colorStyleSwitch = false;
}

function handleColorStyleSwitching() {
    console.clear();

    console.log("checkbox clicked");
    console.log("checkbox status" + colorStyleSwitch.checked);

    if (colorStyleSwitch.checked === true) {
        setColorStyle("light");
    }
    else {
        setColorStyle("dark");
    }
}

function setColorStyle(color) {
    localStorage.setItem("currentColor", color);

    let oppositeColor;

    if (color === "light") {
        oppositeColor = "dark";
    }
    else {
        oppositeColor = "light";
    }

    console.log("Color: " + color);
    console.log("Existing: " + oppositeColor);

    const headElement = document.getElementsByTagName('head')[0];

    [...headElement.children].forEach(child => {
        if (child.href == `${oppositeColor}_mode`) {
            headElement.removeChild(child);
        }
    });

    var styles = document.createElement('link');
    
    styles.rel = 'stylesheet';
    styles.type = 'text/css';
    styles.media = 'screen';
    styles.href = `${color}_mode`;

    headElement.appendChild(styles);
}