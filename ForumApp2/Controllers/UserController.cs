using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ForumApp2.DTOs;
using ForumApp2.Interface;
using ForumApp2.Models;
using ForumApp2.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

//[Authorize(Roles = "User")]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;


    public UserController(IUserRepository userRepository, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [HttpPost("PostTopic")]
    //[Authorize(Roles = "User")]
    public async Task<IActionResult> PostTopic([FromForm] TopicDto topicDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        var author = User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous";

        var topic = new Topic
        {
            Author = author,
            Title = topicDto.Title,
            Content = topicDto.Content,
            CreationDate = DateTime.UtcNow,
            NumberOfComments = 0,
            Status = TopicStatus.Active,
            State = "Pending"
        };
        try
        {
            await _userRepository.CreateTopicAsync(topic);
            return CreatedAtAction(nameof(GetTopicById), new { id = topic.Id }, topic);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpGet("GetTopicById/{id}")]
    public async Task<IActionResult> GetTopicById(int id)
    {
        var topic = await _userRepository.GetTopicByIdAsync(id);
        if (topic == null)
        {
            return NotFound();
        }
        var topicDto = _mapper.Map<TopicDto>(topic);
        return Ok(topicDto);
    }

    [HttpGet("GetAlltopics")]
    public async Task<IActionResult> GetAllTopics()
    {
        var topics = await _userRepository.GetAllTopicsAsync();
        var topicDtos = _mapper.Map<IEnumerable<TopicDto>>(topics);
        return Ok(topicDtos);
    }

    [HttpGet("GetAllCommentsFromTopic/{id}")]
    public async Task<IActionResult> GetCommentsByTopicId(int id)
    {
        var comments = await _userRepository.GetCommentsByTopicIdAsync(id);
        var commentDtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
        return Ok(commentDtos);
    }

    [HttpPost("CreateComment")]
    public async Task<IActionResult> CreateComment([FromBody] CommentDto commentDto)
    {
        if (commentDto == null)
        {
            return BadRequest();
        }

        var author = User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous";


        var comment = _mapper.Map<Comment>(commentDto);
        comment.Author = author;

        try
        {
            await _userRepository.CreateCommentAsync(comment);
            var createdCommentDto = _mapper.Map<CommentDto>(comment);
            return Ok(createdCommentDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("UpdateCommentById/{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDto commentDto)
    {
        if (commentDto == null || commentDto.Id != id)
        {
            return BadRequest();
        }

        var comment = _mapper.Map<Comment>(commentDto);
        await _userRepository.UpdateCommentAsync(comment);
        return NoContent();
    }

    [HttpDelete("DeleteCommentById/{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        await _userRepository.DeleteCommentAsync(id);
        return NoContent();
    }
}
