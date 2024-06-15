using ForumApp2.Interface;
using ForumApp2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ForumApp2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("GetAllCommentFromTopic/{topicId}")]
        public async Task<IActionResult> GetCommentsByTopicId(int topicId)
        {
            var comments = await _commentRepository.GetCommentsByTopicIdAsync(topicId);
            return Ok(comments);
        }

        [NonAction]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }
    }

}
