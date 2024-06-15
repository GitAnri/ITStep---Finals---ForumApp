using ForumApp2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForumApp2.Interface
{
    public interface ITopicRepository
    {
        Task<IEnumerable<Topic>> GetAllTopicsAsync();
        Task<Topic> GetTopicByIdAsync(int topicId);
        
    }

}
