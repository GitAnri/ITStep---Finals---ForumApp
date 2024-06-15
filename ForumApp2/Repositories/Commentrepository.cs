using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForumApp2.Data;
using ForumApp2.Interface;
using ForumApp2.Models;
using Microsoft.EntityFrameworkCore;
namespace ForumApp2.Repositories
{

    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByTopicIdAsync(int topicId)
        {
            return await _context.Comments.Where(c => c.TopicId == topicId).ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }
    }

}
