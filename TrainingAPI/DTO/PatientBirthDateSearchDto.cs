namespace TrainingAPI.DTO
{
    public class PatientBirthDateSearchDto
    {
        public DateTime? ExactDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? BeforeDate { get; set; }
        public DateTime? AfterDate { get; set; }
    }
}
