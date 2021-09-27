using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataLab.Dto.Mongo
{
    public class StudentDto
    {
        [BsonId]
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string FIO { get; set; }
        public bool Privileges { get; set; }
        public EducationType EducationType { get; set; }
        public string LastAction { get; set; }
        public int Warnings { get; set; }
      
    }
}