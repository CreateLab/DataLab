using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace DataLab.Dto.Mongo
{
    public class RoomDto
    {
       
        public ObjectId Id { get; set; }
        public MongoDBRef BuildId { get; set; }
        public int MaxCapacity { get; set; }
        public int Capacity { get; set; }
        public bool IsInsects { get; set; }
        public DateTime DisinfectionDate { get; set; }
        public IEnumerable<StudentDto> Students { get; set; }
    }
}