using System;

namespace DataLab.Dto.MySql
{
    public class ConferenceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StatTime { get; set; }
        public string Place { get; set; }
        
    }
}