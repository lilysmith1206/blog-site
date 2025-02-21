using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace LylinkBackend_API.Controllers
{
    [ApiController]
    public class AnnotationController(IAnnotationRepository annotationDatabase) : Controller
    {
        [HttpGet("GetAnnotations")]
        public IActionResult GetAnnotations([FromQuery] string slug, [FromQuery] string editorName)
        {
            IEnumerable<Annotation> annotations = annotationDatabase.GetAnnotations(slug, editorName);

            return Ok(annotations.Select(annotation => annotation.AnnotationContent));
        }

        [HttpPost("CreateAnnotation")]
        public IActionResult CreateAnnotation([FromBody] CreateAnnotationRequest request)
        {
            var annotation = new Annotation
            {
                AnnotationContent = request.AnnotationContent,
                EditorName = request.EditorName,
                Slug = request.Slug,
                Id = request.AnnotationId
            };

            string? annotationId = annotationDatabase.CreateAnnotation(annotation);

            if (annotationId != null)
            {
                return Ok(annotationId);
            }

            return StatusCode(500);
        }

        [HttpPut("UpdateAnnotation")]
        public IActionResult UpdateAnnotation([FromBody] UpdateAnnotationRequest request)
        {
            var annotation = annotationDatabase.GetAnnotation(request.AnnotationId);

            if (annotation == null)
                return NotFound();

            annotation.AnnotationContent = request.AnnotationContent;
            annotationDatabase.UpdateAnnotation(annotation);

            return Ok();
        }

        [HttpDelete("DeleteAnnotation")]
        public IActionResult DeleteAnnotation([FromQuery] string annotationId)
        {
            bool successfulDelete = annotationDatabase.DeleteAnnotation(annotationId);

            return successfulDelete ? Ok() : NotFound();
        }
    }

    public struct CreateAnnotationRequest
    {
        public string Slug { get; set; }

        public string EditorName { get; set; }

        public string AnnotationContent { get; set; }

        public string AnnotationId { get; set; }
    }

    public struct UpdateAnnotationRequest
    {
        public string AnnotationId { get; set; }

        public string AnnotationContent { get; set; }
    }
}
