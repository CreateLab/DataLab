using System.Runtime.Serialization;
using MongoDB.Bson;

namespace DataLab.Dto.RuleDto
{
    public class Building
    {
        public int Id { get; set; }
        public int RoomCount { get; set; }
        public string Location { get; set; }

        [IgnoreDataMember]
        public BsonValue MongoId { get; set; }
    }
}