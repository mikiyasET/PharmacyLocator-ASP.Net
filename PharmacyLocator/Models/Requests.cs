using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyLocator.Models
{
    [Index(nameof(Requests.Name), IsUnique = true)]
    public class Requests : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "Medicine name is required")]
        [Display(Name = "Medicine Name")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

    }
}