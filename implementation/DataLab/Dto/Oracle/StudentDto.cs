using System;

namespace DataLab.Dto.Oracle
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string FIO { get; set; }
        public string Location { get; set; }
        public DateTime Birthdate { get; set; }
        public string Departament { get; set; }
        public string Position { get; set; }
    }
}