using System.ComponentModel.DataAnnotations;

namespace librarwebapp.Models
{
    public class TwoFactorLoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
