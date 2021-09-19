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
using StudentDto = DataLab.Dto.Mongo.StudentDto;

namespace DataLab
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var dictionary = CreatePersonDictionary();
            var teachers = CreateTeacherDictionary();
            var pull = Enumerable.Range(0, 100).Select(_ => Guid.NewGuid()).ToArray();
            //FillMySql(dictionary);
            FillPsql(dictionary,teachers,pull);
            //FillMongo(dictionary);
        }

        private static IEnumerable<TeacherDto> CreateTeacherDictionary()
        {
            var json = File.ReadAllText("./Data/teachers.json");
            var dataArray = JsonConvert.DeserializeObject<IEnumerable<TeacherDto>>(json);
            return dataArray;
        }

        private static void FillMySql(Dictionary<Guid, string> dictionary)
        {
            var mySqlService = new MySqlService(dictionary);
            using var db = new MySqlDbContext();
            db.StudentDtos.AddRange(mySqlService.Students);
            db.ProjectDtos.AddRange(mySqlService.ProjectDtos);
            db.ProjectStudentsCoAuthorDtos.AddRange(mySqlService.ProjectStudentsCoAuthorDtos);
            db.BookInfoDtos.AddRange(mySqlService.BookInfoDtos);
            db.ConferenceDtos.AddRange(mySqlService.Conference);
            db.PublicationDtos.AddRange(mySqlService.PublicationDtos);
            db.PublicationCoauthorDtos.AddRange(mySqlService.PublicationCoauthorDtos);
            db.ConferenceParticipationDtos.AddRange(mySqlService.ConferenceParticipationDtos);
            db.SaveChanges();
        }

        public static void FillPsql(Dictionary<Guid, string> dictionary, IEnumerable<TeacherDto> teacher, Guid[] guids)
        {
            var psqlService = new PsqlService(dictionary,teacher, guids);
            using var db = new PsqlAppContext();
            db.Universities.AddRange(psqlService.Universities);
            db.Specialisations.AddRange(psqlService.Specialisations);
            db.Disciplines.AddRange(psqlService.Discipline);
            db.StudentDtos.AddRange(psqlService.StudentDtos);
            db.Results.AddRange(psqlService.Results);
            db.SaveChanges();
        }

        private static void FillMongo(Dictionary<Guid, string> dictionary)
        {
            var mongoService = new MongoService(dictionary);
            const string connectionString = "mongodb://admin:admin@localhost:2222";
            var client = new MongoClient(connectionString);
            var mongoDatabase = client.GetDatabase("test");
            var mongoBuildingCollection = mongoDatabase.GetCollection<Building>("building");
            mongoBuildingCollection.InsertOne(mongoService.Building);

            var mongoRoomCollection = mongoDatabase.GetCollection<RoomDto>("rooms");
            mongoRoomCollection.InsertMany(mongoService.RoomDtos);
            var mongoStudentCollection = mongoDatabase.GetCollection<StudentDto>("students");
            mongoStudentCollection.InsertMany(mongoService.StudentDtos);
            var mongoRentCollection = mongoDatabase.GetCollection<Rent>("rents");
            mongoRentCollection.InsertMany(mongoService.Rents);
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