namespace DataLab.Dto.RuleDto
{
    public class ConferenceParticipationDto
    {
        public int Id { get; set; }
        public StudentDto StudentDto { get; set; }
        public ConferenceDto  ConferenceDto{ get; set; }
    }
}