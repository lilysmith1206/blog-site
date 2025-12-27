window.setTheme = (color) => {
    if (!["dark", "light"].includes(color)) {
        return;
    }

    localStorage.setItem("currentColor", color);

    if (color === "dark") {
        document.documentElement.classList.add('dark-mode');
    } else {
        document.documentElement.classList.remove('dark-mode');
    }
}

const stored = localStorage.getItem("currentColor") ?? "light";
setTimeout(() => {
    const style = document.createElement('style');
    style.innerHTML = `
* {
    transition: background-color 0.5s ease, color 0.5s ease, border-color 0.5s ease;
}
`;
    document.head.appendChild(style);
}, 500)
setTheme(stored);
