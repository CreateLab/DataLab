using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using DataLab.Dto.Psql;

namespace DataLab
{
    public class PsqlService
    {
        private IEnumerable<University> _universities;
        private IEnumerable<SpecialisationDto> _specialisations;
        private IEnumerable<DisciplineDto> _discipline;
        private IEnumerable<StudentDto> _studentDtos;
        private IEnumerable<Result> _results;

        public IEnumerable<University> Universities => _universities;

        public IEnumerable<SpecialisationDto> Specialisations => _specialisations;

        public IEnumerable<DisciplineDto> Discipline => _discipline;

        public IEnumerable<StudentDto> StudentDtos => _studentDtos;

        public IEnumerable<Result> Results => _results;

        public PsqlService(IDictionary<Guid, string> people)
        {
            CreateUniversities();
            CreateSpecialisations();
            GenerateDisciplines();
            CreateStudents(people); 
            CreateStudentResults();
        }

        private void CreateStudents(IDictionary<Guid, string> people)
        {
            _studentDtos = people.Select(x => new StudentDto
            {
                StudentId = x.Key.ToString(),
                FIO = x.Value
            });
        }

        private void GenerateDisciplines()
        {
            _discipline = new Faker<DisciplineDto>("ru")
                .Ignore(x=>x.Id)
                .RuleFor(x => x.Name, x => x.Commerce.ProductName())
                .RuleFor(x => x.Semester, f => f.Random.Int(1, 9))
                .RuleFor(x => x.IsExam, f => f.Random.Bool())

                .RuleFor(x => x.LabTime, f => f.Random.Int(0, 100))
                .RuleFor(x => x.LectureTime, f => f.Random.Int(0, 100))
                .RuleFor(x => x.PracticTime, f => f.Random.Int(0, 100))
                .RuleFor(x => x.SpecialisationDto, f => f.PickRandom(_specialisations))
                .RuleFor(x => x.TeacherFio, f => f.Name.FullName())
                .RuleFor(x => x.TeacherId, f => Guid.NewGuid().ToString())
                .RuleFor(x => x.IsFullTime, f => f.Random.Bool())
                .RuleFor(x => x.IsNewStandard, f => f.Random.Bool()).Generate(100);
        }

        private void CreateSpecialisations()
        {
            _specialisations = new Faker<SpecialisationDto>("ru")
                .Ignore(x=>x.Id)
                .RuleFor(x => x.Name, f => $@"{f.Random.Int(1, 10)}.{f.Random.Int(1, 10)}.{f.Random.Int(1, 10)}")
                .RuleFor(x => x.University, f => f.PickRandom(_universities)).Generate(10);
        }

        private void CreateUniversities()
        {
            _universities = new[]
            {
                new University
                {
                    Name = "ITMO"
                },
                new University
                {
                    Name = "VSU"
                }
            };
        }

        private void CreateStudentResults()
        {
            var results = new List<Result>(400);
            foreach (var studentDto in _studentDtos)
            {
                results.AddRange( new Faker<Result>()
                    .Ignore(x=>x.Id)
                    .RuleFor(x => x.Student, _ => studentDto)
                    .RuleFor(x => x.Mark, f => f.Random.Int(0, 101))
                    .RuleFor(x => x.Discipline, f => f.PickRandom(_discipline)).Generate(2));
            }

            _results = results;
        }
    }
}