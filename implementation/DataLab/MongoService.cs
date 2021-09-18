using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using DataLab.Dto.Mongo;

namespace DataLab
{
    public class MongoService
    {
        private readonly Random _random = new Random();

        public Building CreateBuilding(Dictionary<Guid, string> peoples)
        {
            var building = new Faker<Building>("ru")
                .RuleFor(x => x.Location, f => f.Address.FullAddress())
                .Ignore(x => x.Rooms)
                .RuleFor(x => x.RoomCount, f => f.Random.Int(10, 200)).Generate(1).First();
            building.Rooms = GenerateRooms(peoples, building.RoomCount);
            return building;
        }

        private IEnumerable<RoomDto> GenerateRooms(Dictionary<Guid, string> people, int buildingRoomCount)
        {
            return new Faker<RoomDto>()
                .RuleFor(x => x.MaxCapacity, f => f.Random.Int(1, 6))
                .RuleFor(x => x.DesenfectionDate, f => f.Date.Recent())
                .RuleFor(x => x.IsInsects, f => f.Random.Bool())
                .RuleFor(x => x.Students, (f, r) => GenerateStudents(people, r.MaxCapacity))
                .RuleFor(x => x.Capacity, (f, u) => u.Students.ToList().Count).Generate(buildingRoomCount);
        }

        private IEnumerable<StudentDto> GenerateStudents(IDictionary<Guid, string> people, int arg2MaxCapacity)
        {
            var maxCapacity = people.Count > arg2MaxCapacity ? arg2MaxCapacity : people.Count;
            if (maxCapacity <= 2) return Array.Empty<StudentDto>();
            var actions = new[] { "ушел", "пришел" };
            var count = _random.Next(1, maxCapacity);

            var students = new Faker<StudentDto>()
                .RuleFor(x => x.Privileges, f => f.Random.Bool())
                .RuleFor(x => x.Warnings, f => f.Random.Int(1, 4))
                .RuleFor(x => x.EducationType, f => f.PickRandom<EducationType>())
                .RuleFor(x => x.LastAction, f => f.PickRandom(actions))
                .RuleFor(x=>x.StartDate,f=>f.Date.Past())
                .RuleFor(x=>x.EndDate,f=>f.Date.Future())
                .Generate(count);
            foreach (var student in students)
            {
                int index = _random.Next(people.Count);
                var s = people.ToList()[index];
                people.Remove(s.Key);
                student.StudentId = s.Key.ToString();
                student.FIO = s.Value;
            }

            return students;
        }
    }
}