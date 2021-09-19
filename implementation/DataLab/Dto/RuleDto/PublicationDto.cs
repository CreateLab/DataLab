using System;
using System.Collections.Generic;

namespace DataLab.Dto.RuleDto
{
    public class PublicationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Lang { get; set; }
        public string PublisherName { get; set; }
        public int  PublisherVolume { get; set; }
        public string PublisherPlace { get; set; }
        public string Type { get; set; }
        public int Index { get; set; }
        public DateTime PublicationDate { get; set; }
        public StudentDto Author { get; set; }

    }
}