namespace TrainingAPI.DTO
{
    public class PatientResponseDto
    {
        public Guid Id { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Active { get; set; }
        public NameDto Name { get; set; }
    }
}
