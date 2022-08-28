using PharmacyLocator.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyLocator.Models
{
    public class Medicine : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required(ErrorMessage = "Medicine name is required")]
        [Display(Name = "Name")]
        [StringLength(75, ErrorMessage = "Medicine Name can not be more than 75 characters")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }

        [ForeignKey("Admin")]
        public long AddBy { get; set;}
        public virtual Admin Admin { get; set; }
    }
}
