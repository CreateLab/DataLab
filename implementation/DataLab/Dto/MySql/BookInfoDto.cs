using System;

namespace DataLab.Dto.MySql
{
    public class BookInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public StudentDto Student { get; set; }
        public bool IsTaken { get; set; }
        public DateTime TakeDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}