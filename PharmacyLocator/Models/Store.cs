using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyLocator.Models
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Medicine")]
        public int? MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }

        [ForeignKey("Pharmacy")]
        public int? PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }

        public double Price { get; set; }
        public int Amount { get; set; }
        public int AddedAt { get; set; }
    }
}