using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using DataLab.Dto.Data;
using DataLab.Dto.Mongo;
using DataLab.Dto.MySql;
using DataLab.Dto.Oracle;
using DataLab.Dto.Psql;
using DataLab.Dto.RuleDto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using Building = DataLab.Dto.Mongo.Building;
using Rent = DataLab.Dto.Mongo.Rent;
using StudentDto = DataLab.Dto.Mongo.StudentDto;

namespace DataLab
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var dictionary = CreatePersonDictionary();
            var teachers = CreateTeacherDictionary();
            var pool = Enumerable.Range(0, 100).Select(_ => Guid.NewGuid()).ToArray();
            //FillMySql(dictionary, teachers);
            //FillPsql(dictionary, teachers, pool);
            //FillMongo(dictionary);
            //FillOracle(dictionary, teachers, pool);
            var resultOracleService = CreateResultOracleService();
            FillRuleOracle(resultOracleService);
        }

        private static void FillRuleOracle(ResultOracleService resultOracleService)
        {
            using var db = new RukeAppContext();
            db.Buildings.AddRange(resultOracleService.Buildings);
            db.Classes.AddRange(resultOracleService.Classes);
            db.Disciplines.AddRange(resultOracleService.Disciplines);
            db.PublicationCoauthors.AddRange(resultOracleService.PublicationCoauthors);
            db.StudentDtos.AddRange(resultOracleService.StudentDtos);
            db.Disciplines.AddRange(resultOracleService.Disciplines);
            db.StudyGroups.AddRange(resultOracleService.StudyGroups);
            db.Classes.AddRange(resultOracleService.Classes);
            db.BookInfoDtos.AddRange(resultOracleService.BookInfoDtos);
            db.Buildings.AddRange(resultOracleService.Buildings);
            db.ConferenceDtos.AddRange(resultOracleService.ConferenceDtos);
            db.ConferenceParticipationDtos.AddRange(resultOracleService.ConferenceParticipationDtos);
            db.ProjectDtos.AddRange(resultOracleService.ProjectDtos);
            db.ProjectStudentsCoAuthorDtos.AddRange(resultOracleService.ProjectStudentsCoAuthorDtos);
            db.PublicationDtos.AddRange(resultOracleService.PublicationDtos);
            db.StudentGroupDtos.AddRange(resultOracleService.StudentGroupDtos);
            db.Rooms.AddRange(resultOracleService.Rooms);
            db.Results.AddRange(resultOracleService.Results);
            db.Rents.AddRange(resultOracleService.Rents);
            db.Times.AddRange(resultOracleService.Times.ToList());
            db.BirthdayPlaces.AddRange(resultOracleService.BirthdayPlaces.ToList());
            db.PublicationPlaces.AddRange(resultOracleService.PublicationPlaces.ToList());
            db.Fact1.AddRange(resultOracleService.Fact1.ToList());
            db.Fact2.AddRange(resultOracleService.Fact2.ToList());
            db.Fact3.AddRange(resultOracleService.Fact3.ToList());
            db.Fact4.AddRange(resultOracleService.Fact4.ToList());
            db.SaveChanges();
        }

        private static IEnumerable<TeacherDto> CreateTeacherDictionary()
        {
            var json = File.ReadAllText("./Data/teachers.json");
            var dataArray = JsonConvert.DeserializeObject<IEnumerable<TeacherDto>>(json);
            return dataArray;
        }

        private static void FillMySql(Dictionary<Guid, string> dictionary, IEnumerable<TeacherDto> teacherDtos)
        {
            var mySqlService = new MySqlService(dictionary, teacherDtos.ToDictionary(x => new Guid(x.ID), x => x.FIO));
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

        public static ResultOracleService CreateResultOracleService()
        {
            const string connectionString = "mongodb://admin:admin@localhost:2222";
            var client = new MongoClient(connectionString);
            var mongoDatabase = client.GetDatabase("test");
            using var mySqlDbContext = new MySqlDbContext();
            using var oracleAppContext = new OracleAppContext();
            using var psqlAppContext = new PsqlAppContext();
            var resultOracleService = new ResultOracleService();
            var filter = new BsonDocument();
            resultOracleService.MergeStudents(psqlAppContext.StudentDtos.ToList(),
                mongoDatabase.GetCollection<StudentDto>("students").FindSync<StudentDto>(filter).ToList(),
                mySqlDbContext.StudentDtos.ToList(), oracleAppContext.StudentDtos.ToList());
            resultOracleService.MergeDiscpline(oracleAppContext.Disciplines.ToList(),
                psqlAppContext.Disciplines.ToList());
            resultOracleService.MergeStudyGroup(oracleAppContext.StudyGroups.ToList());
            resultOracleService.MergeClasses(oracleAppContext.Classes.ToList());
            resultOracleService.MergeBookInfoDtos(mySqlDbContext.BookInfoDtos.ToList());
            resultOracleService.MergeBuilding(mongoDatabase.GetCollection<Dto.Mongo.Building>("building")
                .FindSync<Dto.Mongo.Building>(filter).ToList());
            resultOracleService.MergeConference(mySqlDbContext.ConferenceDtos.ToList());
            resultOracleService.MergeConferencePaticipantDto(mySqlDbContext.ConferenceParticipationDtos.ToList());
            resultOracleService.MergeProjectDto(mySqlDbContext.ProjectDtos.ToList());
            resultOracleService.MergeProjAndCoAuthor(mySqlDbContext.ProjectStudentsCoAuthorDtos.ToList());
            resultOracleService.MergePublication(mySqlDbContext.PublicationDtos.ToList());
            resultOracleService.MergePublicationCoAuthor(mySqlDbContext.PublicationCoauthorDtos.ToList());
            resultOracleService.MergeRoom(mongoDatabase.GetCollection<RoomDto>("rooms")
                .FindSync<Dto.Mongo.RoomDto>(filter).ToList());
            resultOracleService.MergeRents(mongoDatabase.GetCollection<Rent>("rents").FindSync<Dto.Mongo.Rent>(filter)
                .ToList());
            resultOracleService.MergeResults(oracleAppContext.Results.ToList(), psqlAppContext.Results.ToList());
            resultOracleService.MergeStudentGroupDto(oracleAppContext.StudentGroupDtos.ToList());
            resultOracleService.GenerateTimes();
            resultOracleService.GenerateBirthdayPlace();
            resultOracleService.GeneratePubPlaces();
            resultOracleService.GenerateFact1();
            resultOracleService.GenerateFact2();
            resultOracleService.GenerateFact3();
            resultOracleService.GenerateFact4();
            return resultOracleService;
        }

        public static void FillOracle(Dictionary<Guid, string> dictionary, IEnumerable<TeacherDto> teacher,
            Guid[] guids)
        {
            var oracleService = new OracleService(dictionary, teacher, guids);
            using var db = new OracleAppContext();
            db.Classes.AddRange(oracleService.Classes);
            db.Disciplines.AddRange(oracleService.Disciplines);
            db.Results.AddRange(oracleService.Results);
            db.StudentDtos.AddRange(oracleService.StudentDtos);
            db.StudyGroups.AddRange(oracleService.StudyGroups);
            db.StudentGroupDtos.AddRange(oracleService.StudentGroupDtos);
            db.SaveChanges();
        }

        public static void FillPsql(Dictionary<Guid, string> dictionary, IEnumerable<TeacherDto> teacher, Guid[] guids)
        {
            var psqlService = new PsqlService(dictionary, teacher, guids);
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
            mongoBuildingCollection.InsertMany(mongoService.Buildings);

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