function GetUrlSlug() {
    // Get the full URL
    const url = window.location.href;

    // Extract the slug from the URL
    const urlParts = url.split('/');
    const slug = urlParts[urlParts.length - 1].split('?')[0]; // Get part before any query parameters

    return slug;
}

function GetAnnotatorName() {
    // Get the query parameters from the URL
    const params = new URLSearchParams(window.location.search);

    // Get the 'editor' parameter value
    const editor = params.get('editor');

    return editor;
}

var r = Recogito.init({
    content: document.getElementsByTagName('main')[0] // Replace with your content element
});

const slug = GetUrlSlug();
const editorName = GetAnnotatorName();

function loadAnnotations() {
    var params = new URLSearchParams({
        slug: slug,
        editorName: editorName
    });

    fetch('/GetAnnotations?' + params.toString(), {
        method: 'GET'
    })
    .then(response => response.json())
    .then(annotations => {
        // Load annotations into Recogito
        annotations = annotations.map(annotation => JSON.parse(annotation));

        console.log(annotations);

        r.setAnnotations(annotations);
    })
    .catch(error => console.error('Error fetching annotations:', error));
}

    // Call the function to load annotations
loadAnnotations();

    // Handle createAnnotation event
r.on('createAnnotation', function (annotation, overrideId) {

    console.log(overrideId);

    fetch('/CreateAnnotation', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            slug: slug,
            editorName: editorName,
            annotationContent: JSON.stringify(annotation),
            annotationId: annotation.id
        })
    })
    .then(response => {
        return response.text();
    })
    .then(annotationId => {
        console.log('Annotation created with ID:', annotationId);
    })
    .catch(error => console.error('Error creating annotation:', error));
});

    // Handle updateAnnotation event
r.on('updateAnnotation', function(annotation, previous) {
    fetch('/UpdateAnnotation', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            annotationId: annotation.id, // Ensure the annotation has an 'id' property
            annotationContent: JSON.stringify(annotation)
        })
    })
    .then(response => {
        if (response.ok) {
            console.log('Annotation updated successfully');
        } else {
            console.error('Error updating annotation:', response.statusText);
        }
    })
    .catch(error => console.error('Error updating annotation:', error));
});

    // Handle deleteAnnotation event
r.on('deleteAnnotation', function(annotation) {
    var params = new URLSearchParams({
        annotationId: annotation.id
    });

    fetch('/DeleteAnnotation?' + params.toString(), {
        method: 'DELETE'
    })
    .then(response => {
        if (response.ok) {
        console.log('Annotation deleted successfully');
        } else {
        console.error('Error deleting annotation:', response.statusText);
        }
    })
    .catch(error => console.error('Error deleting annotation:', error));
});
