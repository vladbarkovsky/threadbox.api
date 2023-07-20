namespace ThreadboxApi.Application.Posts.Models
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid ThreadId { get; set; }
        public List<string> PostImageUrls { get; set; }
    }
}