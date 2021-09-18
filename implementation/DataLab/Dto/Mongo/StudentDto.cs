using System;

namespace DataLab.Dto.Mongo
{
    public class StudentDto
    {
        public string StudentId { get; set; }
        public string FIO { get; set; }
        public bool Privileges { get; set; }
        public EducationType EducationType { get; set; }
        public string LastAction { get; set; }
        public int Warnings { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}