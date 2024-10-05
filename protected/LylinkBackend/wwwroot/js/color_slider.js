let currentColorStatus = localStorage.getItem("currentColor");
let colorStyleSwitch = document.getElementById("colorStyleSwitchCheckbox");

if (currentColorStatus !== null) {
    if (currentColorStatus === "dark") {
        setColorStyle("dark");
        colorStyleSwitch.checked = true;
    }
    else if (currentColorStatus === "light") {
        setColorStyle("light");
        colorStyleSwitch.checked = false;
    }
    else {
        throw `${currentColorStatus} is not a valid color style.`;
    }
}
else {
    setColorStyle("light");
    colorStyleSwitch.checked = false;
}

function handleColorStyleSwitching() {
    if (colorStyleSwitch.checked === true) {
        setColorStyle("light");
    }
    else {
        setColorStyle("dark");
    }
}

function setColorStyle(color) {
    localStorage.setItem("currentColor", color);

    if (color == "dark") {
        // Enable dark mode, disable light mode
        document.getElementById('darkStyle').removeAttribute('disabled');
        document.getElementById('lightStyle').setAttribute('disabled', 'true');
    } else {
        // Enable light mode, disable dark mode
        document.getElementById('lightStyle').removeAttribute('disabled');
        document.getElementById('darkStyle').setAttribute('disabled', 'true');
    }

    // Optionally, save the preference to localStorage
    localStorage.setItem('theme', this.checked ? 'dark' : 'light');
}