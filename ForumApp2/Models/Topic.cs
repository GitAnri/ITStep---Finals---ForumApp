namespace ForumApp2.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string Author { get; set; }
        public TopicState State { get; set; } = TopicState.Pending;
        public TopicStatus Status { get; set; } = TopicStatus.Active;
        public ICollection<Comment> Comments { get; set; }
    }

}
