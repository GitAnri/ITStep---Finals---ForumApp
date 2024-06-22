namespace ForumApp2.DTOs
{
    public class TopicGetDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<CommentForGettingDto> Comments { get; set; }
    }
}
