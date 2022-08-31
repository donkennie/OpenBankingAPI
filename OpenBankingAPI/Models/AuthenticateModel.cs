using System.ComponentModel.DataAnnotations;

namespace OpenBankingAPI.Models
{
    public class AuthenticateModel
    {
        [Required]
        [RegularExpression(@"^[0][1-9]/d{9}$|^(1-9)\d{9}$")]
        public string AccountNumber { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must be 4-digit")]
        public string Pin { get; set; }
    }
}
