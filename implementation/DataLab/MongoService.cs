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

        public List<Building> Buildings { get; set; }
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
            var buildings = new Faker<Building>("ru")
                .RuleFor(x => x.Id, _ => ObjectId.GenerateNewId())
                .RuleFor(x => x.Location, f => f.Address.FullAddress())
                .RuleFor(x => x.RoomCount, f => f.Random.Int(50, 70)).Generate(3);
            Buildings = buildings;
        }

        private void GenerateRooms()
        {
            RoomDtos = Buildings.SelectMany(GenerateBuildRooms).ToList();
            //.RuleFor(x => x.Students, (f, r) => GenerateStudents(people, r.MaxCapacity))
            // .RuleFor(x => x.Capacity, (f, u) => u.Students.ToList().Count).Generate(buildingRoomCount);
        }

        private IEnumerable<RoomDto> GenerateBuildRooms(Building building)
        {
            return new Faker<RoomDto>()
                .RuleFor(x => x.Id, _ => ObjectId.GenerateNewId())
                .RuleFor(x => x.Number, f => f.Random.Int())
                .RuleFor(x => x.MaxCapacity, f => f.Random.Int(1, 6))
                .RuleFor(x => x.DisinfectionDate, f => f.Date.Recent())
                .RuleFor(x => x.IsInsects, f => f.Random.Bool())
                .RuleFor(x => x.BuildId, _ => new MongoDBRef("building", building.Id))
                .Generate(building.RoomCount);
        }

        private void GenerateStudents(IDictionary<Guid, string> people)
        {
            StudentDtos = people.Select(GenerateStudent).ToList();
        }


        private StudentDto GenerateStudent(KeyValuePair<Guid, string> keyValuePair)
        {
            var actions = new[] { "ушел", "пришел" };
            return new Faker<StudentDto>("ru")
                .RuleFor(x => x.Id, _ =>Guid.NewGuid().ToString())
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
            var years = new[] { 2016, 2017, 2018, 2019, 2020, 2021 };
            foreach (var student in StudentDtos.Take(500).ToList())
            {
                var randomRoom = GetRandomRoom();
                randomRoom.Capacity++;
                var rent = new Faker<Rent>().RuleFor(x => x.Id, _ => ObjectId.GenerateNewId())
                    .RuleFor(x => x.Room, _ => new MongoDBRef("rooms", randomRoom.Id))
                    .RuleFor(x => x.Student, _ => new MongoDBRef("students", student.Id))
                    .RuleFor(x => x.StartDate,
                        f => new DateTime(f.PickRandom(years), 9, 1))
                    .RuleFor(x => x.EndDate, (_, u) => new DateTime(u.StartDate.Year + 1, 8, 31)).Generate(1).First();
                rents.Add(rent);
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