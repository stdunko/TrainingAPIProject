using TrainingAPI.Enums;

namespace TrainingAPI.DTO
{
    public class PatientCreateDto
    {
        public GenderEnum Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public ActiveEnum Active { get; set; }
        public NameDto Name { get; set; }
    }
}
