namespace HighLoadChan.Presentation.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ThreadWebModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class PostWebModel
    {
        [Required]
        public int ThreadId { get; set; }
        
        public string Author { get; set; }

        [Required]
        public string Content { get; set; }
    }
}