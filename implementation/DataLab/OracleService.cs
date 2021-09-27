using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using DataLab.Dto.Data;
using DataLab.Dto.Oracle;

namespace DataLab
{
    public class OracleService
    {
        private IEnumerable<StudentDto> _studentDtos;
        private IEnumerable<StudyGroup> _studyGroups;
        private IEnumerable<Discipline> _disciplines;
        private IEnumerable<Class> _classes;
        private IEnumerable<StudentGroupDto> _studentGroupDtos;
        private IEnumerable<Result> _results;

        public IEnumerable<StudentDto> StudentDtos => _studentDtos;

        public IEnumerable<StudyGroup> StudyGroups => _studyGroups;

        public IEnumerable<Discipline> Disciplines => _disciplines;

        public IEnumerable<Class> Classes => _classes;

        public IEnumerable<StudentGroupDto> StudentGroupDtos => _studentGroupDtos;

        public IEnumerable<Result> Results => _results;

        private List<Guid> _guids;

        public OracleService(IDictionary<Guid, string> people, IEnumerable<TeacherDto> dictionary, Guid[] guids)
        {
            _guids = new List<Guid>(guids);
            CreateStudents(people);
            GenerateStudyGroup();
            GenerateDiscipline(dictionary);
            GenerateClasses(dictionary);
            GenerateSDDtos();
            GenerateResults();
        }

        private void GenerateResults()
        {
            _results = new Faker<Result>("ru")
                .Ignore(x => x.Id)
                .RuleFor(x => x.Discipline, f => f.PickRandom(_disciplines))
                .RuleFor(x => x.Mark, f => f.Random.Int(2, 5))
                .RuleFor(x => x.Date, f => f.Date.Recent())
                .RuleFor(x => x.Student, f => f.PickRandom(_studentDtos)).Generate(5000);
        }

        private void GenerateSDDtos()
        {
            _studentGroupDtos = _studentDtos.Select(studentDto => new Faker<StudentGroupDto>()
                .Ignore(x => x.Id)
                .RuleFor(x => x.StudentDto, _ => studentDto)
                .RuleFor(x => x.StudyGroup, f => f.PickRandom(_studyGroups)).Generate(1).First()).ToArray();
        }


        private void GenerateClasses(IEnumerable<TeacherDto> dictionary)
        {
            _classes = new Faker<Class>("ru").Ignore(x => x.Id)
                .RuleFor(x => x.Discipline, f => f.PickRandom(_disciplines))
                .RuleFor(x => x.Room, f => f.Random.Int())
                .RuleFor(x => x.Time, f => f.Date.Soon())
                .RuleFor(x => x.TeacherId, f => f.PickRandom(dictionary).ID)
                .RuleFor(x => x.TeacherFio, (_, u) => dictionary.First(x => x.ID == u.TeacherId).FIO)
                .RuleFor(x => x.StudyGroup, f => f.PickRandom(_studyGroups))
                .Generate(50);
        }

        private void GenerateDiscipline(IEnumerable<TeacherDto> dictionary)
        {
            _disciplines = new Faker<Discipline>("ru")
                .Ignore(x => x.Id)
                .RuleFor(x => x.Specialisation, f => f.Company.CompanyName())
                .RuleFor(x => x.DiscpId, _ => GetDGuid())
                .RuleFor(x => x.IsFullTime, f => f.Random.Bool())
                .RuleFor(x => x.TeacherId, f => f.PickRandom(dictionary).ID)
                .RuleFor(x => x.TeacherFio, (_, u) => dictionary.First(x => x.ID == u.TeacherId).FIO).Generate(50);
        }

        private string GetDGuid()
        {
            var first = _guids.First();
            _guids.Remove(first);
            return first.ToString();
        }

        private void GenerateStudyGroup()
        {
            var q = new[] { "маг", "бак" };

            _studyGroups = new Faker<StudyGroup>("ru")
                .Ignore(x => x.Id)
                .RuleFor(x => x.Name, f => f.Company.CompanyName())
                .RuleFor(x => x.Qualification, f => f.PickRandom(q))
                .RuleFor(x => x.Course, (f, u) => u.Qualification == "маг" ? f.Random.Int(5, 6) : f.Random.Int(1, 4))
                .RuleFor(x => x.Year, f => f.Random.Int(2013, 2021).ToString())
                .RuleFor(x => x.StartDate, (_, u) => new DateTime(int.Parse(u.Year), 9, 1))
                .RuleFor(x => x.EndDate, (_, u) => new DateTime(int.Parse(u.Year) + 1, 8, 31)).Generate(50);
        }

        private void CreateStudents(IDictionary<Guid, string> people)
        {
            var addresses = new Faker<Address>("ru").RuleFor(x => x.AddressLine, f => f.Address.FullAddress())
                .Generate(10);
            _studentDtos = people.Select(x => GenerateStudent(x, addresses)).ToList();
        }

        private StudentDto GenerateStudent(KeyValuePair<Guid, string> pair, List<Address> addresses)
        {
            var dep = new[] { "пи", "суир", "мде" };
            var pos = new[] { "доцент", "не доцент" };
            var way = new[] { "разработка систем", "система разработки" };

            return new Faker<StudentDto>("ru")
                .Ignore(x => x.Id)
                .RuleFor(x => x.Birthdate, f => f.Date.Between(new DateTime(1995, 1, 1), new DateTime(2002, 1, 1)))
                .RuleFor(x => x.Location, f => f.PickRandom(addresses).AddressLine)
                .RuleFor(x => x.Departament, f => f.PickRandom(dep))
                .RuleFor(x => x.Position, f => f.PickRandom(pos))
                .RuleFor(x => x.IsPaid, f => f.Random.Bool())
                .RuleFor(x => x.StudyWay, f => f.PickRandom(way))
                .RuleFor(x => x.StudentId, _ => pair.Key.ToString())
                .RuleFor(x => x.FIO, _ => pair.Value)
                .RuleFor(x => x.StartDate, f => f.Date.Recent())
                .RuleFor(x => x.EndDate, f => f.Date.Future()).Generate(1).First();
        }
    }
}