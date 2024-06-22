using AutoMapper;
using ForumApp2.DTOs;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AdminController(IAdminTopicRepository adminTopicRepository, IUserRepository userRepository, IMapper mapper)
        {
            _adminTopicRepository = adminTopicRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet("GetAlltopics")]
        public async Task<IActionResult> GetAllTopics()
        {
            var topics = await _userRepository.GetAllTopicsAsync();
            var topicDtos = _mapper.Map<IEnumerable<TopicGetDto>>(topics);
            return Ok(topicDtos);
        }

        [HttpGet("GetTopicById/{id}")]
        public async Task<IActionResult> GetTopicById(int id)
        {
            var topic = await _userRepository.GetTopicByIdAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            var topicDto = _mapper.Map<TopicGetDto>(topic);
            return Ok(topicDto);
        }

        [HttpPost("ChangeTopicState/{id}/{newState}")]
        public async Task<IActionResult> ChangeTopicState(int id, TopicState newState)
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
        public async Task<IActionResult> BanUser(string userId)
        {
            await _adminTopicRepository.BanUserAsync(userId);
            return NoContent();
        }

        [HttpPost("UnbanUserById/{userId}")]
        public async Task<IActionResult> UnbanUser(string userId)
        {
            await _adminTopicRepository.UnbanUserAsync(userId);
            return NoContent();
        }
    }

}
