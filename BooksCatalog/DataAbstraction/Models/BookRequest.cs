using System.ComponentModel.DataAnnotations;

namespace DataAbstraction.Models
{
    public class BookRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        public string Category { get; set; } = "Other";
        public decimal Price { get; set; }
        public string Description { get; set; } = "Description not set";
    }
}
