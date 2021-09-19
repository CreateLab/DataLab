using System.Text.RegularExpressions;

namespace DataLab.Dto.Oracle
{
    public class StudentGroupDto
    {
        public int Id { get; set; }
        public StudyGroup StudyGroup { get; set; }
        public StudentDto StudentDto { get; set; }  
    }
}