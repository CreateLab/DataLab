using System;

namespace DataLab.Dto.Oracle
{
    public class StudyGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Course { get; set; }
        public string Year { get; set; }
        public string Qualification { get; set; }
    }
}