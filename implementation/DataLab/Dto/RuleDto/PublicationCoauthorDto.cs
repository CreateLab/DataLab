namespace DataLab.Dto.RuleDto
{
    public class PublicationCoauthor
    {
        public int Id { get; set; }
        public StudentDto Author { get; set; }
        public PublicationDto Publication { get; set; }
    }
}