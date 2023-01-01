using Microsoft.EntityFrameworkCore;
using PharmacyLocator.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyLocator.Models
{
    [Index(nameof(Medicine.Name), IsUnique = true)]
    public class Medicine : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "Medicine name is required")]
        [Display(Name = "Name")]
        [StringLength(75, ErrorMessage = "Medicine Name can not be more than 75 characters")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Description { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Image { get; set; }
        
        [ForeignKey("Admin")]
        public long AddBy { get; set;}
        public virtual Admin Admin { get; set; }
    }
}
