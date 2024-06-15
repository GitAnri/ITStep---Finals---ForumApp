using ForumApp2.Interface;
using ForumApp2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumApp2.Controllers
{
    [ApiController]
    [Route("api/admin")]
   // [Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminTopicRepository _adminTopicRepository;

        public AdminController(IAdminTopicRepository adminTopicRepository)
        {
            _adminTopicRepository = adminTopicRepository;
        }

        [HttpGet("GetAllTopics")]
        public async Task<IActionResult> GetAllTopics()
        {
            var topics = await _adminTopicRepository.GetAllTopicsAsync();
            return Ok(topics);
        }

        [HttpGet("GetTopicById/{id}")]
        public async Task<IActionResult> GetTopicById(int id)
        {
            var topic = await _adminTopicRepository.GetTopicByIdAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return Ok(topic);
        }

        [HttpPost("ChangeTopicState/{id}/{newState}")]
        public async Task<IActionResult> ChangeTopicState(int id, String newState)
        {
            await _adminTopicRepository.ChangeTopicStateAsync(id, newState);
            return NoContent();
        }

        [HttpPost("ChangeTopicStatus/{id}/{newStatus}")]
        public async Task<IActionResult> ChangeTopicStatus(int id, TopicStatus newStatus)
        {
            await _adminTopicRepository.ChangeTopicStatusAsync(id, newStatus);
            return NoContent();
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminTopicRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("BanUserById/{userId}")]
        public async Task<IActionResult> BanUser(int userId)
        {
            await _adminTopicRepository.BanUserAsync(userId);
            return NoContent();
        }

        [HttpPost("UnbanUserById/{userId}")]
        public async Task<IActionResult> UnbanUser(int userId)
        {
            await _adminTopicRepository.UnbanUserAsync(userId);
            return NoContent();
        }
    }

}
