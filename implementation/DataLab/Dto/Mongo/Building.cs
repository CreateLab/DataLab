using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;

namespace DataLab.Dto.Mongo
{
    public class Building
    {
        public ObjectId Id { get; set; }
        public int RoomCount { get; set; }
        public IEnumerable<RoomDto> Rooms { get; set; }
        public string Location { get; set; }
    }
}