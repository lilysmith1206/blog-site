let currentColorStatus = localStorage.getItem("currentColor");
let darkMoon = document.getElementById("darkMoon");
let lightSun = document.getElementById("lightSun");

setTimeout(() => {
    const style = document.createElement('style');
    style.innerHTML = `
* {
    transition: background-color 0.5s ease, color 0.5s ease, border-color 0.5s ease;
}
`;

    document.head.appendChild(style);
}, 500)

const validColorOptions = ["dark", "light"];

if (currentColorStatus !== null && validColorOptions.includes(currentColorStatus)) {
    setColorStyle(currentColorStatus);
}
else {
    localStorage.setItem('currentColor', 'light');

	showLightMode();
}

document.getElementById("darkMoon").addEventListener("click", handleDarkMoonClick);
document.getElementById("lightSun").addEventListener("click", handleLightSunClick);

function setColorStyle(color) {
    localStorage.setItem("currentColor", color);

    darkMoon.classList.remove('shown', 'hidden');
    lightSun.classList.remove('shown', 'hidden');

    if (color == "dark") {
        darkMoon.classList.add('shown');
        lightSun.classList.add('hidden');

        showDarkMode();
    }
    else if (color == "light") {
        lightSun.classList.add('shown');
        darkMoon.classList.add('hidden');

        showLightMode();
    }
    else {
        throw `${currentColorStatus} is not a valid color style.`;
    }
}

function showDarkMode() {
    document.documentElement.classList.add('dark-mode');
}

function showLightMode() {
    document.documentElement.classList.remove('dark-mode');
}

function handleDarkMoonClick() {
    setColorStyle("light");
}

function handleLightSunClick() {
    setColorStyle("dark");
}