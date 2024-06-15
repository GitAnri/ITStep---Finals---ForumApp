using ForumApp2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ForumApp2.Interface
{

    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsByTopicIdAsync(int topicId);
        Task<Comment> GetCommentByIdAsync(int commentId);
    }

}
