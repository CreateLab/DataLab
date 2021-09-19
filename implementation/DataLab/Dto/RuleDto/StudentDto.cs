﻿using System;
using DataLab.Dto.Mongo;

namespace DataLab.Dto.RuleDto
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string FIO { get; set; }
        public string Location { get; set; }
        public DateTime Birthdate { get; set; }
        public string Departament { get; set; }
        public string Position { get; set; }
        public bool IsPaid { get; set; }
      
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StudyWay { get; set; }
        public bool Privileges { get; set; }
        public EducationType EducationType { get; set; }
        public string LastAction { get; set; }
        public int Warnings { get; set; }
    }
}