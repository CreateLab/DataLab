using System;

namespace DataLab.Dto.RuleDto
{
    public class Result
    {
        public int Id { get; set; }
        public int? Mark { get; set; }
        public DateTime? Date { get; set; }
        public StudentDto Student { get; set; }
        
        public Discipline Discipline { get; set; }
    }
}