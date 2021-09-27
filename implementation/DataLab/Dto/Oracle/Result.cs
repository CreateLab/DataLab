using System;
using System.Globalization;

namespace DataLab.Dto.Oracle
{
    public class Result
    {
        public int Id { get; set; }
        public int Mark { get; set; }

        public string LetterMark
        {
            get
            {
                return Mark switch
                {

                    2 => "FX",
                    3 => "FE",
                    4 => "B",
                    5 => "A",
                    _ => throw new ArgumentException()
                };
            }
        }

        public DateTime Date { get; set; }
        public StudentDto Student { get; set; }
        public Discipline Discipline { get; set; }
    }
}