using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class AnnotationsRepository(LylinkdbContext context) : IAnnotationRepository
    {
        public IEnumerable<Annotation> GetAnnotations(string slug, string editorName)
        {
            return context.Annotations
                .Where(annotation => slug == annotation.Slug && annotation.EditorName == editorName)
                .ToList();
        }

        public string? CreateAnnotation(Annotation annotation)
        {
            context.Annotations.Add(annotation);

            context.SaveChanges();

            return annotation.Id;
        }

        public bool UpdateAnnotation(Annotation annotation)
        {
            var currentAnnotation = context.Annotations.Single(dbAnnotation => dbAnnotation.Id == annotation.Id);

            currentAnnotation.AnnotationContent = annotation.AnnotationContent;
            currentAnnotation.EditorName = annotation.EditorName;
            currentAnnotation.Slug = annotation.Slug;

            return context.SaveChanges() == 1;
        }

        public bool DeleteAnnotation(string annotationId)
        {
            Annotation? annotation = context.Annotations.SingleOrDefault(annotation => annotation.Id == annotationId);

            if (annotation == null)
            {
                return false;
            }

            context.Annotations.Remove(annotation);

            return context.SaveChanges() == 1;
        }

        public bool DeleteAnnotation(Annotation annotation)
        {
            context.Annotations.Remove(annotation);

            return context.SaveChanges() == 1;
        }

        public Annotation? GetAnnotation(string id)
        {
            return context.Annotations.SingleOrDefault(annotation => annotation.Id == id);
        }

    }
}
