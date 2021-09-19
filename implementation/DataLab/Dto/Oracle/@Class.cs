using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataLab.Dto.Oracle
{
    public class Class
    {
        public int Id { get; set; }
        public Discipline Discipline { get; set; }
        public string TeacherId { get; set; }
        public string TeacherFio { get; set; }
        public DateTime Time { get; set; }
        public int Room { get; set; }
        public StudyGroup StudyGroup { get; set; }
    }
}