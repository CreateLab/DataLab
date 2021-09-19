using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataLab.Dto.Mongo
{
    public class Rent
    {
        public ObjectId Id { get; set; }
        public MongoDBRef  Student{ get; set; }
        public MongoDBRef  Room{ get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}