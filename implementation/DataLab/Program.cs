using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using DataLab.Dto.Data;
using DataLab.Dto.Mongo;
using DataLab.Dto.MySql;
using DataLab.Dto.Psql;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace DataLab
{
    class Program
    {
        static void Main(string[] args)
        {
            var dictionary = CreatePersonDictionary();
            //FillMySql(dictionary);
            //FillPsql(dictionary);
            FillMongo(dictionary);
        }

        private static void FillMySql(Dictionary<Guid, string> dictionary)
        {
            var mySqlService = new MySqlService(dictionary);
            using (var db = new MySqlDbContext())
            {
                db.StudentDtos.AddRange(mySqlService.Students);
                db.ProjectDtos.AddRange(mySqlService.ProjectDtos);
                db.ProjectStudentsCoAuthorDtos.AddRange(mySqlService.ProjectStudentsCoAuthorDtos);
                db.BookInfoDtos.AddRange(mySqlService.BookInfoDtos);
                db.ConferenceDtos.AddRange(mySqlService.Conference);
                db.PublicationDtos.AddRange(mySqlService.PublicationDtos);
                db.PublicationCoauthorDtos.AddRange(mySqlService.PublicationCoauthorDtos);
                db.SaveChanges();
            }
        }

        public static void FillPsql(Dictionary<Guid, string> dictionary)
        {
            var psqlService = new PsqlService(dictionary);
            using (var db = new PsqlAppContext())
            {
                db.Universities.AddRange(psqlService.Universities);
                db.Specialisations.AddRange(psqlService.Specialisations);
                db.Disciplines.AddRange(psqlService.Discipline);
                db.StudentDtos.AddRange(psqlService.StudentDtos);
                db.Results.AddRange(psqlService.Results);
                db.SaveChanges();
            }
        }

        private static void FillMongo(Dictionary<Guid, string> dictionary)
        {
            var building = new MongoService().CreateBuilding(dictionary);
            string connectionString = "mongodb://admin:admin@localhost:2222";
            MongoClient client = new MongoClient(connectionString);
            var mongoDatabase = client.GetDatabase("test");
            var mongoCollection = mongoDatabase.GetCollection<Building>("building");
            mongoCollection.InsertOne(building);
            var mongoDbRef = new MongoDBRef("building", building.Id);
            foreach (var room in building.Rooms)
            {
                room.BuildId = mongoDbRef;
            }


            var roomsCollection = mongoDatabase.GetCollection<RoomDto>("rooms");
            roomsCollection.InsertMany(building.Rooms);
            var filter = new BsonDocument();
        }

        private static Dictionary<Guid, string> CreatePersonDictionary()
        {
            var json = File.ReadAllText("./Data/names.json");
            var dataArray = JsonConvert.DeserializeObject<IEnumerable<Data>>(json);
            var dictionary = dataArray.ToDictionary(x => x.Id, x => x.Name);
            return dictionary;
        }
    }
}