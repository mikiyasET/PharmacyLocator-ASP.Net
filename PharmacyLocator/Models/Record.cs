using PharmacyLocator.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyLocator.Models
{
    public class Record : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        [ForeignKey("Medicine")]
        public long MedicineId { get; set; }

        public virtual Medicine? Medicine { get; set; }

        [Required]
        [ForeignKey("User")]
        public long UserId { get; set; }
        
        public virtual User? User { get; set; }

        [DefaultValue(1)]
        public int Count { get; set; }
    }
}