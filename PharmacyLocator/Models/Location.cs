using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyLocator.Models
{
    [Index(nameof(Location.Name), IsUnique = true)]
    public class Location : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "Location name is required")]
        [Display(Name = "Name")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
    }
}