namespace DataLab.Dto.MySql
{
    public class PublicationCoauthorDto
    {
        public int Id { get; set; }
        public StudentDto Author { get; set; }
        public PublicationDto Publication { get; set; }
    }
}