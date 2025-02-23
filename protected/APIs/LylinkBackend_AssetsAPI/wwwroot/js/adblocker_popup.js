function showAdblockPopup() {
    const adblockPopup = document.getElementById('adblock-popup');
    const adblockBackground = document.getElementById('adblock-background');

    if (adblockPopup === null || adblockBackground === null) {
        document.addEventListener("DOMContentLoaded", () => {
            document.getElementById('adblock-popup').style.display = 'block';
            document.getElementById('adblock-background').style.display = 'block';
        });
    }
    else {
        adblockPopup.style.display = 'block';
        adblockBackground.style.display = 'block';
    }
}

function closePopup() {
    document.getElementById('adblock-popup').style.display = 'none';
    document.getElementById('adblock-background').style.display = 'none';
}