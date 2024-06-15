using ForumApp2.Data;
using ForumApp2.Interface;
using ForumApp2.Models;
using Microsoft.EntityFrameworkCore;


namespace ForumApp2.Repositories
{
    public class AdminTopicRepository : IAdminTopicRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminTopicRepository(ApplicationDbContext context)
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

        public async Task ChangeTopicStateAsync(int topicId, String newState)
        {
            var topic = await _context.Topics.FindAsync(topicId);
            if (topic != null)
            {
                topic.State = newState;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ChangeTopicStatusAsync(int topicId, TopicStatus newStatus)
        {
            var topic = await _context.Topics.FindAsync(topicId);
            if (topic != null)
            {
                topic.Status = newStatus;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task BanUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsBanned = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnbanUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsBanned = false;
                await _context.SaveChangesAsync();
            }
        }
    }

}
