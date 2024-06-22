using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using ForumApp2.Data;
using ForumApp2.Interface;
using ForumApp2.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateTopicAsync(Topic topic)
    {
        if (topic == null)
        {
            throw new ArgumentNullException(nameof(topic));
        }
        _context.Topics.Add(topic);
        await _context.SaveChangesAsync();
    }

    public async Task<Topic> GetTopicByIdAsync(int topicId)
    {
        return await _context.Topics
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == topicId);
    }


    public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
    {
        return await _context.Topics.ToListAsync();
    }


    public async Task CreateCommentAsync(Comment comment)
    {
        if (comment == null)
            throw new ArgumentNullException(nameof(comment));

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        if (comment == null)
            throw new ArgumentNullException(nameof(comment));

        _context.Entry(comment).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int commentId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteTopicAsync(int id)
    {
        var topic = await _context.Topics.FindAsync(id);
        if (topic != null)
        {
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
        }
    }
}
