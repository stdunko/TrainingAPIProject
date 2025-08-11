using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrainingAPI.Enums;

namespace TrainingAPI.Models
{
    public class Patient
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public Name Name { get; set; }
        public Guid NameId { get; set; } 
        public GenderEnum Gender { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public ActiveEnum Active { get; set; }
    }
}
