using System.ComponentModel.DataAnnotations;

namespace OpenBankingAPI.Models
{
    public class UpdateAccountModel
    {
        [Key]
        public int Id { get; set; }

       // public string FirstName { get; set; }

       // public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        [RegularExpression(@"^[0-9]{4}$/", ErrorMessage = "")]
        [Required]
        public string Pin { get; set; }

        [Compare("Pin", ErrorMessage = "Pins do not match")]
        public string ComfirmPin { get; set; }

        public DateTime DateLastUpdated{ get; set; }
    }
}
