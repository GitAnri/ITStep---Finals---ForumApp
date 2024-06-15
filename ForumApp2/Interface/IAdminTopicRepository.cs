using ForumApp2.Models;

namespace ForumApp2.Interface
{
    public interface IAdminTopicRepository
    {
        Task<IEnumerable<Topic>> GetAllTopicsAsync();
        Task<Topic> GetTopicByIdAsync(int topicId);
        Task ChangeTopicStateAsync(int topicId, String newState);
        Task ChangeTopicStatusAsync(int topicId, TopicStatus newStatus);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task BanUserAsync(int userId);
        Task UnbanUserAsync(int userId);
    }

}
