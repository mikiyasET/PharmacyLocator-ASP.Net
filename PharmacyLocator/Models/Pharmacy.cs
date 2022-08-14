using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyLocator.Models
{
    public class Pharmacy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string? Name { get; set; }
        public int Lng { get; set; }
        public int Lat { get; set; }
        [StringLength(10)]
        public string Phone { get; set; }

        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public virtual Location Location { get; set; }

        [ForeignKey("Admin")]
        public int? AddBy { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
