namespace TrainingAPI.DTO
{
    public class NameDto
    {
        public string Use { get; set; }
        public string Family { get; set; }
        public List<string> Given { get; set; } = new List<string>();
    }
}
