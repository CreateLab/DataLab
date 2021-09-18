using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataLab.Dto.MySql
{
    public class ProjectStudentsCoAuthorDto
    {
        public int Id { get; set; }
        public StudentDto Author { get; set; }
        public ProjectDto Project { get; set; }
    }
}