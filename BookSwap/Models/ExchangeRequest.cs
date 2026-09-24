using System.ComponentModel.DataAnnotations;

namespace BookSwap.Models
{
    public class ExchangeRequest
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Offered Book")]
        public int? OfferedBookId { get; set; }

        [Required]
        [Display(Name = "Requested Book")]
        public int? RequestedBookId { get; set; }

        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";
    }
}