namespace DataLab.Dto.Psql
{
    public class DisciplineDto
    {
        public int Id { get; set; }
        public string DiscpId { get; set; }
        public bool IsNewStandard { get; set; }
        public string   Name { get; set; }
        public bool IsFullTime { get; set; }
        public string Faculty { get; set; }
        public SpecialisationDto SpecialisationDto { get; set; }
        public int Semester { get; set; }
        public int LectureTime { get; set; }
        public int PracticeTime { get; set; }
        public int LabTime { get; set; }
        public bool IsExam { get; set; }
        public string TeacherId { get; set; }
        public string TeacherFio { get; set; }
    }
}