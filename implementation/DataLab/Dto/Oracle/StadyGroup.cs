using System;

namespace DataLab.Dto.Oracle
{
    public class StadyGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Course { get; set; }
    }
}