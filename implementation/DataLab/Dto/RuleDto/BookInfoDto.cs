using System;

namespace DataLab.Dto.RuleDto
{
    public class BookInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string PersonId { get; set; }
        public bool IsTaken { get; set; }
        public DateTime TakeDate { get; set; }
        public DateTime ReturnDate { get; set; }
       
    }
}