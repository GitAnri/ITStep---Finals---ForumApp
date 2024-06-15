using ForumApp2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ForumApp2.Interface
{

    public interface IUserRepository
    {
        Task<Topic> GetTopicByIdAsync(int topicId);
        Task<IEnumerable<Topic>> GetAllTopicsAsync();
        Task<IEnumerable<Comment>> GetCommentsByTopicIdAsync(int topicId);
        Task CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int commentId);
        Task CreateTopicAsync(Topic topic);
    }

    //Task BanUserAsync(int userId);
    //Task UnbanUserAsync(int userId);
}


