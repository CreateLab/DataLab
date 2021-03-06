using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using MongoDB.Bson;

namespace DataLab.Dto.RuleDto
{
    public class Room
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public Building Build { get; set; }
        public int MaxCapacity { get; set; }
        public int Capacity { get; set; }
        public bool IsInsects { get; set; }
        public DateTime DisinfectionDate { get; set; }
        [IgnoreDataMember]
        [NotMapped]
        public string MongoId { get; set; }
    }
}