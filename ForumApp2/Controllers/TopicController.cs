using AutoMapper;
using ForumApp2.DTOs;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public TopicController(IUserRepository userRepository, IMapper mapper)
        {
           _mapper = mapper;
            _userRepository = userRepository;
        }


        [HttpGet("GetAlltopics")]
        public async Task<IActionResult> GetAllTopics()
        {
            var topics = await _userRepository.GetAllTopicsAsync();
            var topicDtos = _mapper.Map<IEnumerable<TopicPostDto>>(topics);
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
    }

}
