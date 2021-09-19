namespace DataLab.Dto.Oracle
{
    public class Discipline
    {
        public int Id { get; set; }
        public string TeacherId { get; set; }
        public string TeacherFio { get; set; }
        public string DiscpId { get; set; }
        public string Specialisation { get; set; }
        public bool IsFullTime { get; set; }
    }
}