using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using DataLab.Dto.Data;
using DataLab.Dto.MySql;
using DataLab.Dto.Psql;
using StudentDto = DataLab.Dto.MySql.StudentDto;

namespace DataLab
{
    public class MySqlService
    {
        private readonly Random _random = new Random();
        private IEnumerable<StudentDto> _students;
        private IEnumerable<BookInfoDto> _bookInfoDtos;
        private IEnumerable<ConferenceDto> _conference;
        private IEnumerable<ProjectDto> _projectDtos;
        private IEnumerable<PublicationDto> _publicationDtos;
        private List<ProjectStudentsCoAuthorDto> _projectStudentsCoAuthorDtos;
        private List<PublicationCoauthorDto> _publicationCoauthorDtos;

        public IEnumerable<ConferenceParticipationDto> ConferenceParticipationDtos { get; set; }

        public List<PublicationCoauthorDto> PublicationCoauthorDtos => _publicationCoauthorDtos;

        public IEnumerable<StudentDto> Students => _students;

        public IEnumerable<BookInfoDto> BookInfoDtos => _bookInfoDtos;

        public IEnumerable<ConferenceDto> Conference => _conference;

        public IEnumerable<ProjectDto> ProjectDtos => _projectDtos;

        public IEnumerable<PublicationDto> PublicationDtos => _publicationDtos;

        public List<ProjectStudentsCoAuthorDto> ProjectStudentsCoAuthorDtos => _projectStudentsCoAuthorDtos;

        public MySqlService(IDictionary<Guid, string> people, IDictionary<Guid, string> teachers)
        {
            CreateStudents(people,teachers);
            CreateConferentions();
            CreateBooks();
            CreateProjects();
            CreatePublications();
            GenerateProjectCoauthors();
            CreatePublicationCouathors();
            CreateConferenceDto();
        }

        private void CreateConferenceDto()
        {
            ConferenceParticipationDtos = new Faker<ConferenceParticipationDto>()
                .Ignore(x => x.Id)
                .RuleFor(x => x.ConferenceDto, f => f.PickRandom(Conference))
                .RuleFor(x => x.StudentDto, f => f.PickRandom(_students)).Generate(20);
        }

        private void CreatePublicationCouathors()
        {
            _publicationCoauthorDtos = new List<PublicationCoauthorDto>(100);
            _publicationCoauthorDtos.AddRange(new Faker<PublicationCoauthorDto>()
                .Ignore(x => x.Id)
                .RuleFor(x => x.Author, f => f.PickRandom(_students))
                .RuleFor(x => x.Publication, f => f.PickRandom(_publicationDtos))
                .Generate(100)
            );
        }

        private void GenerateProjectCoauthors()
        {
            _projectStudentsCoAuthorDtos = new List<ProjectStudentsCoAuthorDto>(100);
            _projectStudentsCoAuthorDtos.AddRange(new Faker<ProjectStudentsCoAuthorDto>()
                .Ignore(x => x.Id)
                .RuleFor(x => x.Author, f => f.PickRandom(_students))
                .RuleFor(x => x.Project, f => f.PickRandom(_projectDtos))
                .Generate(100)
            );
        }

        private void CreatePublications()
        {
            var addresses = new Faker<Address>("ru").RuleFor(x => x.AddressLine, f => f.Address.FullAddress())
                .Generate(10);
            _publicationDtos = new Faker<PublicationDto>("ru")
                .Ignore(x => x.Id)
                .RuleFor(x => x.Author, f => f.PickRandom(_students))
                .RuleFor(x => x.Lang, _ => "ru")
                .RuleFor(x => x.Name, f => f.Hacker.Abbreviation())
                .RuleFor(x => x.Index, f => f.Random.Int(0, 200))
                .RuleFor(x => x.Name, f => f.Hacker.Abbreviation())
                .RuleFor(x => x.Type, f => f.Hacker.Abbreviation())
                .RuleFor(x => x.PublicationDate, f => f.Date.Between(new DateTime(2013,1,1),new DateTime(2021,12,31)))
                .RuleFor(x => x.PublisherName, f => f.Commerce.Product())
                .RuleFor(x => x.PublisherPlace, f => f.PickRandom(addresses).AddressLine)
                .RuleFor(x => x.PublisherVolume, f => f.Random.Int(0, 200))
                .Generate(100);
        }

        private void CreateProjects()
        {
            _projectDtos = new Faker<ProjectDto>("ru")
                .Ignore(x => x.Id)
                .RuleFor(x => x.Author, f => f.PickRandom(_students))
                .RuleFor(x => x.StartDate, f => f.Date.Past())
                .RuleFor(x => x.EndDate, f => f.Date.Future())
                .RuleFor(x => x.Name, f => f.Hacker.Abbreviation())
                .Generate(100);
        }

        private void CreateBooks()
        {
            _bookInfoDtos = new Faker<BookInfoDto>("ru")
                .RuleFor(x => x.Name, f => f.Database.Engine())
                .RuleFor(x => x.IsTaken, f => f.Random.Bool())
                .RuleFor(x => x.TakeDate, f => f.Date.Between(new DateTime(2016, 1, 1), new DateTime(2021, 12, 31)))
                .RuleFor(x => x.ReturnDate, (f, u) => f.Date.Between(u.TakeDate, new DateTime(2022, 1, 1)))
                .Ignore(x => x.Id)
                .RuleFor(x => x.Person, f => f.PickRandom(_students)).Generate(100);
        }

        private void CreateConferentions()
        {
            var addresses = new Faker<Address>("ru").RuleFor(x => x.AddressLine, f => f.Address.FullAddress())
                .Generate(10);
            _conference = new Faker<ConferenceDto>("ru")
                .RuleFor(x => x.Name, f => f.Commerce.Department())
                .RuleFor(x => x.Place, f => f.PickRandom(addresses).AddressLine)
                .RuleFor(x => x.StartTime, f => f.Date.Between(new DateTime(2016, 1, 1), new DateTime(2021, 1, 1)))
                .Ignore(x => x.Id).Generate(50);
        }

        private void CreateStudents(IDictionary<Guid, string> people, IDictionary<Guid, string> teachers)
        {
            var pos = new[] {"бакалавр", "магистр" };
           var peoples = people.AsEnumerable().Select(x => new StudentDto
            {
                StudentId = x.Key.ToString(),
                FIO = x.Value,
                Position = GetRandomPosition(pos)
            }).ToList();
            peoples.AddRange(teachers.AsEnumerable().Select(x => new StudentDto
            {
                StudentId = x.Key.ToString(),
                FIO = x.Value,
                Position = "доцент"
            }));
            _students = peoples;
        }

        private string GetRandomPosition(IReadOnlyList<string> pos)
        {
            var rand = _random.Next(pos.Count);
            return pos[rand];
        }
    }
}