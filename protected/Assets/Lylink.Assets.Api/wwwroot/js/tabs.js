const containers = document.querySelectorAll('.tab-container');

containers.forEach(container => {
    const buttons = container.querySelectorAll(':scope > .tab-buttons .tab-button');
    const contents = container.querySelectorAll(':scope > .tab-content');

    function activateTab(tabId) {
        contents.forEach(content => {
            content.style.display = 'none';
        });

        buttons.forEach(button => {
            button.classList.remove('active');
        });

        const selectedContent = container.querySelector(`:scope > .tab-content#${tabId}`);
        if (selectedContent) {
            selectedContent.style.display = 'block';
        }

        buttons.forEach(button => {
            if (button.getAttribute('data-tab') === tabId) {
                button.classList.add('active');
            }
        });
    }

    buttons.forEach(button => {
        button.addEventListener('click', () => {
            const tabId = button.getAttribute('data-tab');
            activateTab(tabId);
        });
    });

    if (buttons.length) {
        activateTab(buttons[0].getAttribute('data-tab'));
    }
});
