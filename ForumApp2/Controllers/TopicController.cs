using ForumApp2.Interface;
using ForumApp2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForumApp2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TopicController : ControllerBase
    {
        private readonly ITopicRepository _topicRepository;


        public TopicController(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }


        [HttpGet("GetAllTopics")]
        public async Task<IActionResult> GetAllTopics()
        {
            var topics = await _topicRepository.GetAllTopicsAsync();
            return Ok(topics);
        }

        [HttpGet("GetTopicById/{id}")]
        public async Task<IActionResult> GetTopicById(int id)
        {
            var topic = await _topicRepository.GetTopicByIdAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return Ok(topic);
        }
    }

}
