using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface IAnnotationDatabaseService
    {
        public Annotation? GetAnnotation(string id);

        public IEnumerable<Annotation> GetAnnotations(string slug, string editorName);

        public string? CreateAnnotation(Annotation annotation);

        public bool UpdateAnnotation(Annotation annotation);

        public bool DeleteAnnotation(string annotationId);

        public bool DeleteAnnotation(Annotation annotation);
    }
}
