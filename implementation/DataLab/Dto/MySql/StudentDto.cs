using System.Collections.Generic;

namespace DataLab.Dto.MySql
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string FIO { get; set; }
        public string Position { get; set; }
    }
}