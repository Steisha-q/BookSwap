using System.ComponentModel.DataAnnotations;

namespace BookSwap.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        public string? Genre { get; set; }

        [Display(Name = "Publication Year")]
        public int? PublicationYear { get; set; }

        public string? Publisher { get; set; }

        public string? ISBN { get; set; }

        public string? Language { get; set; }

        public string? Description { get; set; }

        public string? Condition { get; set; }

        public string? City { get; set; }

        [Display(Name = "Book Cover")]
        public string? ImagePath { get; set; }
    }
}