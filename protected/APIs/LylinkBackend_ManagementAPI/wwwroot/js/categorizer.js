function getSlugBody(slugBox) {
    if (slugBox.value == "none") {
        return;
    }

    fetch(`/getPostCategoryFromId?categoryId=${slugBox.value}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json' // Assuming you're sending JSON
        },
    })
    .then(async response => {
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }

        response = await response.text();

        return JSON.parse(response);
    })
    .then(data => {
        document.getElementById('title').value = data.title;
        document.getElementById('keywords').value = data.keywords;
        document.getElementById('description').value = data.description;
        document.getElementById('name').value = data.name;
        document.getElementById('slug').value = data.slug;
        document.getElementById('parentCategoryBox').value = data.parentId;
        document.getElementById('sortingMethods').value = data.postSortingMethod;

        htmlTextView.textContent = beautify.html(data.body, options);
        renderedView.innerHTML = data.body;
        bodyInputElement.value = beautify.html(data.body, options);

        document.getElementById("rendered").placeholder = "";
    })
    .catch(error => {
        console.error('There was a problem with the fetch operation:', error);
    });
}
