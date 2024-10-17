namespace FormsClone.Models.Templates
{
    public class Template
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Theme { get; set; }
        public string[]? Tags { get; set; }
    }
}