namespace DataLab.Dto.Psql
{
    public class SpecialisationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public University University { get; set; }
    }
}