using System;
using System.Linq;

namespace DataLab.Dto.RuleDto
{
    public class Time
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Sem { get; set; }

        public Time()
        {
            
        }
        public Time(DateTime time)
        {
            Year = time.Year;
            Sem = time.Month < 6 ? 1 : 2;
        }
    }
}