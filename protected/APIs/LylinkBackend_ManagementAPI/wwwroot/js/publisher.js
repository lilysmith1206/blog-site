const isDraftInput = document.getElementById('isDraftInput');
const isDraft = document.getElementById('isDraft');

isDraftInput.addEventListener('change', function () {
    if (isDraftInput.checked) {
        isDraft.value = "true";
    } else {
        isDraft.value = "false";
    }
});

function getSlugBody(slugBox) {
    fetch(`/getPostFromId?id=${slugBox.value}`, {
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
        console.log(data);

        document.getElementById('title').value = data.title;
        document.getElementById('keywords').value = data.keywords;
        document.getElementById('description').value = data.description;
        document.getElementById('name').value = data.name;
        document.getElementById('slug').value = data.slug;
        document.getElementById('isDraft').value = data.isDraft == true;
        document.getElementById('isDraftInput').checked = data.isDraft == true;

        htmlTextView.textContent = beautify.html(data.body, options);
        renderedView.innerHTML = data.body;
        bodyInputElement.value = beautify.html(data.body, options);

        if (data.parentId === undefined) {
            document.getElementById('categoryBox').value = 'none';
        }
        else {
            document.getElementById('categoryBox').value = data.parentId;
        }
    })
    .catch(error => {
        console.error('There was a problem with the fetch operation:', error);
    });
}
