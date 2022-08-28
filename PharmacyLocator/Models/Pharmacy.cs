using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;
using PharmacyLocator.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyLocator.Models
{
    [Index(nameof(Pharmacy.Username), IsUnique = true)]
    public class Pharmacy : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "Pharmacy name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Username")]
        [StringLength(20, ErrorMessage = "Username charcters length must be with in 4 - 20 letters.", MinimumLength = 4)]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^([a-zA-Z0-9@*#]{8,15})$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public int? Lng { get; set; }
        public int? Lat { get; set; }
        
        [StringLength(13,MinimumLength = 10)]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [ForeignKey("Location")]
        public long? LocationId { get; set; }
        public virtual Location Location { get; set; }

        [ForeignKey("Admin")]
        public long? AddBy { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
