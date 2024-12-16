using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Property
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Required]
    public string Location { get; set; }
    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Area must be greater than or equal to 1.")]
    public double Area { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Rooms must be greater than or equal to 1.")]
    public string Rooms { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Bathrooms must be greater than or equal to 1.")]
    public int Bathrooms { get; set; }
    public List<string> ImageUrls { get; set; }

    [NotMapped]
    public List<IFormFile> Images { get; set; }

    public bool Favorite { get; set; } = false;
    public bool UnderConstruction { get; set; } = false;

}
