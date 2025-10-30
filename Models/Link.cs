namespace Resumai.Models
{
    public class Link
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public Guid ResumeId { get; set; }
        public Resume Resume { get; set; } = null!;
        public Link()
        {
        }
        public Link(string url, Guid resumeId)
        {
            Id = Guid.NewGuid();
            Url = url;
            ResumeId = resumeId;
        }
    }
}