using System;

namespace DataLab.Dto.RuleDto
{
    public class Rent
    {
        public int Id { get; set; }
        public StudentDto Student { get; set; }
        public Room Room { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}