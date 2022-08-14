using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyLocator.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        [DefaultValue(1)]
        public int Priority { get; set; }
    }
}
