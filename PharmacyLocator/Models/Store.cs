using PharmacyLocator.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyLocator.Models
{
    public class Store : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Medicine")]
        public long MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }

        [ForeignKey("Pharmacy")]
        public long PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }

    }
}