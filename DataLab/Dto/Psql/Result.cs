namespace DataLab.Dto.Psql
{
    public class Result
    {
        public int Id { get; set; }
        public int Mark { get; set; }
        public StudentDto Student { get; set; }
        public DisciplineDto Discipline { get; set; }
    }
}