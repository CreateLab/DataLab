using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLab.Dto.MySql
{
    [Table("StudentDtos")]
    public class StudentDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string FIO { get; set; }
        public string Position { get; set; }
    }
}