using System;
using System.Collections.Generic;
using Bogus.DataSets;

namespace DataLab.Dto.RuleDto
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public StudentDto Author { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}