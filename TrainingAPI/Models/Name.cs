using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TrainingAPI.Models
{
    public class Name
    {
        [Key]
        public Guid ID { get; set; }
        public Patient Patient { get; set; }
        public Guid PatientId { get; set; }
        public string Use { get; set; }
        [Required]
        public string Family { get; set; }
        public string GivenSerialized { get; set; } = string.Empty;
        [NotMapped]
        public List<string> Given
        {
            get => string.IsNullOrEmpty(GivenSerialized)
                ? new List<string>()
                : new List<string>(GivenSerialized.Split(','));
            set => GivenSerialized = string.Join(",", value);
        }
    }
}