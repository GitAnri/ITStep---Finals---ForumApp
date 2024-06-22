using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ForumApp2.Data;
using ForumApp2.DTOs;
using ForumApp2.Interface;
using ForumApp2.Models;
using ForumApp2.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//[Authorize(Roles = "User")]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public UserController(IUserRepository userRepository, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _context = context;
    }

    [HttpPost("PostTopic")]
    public async Task<IActionResult> PostTopic([FromForm] TopicPostDto topicDto)
    {
        if (topicDto == null)
        {
            return BadRequest();
        }

        var author = "User";

        var topic = _mapper.Map<Topic>(topicDto);
        topic.Author = author;
        topic.CreationDate = DateTime.UtcNow;
        topic.Status = TopicStatus.Active;
        topic.State = TopicState.Pending;

        try
        {
            await _userRepository.CreateTopicAsync(topic);

            var createdTopicDto = _mapper.Map<TopicPostDto>(topic);
            return CreatedAtAction(nameof(GetTopicById), new { id = topic.Id }, createdTopicDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpDelete("DeleteTopicById/{id}")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        var topic = await _userRepository.GetTopicByIdAsync(id);
        if (topic == null)
        {
            return NotFound();
        }
        try
        {
            await _userRepository.DeleteTopicAsync(id);
            return NoContent();
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
        var topicDto = _mapper.Map<TopicGetDto>(topic);
        return Ok(topicDto);
    }

    [HttpGet("GetAlltopics")]
    public async Task<IActionResult> GetAllTopics()
    {
        var topics = await _userRepository.GetAllTopicsAsync();
        var topicDtos = _mapper.Map<IEnumerable<TopicPostDto>>(topics);
        return Ok(topicDtos);
    }

    [HttpPost("CreateComment")]
    public async Task<IActionResult> CreateComment([FromForm] CommentDto commentDto)
    {
        if (commentDto == null)
        {
            return BadRequest();
        }

        var author = "User";


        var comment = _mapper.Map<Comment>(commentDto);
        comment.Author = author;
        comment.CreationDate = DateTime.UtcNow;
        var topic = await _context.Topics.FindAsync(commentDto.TopicId);

        if (topic == null || topic.Status != TopicStatus.Active)
        {
            return BadRequest("Topic not found or is inactive.");
        }

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
    public async Task<IActionResult> UpdateComment([FromForm] CommentUpdateDto commentUpd)
    {
        var existingComment = await _context.Comments.FindAsync(commentUpd.Id);

        if (existingComment == null)
        {
            return NotFound(); 
        }

        _mapper.Map(commentUpd, existingComment);

        try
        {
            _context.Entry(existingComment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("DeleteCommentById/{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        await _userRepository.DeleteCommentAsync(id);
        return NoContent();
    }
}
