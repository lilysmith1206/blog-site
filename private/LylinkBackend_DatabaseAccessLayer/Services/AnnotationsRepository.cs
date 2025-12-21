using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class AnnotationsRepository(IDbContextFactory<LylinkdbContext> contextFactory) : IAnnotationRepository
    {
        public IEnumerable<Annotation> GetAnnotations(string slug, string editorName)
        {
            try
            {
                using var context = contextFactory.CreateDbContext();

                return context.Annotations
                    .Where(annotation => slug == annotation.Slug && annotation.EditorName == editorName)
                    .ToList();
            }
            catch (MySqlException)
            {
                return [];
            }
        }

        public string? CreateAnnotation(Annotation annotation)
        {
            try
            {
                using var context = contextFactory.CreateDbContext();

                context.Annotations.Add(annotation);

                context.SaveChanges();

                return annotation.Id;
            }
            catch (MySqlException)
            {
                return null;
            }
        }

        public bool UpdateAnnotation(Annotation annotation)
        {
            try
            {
                using var context = contextFactory.CreateDbContext();

                var currentAnnotation = context.Annotations.Single(dbAnnotation => dbAnnotation.Id == annotation.Id);

                currentAnnotation.AnnotationContent = annotation.AnnotationContent;
                currentAnnotation.EditorName = annotation.EditorName;
                currentAnnotation.Slug = annotation.Slug;

                return context.SaveChanges() == 1;
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        public bool DeleteAnnotation(string annotationId)
        {
            try
            {
                using var context = contextFactory.CreateDbContext();

                Annotation? annotation = context.Annotations.SingleOrDefault(annotation => annotation.Id == annotationId);

                if (annotation == null)
                {
                    return false;
                }

                context.Annotations.Remove(annotation);

                return context.SaveChanges() == 1;
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        public bool DeleteAnnotation(Annotation annotation)
        {
            try
            {
                using var context = contextFactory.CreateDbContext();
                
                context.Annotations.Remove(annotation);

                return context.SaveChanges() == 1;
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        public Annotation? GetAnnotation(string id)
        {
            try
            {
                using var context = contextFactory.CreateDbContext();

                return context.Annotations.SingleOrDefault(annotation => annotation.Id == id);
            }
            catch (MySqlException)
            {
                return null;
            }
        }

    }
}
