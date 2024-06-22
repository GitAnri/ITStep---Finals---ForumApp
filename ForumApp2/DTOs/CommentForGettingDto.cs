using ForumApp2.Models;

namespace ForumApp2.DTOs
{
    public class CommentForGettingDto
    {
        public int TopicId { get; set; }
        public int Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
