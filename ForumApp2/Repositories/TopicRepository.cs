using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumApp2.Data;
using ForumApp2.Interface;
using ForumApp2.Models;
using Microsoft.EntityFrameworkCore;
namespace ForumApp2.Repositories
{

    public class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _context;

        public TopicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
        {
            return await _context.Topics.ToListAsync();
        }

        public async Task<Topic> GetTopicByIdAsync(int topicId)
        {
            return await _context.Topics.FindAsync(topicId);
        }


        public async Task UpdateTopicAsync(Topic topic)
        {
            _context.Entry(topic).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTopicAsync(int topicId)
        {
            var topic = await _context.Topics.FindAsync(topicId);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
            }
        }
    }

}
