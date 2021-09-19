using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using DataLab.Dto.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using SharpCompress.Compressors.Xz;

namespace DataLab
{
    public class MongoService
    {
        private readonly Random _random = new Random();

        public Building Building { get; set; }
        public List<RoomDto> RoomDtos { get; set; }
        public IEnumerable<StudentDto> StudentDtos { get; set; }
        public IEnumerable<Rent> Rents { get; set; }

        public MongoService(Dictionary<Guid, string> peoples)
        {
            CreateBuilding();
            GenerateRooms();
            GenerateStudents(peoples);
            GenerateRoomStudent();
        }

        private void CreateBuilding()
        {
            var building = new Faker<Building>("ru")
                .RuleFor(x => x.Id, _ => ObjectId.GenerateNewId())
                .RuleFor(x => x.Location, f => f.Address.FullAddress())
                .RuleFor(x => x.RoomCount, f => f.Random.Int(200, 500)).Generate(1).First();
            Building = building;
        }

        private void GenerateRooms()
        {
            RoomDtos = new Faker<RoomDto>()
                .RuleFor(x => x.Id, _=> ObjectId.GenerateNewId())
                .RuleFor(x=>x.Number,f=>f.Random.Int())
                .RuleFor(x => x.MaxCapacity, f => f.Random.Int(1, 6))
                .RuleFor(x => x.DisinfectionDate, f => f.Date.Recent())
                .RuleFor(x => x.IsInsects, f => f.Random.Bool())
                .RuleFor(x => x.BuildId, _ => new MongoDBRef("building", Building.Id))
                .Generate(Building.RoomCount);
            //.RuleFor(x => x.Students, (f, r) => GenerateStudents(people, r.MaxCapacity))
            // .RuleFor(x => x.Capacity, (f, u) => u.Students.ToList().Count).Generate(buildingRoomCount);
        }

        private void GenerateStudents(IDictionary<Guid, string> people)
        {
            StudentDtos = people.Select(GenerateStudent);
        }


        private StudentDto GenerateStudent(KeyValuePair<Guid, string> keyValuePair)
        {
            var actions = new[] { "ушел", "пришел" };
            return new Faker<StudentDto>("ru")
                .RuleFor(x => x.Id, _ => ObjectId.GenerateNewId())
                .RuleFor(x => x.Privileges, f => f.Random.Bool())
                .RuleFor(x => x.Warnings, f => f.Random.Int(1, 100))
                .RuleFor(x => x.LastAction, f => f.PickRandom(actions))
                .RuleFor(x => x.EducationType, f => f.PickRandom<EducationType>())
                .RuleFor(x => x.StudentId, _ => keyValuePair.Key.ToString())
                .RuleFor(x => x.FIO, _ => keyValuePair.Value).Generate(1).First();
        }

        private void GenerateRoomStudent()
        {
            var rents = new List<Rent>();
            foreach (var student in StudentDtos)
            {
                var randomRoom = GetRandomRoom();
                randomRoom.Capacity++;
                rents.Add(new Faker<Rent>().RuleFor(x => x.Id, _ => ObjectId.GenerateNewId())
                    .RuleFor(x => x.Room, _ => new MongoDBRef("rooms", randomRoom.Id))
                    .RuleFor(x => x.Student, _ => new MongoDBRef("students", student.Id))
                    .RuleFor(x=>x.StartDate,f=>f.Date.Recent())
                    .RuleFor(f=>f.EndDate,f=>f.Date.Soon()));
            }

            Rents = rents;
        }

        private RoomDto GetRandomRoom()
        {
            while (true)
            {
                var next = _random.Next(0, RoomDtos.Count);
                var room = RoomDtos[next];
                if (room.Capacity < room.MaxCapacity) return room;
            }
        }
    }
}