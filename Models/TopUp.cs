using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopUpMVC.Models
{
    public class TopUp
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Player ID is required.")]
        [StringLength(50, ErrorMessage = "Player ID cannot exceed 50 characters.")]
        public string PlayerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Player Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string PlayerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Game is required.")]
        public string Game { get; set; } = string.Empty;

        [Required(ErrorMessage = "Package is required.")]
        public string Package { get; set; } = string.Empty;

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, 99999.99, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
