namespace ForumApp2.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string Author { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public DateTime CreatedAt { get; set; }
    }


}
