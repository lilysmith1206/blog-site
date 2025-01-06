let currentColorStatus = localStorage.getItem("currentColor");
let darkMoon = document.getElementById("darkMoon");
let lightSun = document.getElementById("lightSun");

if (currentColorStatus !== null) {
    if (currentColorStatus == "dark") {
        showDarkMode();

        darkMoon.style.display = 'inline';
    }
    else if (currentColorStatus == "light") {
        showLightMode();

        lightSun.style.display = 'inline';
    }
}
else {
    localStorage.setItem('theme', 'light');
	
	showLightMode();
}

document.getElementById("darkMoon").addEventListener("click", handleDarkMoonClick);
document.getElementById("lightSun").addEventListener("click", handleLightSunClick);

setTimeout(() => {
    document.getElementById('colorTransitionStyle').removeAttribute('disabled');
}, 300)

function setColorStyle(color) {
    localStorage.setItem("currentColor", color);

    if (color == "dark") {
        hideImage(lightSun, darkMoon);

        showDarkMode();
    }
    else if (color == "light") {
        hideImage(darkMoon, lightSun);

        showLightMode();
    }
    else {
        throw `${currentColorStatus} is not a valid color style.`;
    }

    localStorage.setItem('theme', this.checked ? 'dark' : 'light');
}

function hideImage(element, newElement) {
    newElement.style.display = 'inline';

    element.classList.add('hideImage');
    newElement.classList.add('showImage');

    setTimeout(() => {
        element.style.display = 'none';

        element.classList.remove('hideImage');
        newElement.classList.remove('showImage');
    }, 300);
}

function showDarkMode() {
    document.body.classList.add('dark-mode');
}

function showLightMode() {
    document.body.classList.remove('dark-mode');
}

function handleDarkMoonClick() {
    setColorStyle("light");
}

function handleLightSunClick() {
    setColorStyle("dark");
}