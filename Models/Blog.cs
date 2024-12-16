using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Allurerealstate.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public List<string> ImageUrls { get; set; }
        [NotMapped]
        public List<IFormFile> Images { get; set; }
    }
}
