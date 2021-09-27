namespace DataLab.Dto.RuleDto
{
    public class Fact1
    {
        public int Id { get; set; }
        public Time Time { get; set; }
        public int CountRedDiplomas { get; set; }
        public int CountBlueDiplomas { get; set; }
        public int CountPublication { get; set; }
        public int CountConference { get; set; }

        public int ActiveReadTickets { get; set; }
        public int ActiveReadTeacherTickets { get; set; }
        public int CountInBuilding { get; set; }
        public int CountOutOfBuilding { get; set; }
        
    }
}