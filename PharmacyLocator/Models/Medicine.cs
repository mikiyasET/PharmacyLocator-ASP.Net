using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyLocator.Models
{
    public class Medicine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string? Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        [ForeignKey("Admin")]
        public int? AddBy { get; set;}
        public virtual Admin Admin { get; set; }
    }
}