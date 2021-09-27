using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataLab.Dto.Mongo;
using DataLab.Dto.MySql;
using DataLab.Dto.Oracle;
using DataLab.Dto.Psql;
using DataLab.Dto.RuleDto;
using BookInfoDto = DataLab.Dto.RuleDto.BookInfoDto;
using Building = DataLab.Dto.RuleDto.Building;
using Class = DataLab.Dto.RuleDto.Class;
using ConferenceDto = DataLab.Dto.RuleDto.ConferenceDto;
using ConferenceParticipationDto = DataLab.Dto.RuleDto.ConferenceParticipationDto;
using Discipline = DataLab.Dto.RuleDto.Discipline;
using ProjectDto = DataLab.Dto.RuleDto.ProjectDto;
using ProjectStudentsCoAuthorDto = DataLab.Dto.RuleDto.ProjectStudentsCoAuthorDto;
using PublicationDto = DataLab.Dto.RuleDto.PublicationDto;
using Rent = DataLab.Dto.Mongo.Rent;
using Result = DataLab.Dto.RuleDto.Result;
using StudentDto = DataLab.Dto.RuleDto.StudentDto;
using StudentGroupDto = DataLab.Dto.RuleDto.StudentGroupDto;
using StudyGroup = DataLab.Dto.RuleDto.StudyGroup;

namespace DataLab
{
    public class ResultOracleService
    {
        private IEnumerable<StudentDto> _studentDtos;

        private IEnumerable<Discipline> _disciplines;

        private IEnumerable<StudyGroup> _studyGroups;

        private IEnumerable<Class> _classes;

        private IEnumerable<BookInfoDto> _bookInfoDtos;

        private IEnumerable<Building> _buildings;

        private IEnumerable<ConferenceDto> _conferenceDtos;

        private IEnumerable<ConferenceParticipationDto> _conferenceParticipationDtos;

        private IEnumerable<ProjectDto> _projectDtos;

        private IEnumerable<ProjectStudentsCoAuthorDto> _projectStudentsCoAuthorDtos;

        private IEnumerable<PublicationDto> _publicationDtos;

        private IEnumerable<PublicationCoauthor> _publicationCoauthors;

        private IEnumerable<Room> _rooms;

        private IEnumerable<Result> _results;

        private IEnumerable<Dto.RuleDto.Rent> _rents;

        private IEnumerable<StudentGroupDto> _studentGroupDtos;

        private IEnumerable<BirthdayPlace> _birthdayPlaces;

        private IEnumerable<PublicationPlace> _publicationPlaces;

        private IEnumerable<Time> _times;

        private IEnumerable<Fact1> _fact1;

        private IEnumerable<Fact2> _fact2;

        private IEnumerable<Fact3> _fact3;

        private IEnumerable<Fact4> _fact4;

        public IEnumerable<Building> Buildings => _buildings;

        public IEnumerable<StudentGroupDto> StudentGroupDtos => _studentGroupDtos;

        public IEnumerable<BirthdayPlace> BirthdayPlaces => _birthdayPlaces;

        public IEnumerable<PublicationPlace> PublicationPlaces => _publicationPlaces;

        public IEnumerable<Time> Times => _times;

        public IEnumerable<Fact1> Fact1 => _fact1;

        public IEnumerable<Fact2> Fact2 => _fact2;

        public IEnumerable<Fact3> Fact3 => _fact3;

        public IEnumerable<Fact4> Fact4 => _fact4;

        public IEnumerable<StudentDto> StudentDtos => _studentDtos;

        public IEnumerable<Discipline> Disciplines => _disciplines;

        public IEnumerable<StudyGroup> StudyGroups => _studyGroups;

        public IEnumerable<Class> Classes => _classes;

        public IEnumerable<BookInfoDto> BookInfoDtos => _bookInfoDtos;

        public IEnumerable<ConferenceDto> ConferenceDtos => _conferenceDtos;

        public IEnumerable<ConferenceParticipationDto> ConferenceParticipationDtos => _conferenceParticipationDtos;

        public IEnumerable<ProjectDto> ProjectDtos => _projectDtos;

        public IEnumerable<ProjectStudentsCoAuthorDto> ProjectStudentsCoAuthorDtos => _projectStudentsCoAuthorDtos;

        public IEnumerable<PublicationDto> PublicationDtos => _publicationDtos;

        public IEnumerable<PublicationCoauthor> PublicationCoauthors => _publicationCoauthors;

        public IEnumerable<Room> Rooms => _rooms;

        public IEnumerable<Result> Results => _results;

        public IEnumerable<Dto.RuleDto.Rent> Rents => _rents;

        /// <summary>
        /// 1
        /// </summary>
        /// <param name="psqlStudents"></param>
        /// <param name="mongoStudents"></param>
        /// <param name="mySqlStudents"></param>
        /// <param name="oracleSudents"></param>
        public void MergeStudents(IEnumerable<Dto.Psql.StudentDto> psqlStudents,
            IEnumerable<Dto.Mongo.StudentDto> mongoStudents, IEnumerable<Dto.MySql.StudentDto> mySqlStudents,
            IEnumerable<Dto.Oracle.StudentDto> oracleSudents)
        {
            var groupBy = oracleSudents.GroupBy(x => x.StudentId);
            var studentsIds = new HashSet<string>(psqlStudents.Select(x => x.StudentId));
            _studentDtos = studentsIds.Select(id =>
            {
                var psqlStudent = psqlStudents.First(x => x.StudentId == id);
                var mongoStudent = mongoStudents.First(x => x.StudentId == id);
                var mySqlStudent = mySqlStudents.First(x => x.StudentId == id);
                var oracleStudent = oracleSudents.First(x => x.StudentId == id);

                return new StudentDto
                {
                    Birthdate = oracleStudent.Birthdate,
                    Departament = oracleStudent.Departament,
                    Location = oracleStudent.Location,
                    Position = mySqlStudent.Position,
                    Warnings = mongoStudent.Warnings,
                    Privileges = mongoStudent.Privileges,
                    EducationType = mongoStudent.EducationType,
                    IsPaid = oracleStudent.IsPaid,
                    EndDate = oracleStudent.EndDate,
                    LastAction = mongoStudent.LastAction,
                    StartDate = oracleStudent.StartDate,
                    StudentId = id,
                    StudyWay = oracleStudent.StudyWay,
                    FIO = psqlStudent.FIO,
                    MongoId = mongoStudent.Id.ToString()
                };
            }).ToList();
        }

        /// <summary>
        /// 2
        /// </summary>
        /// <param name="oracleDiscipline"></param>
        /// <param name="psqlDisciplines"></param>
        public void MergeDiscpline(IEnumerable<Dto.Oracle.Discipline> oracleDiscipline,
            IEnumerable<DisciplineDto> psqlDisciplines)
        {
            var numbers = oracleDiscipline.Select(x => x.DiscpId).Union(psqlDisciplines.Select(x => x.DiscpId));
            _disciplines = numbers.Select(id =>
            {
                var psql = psqlDisciplines.FirstOrDefault(x => x.DiscpId == id);
                var oracle = oracleDiscipline.FirstOrDefault(x => x.DiscpId == id);

                return new Discipline
                {
                    Faculty = psql?.Faculty,
                    Name = psql?.Name,
                    Semester = psql?.Semester,
                    Specialisation = psql?.SpecialisationDto?.Name ?? oracle?.Specialisation,
                    DiscpId = id,
                    IsExam = psql?.IsExam,
                    LabTime = psql?.LabTime,
                    LectureTime = psql?.LectureTime,
                    PracticeTime = psql?.PracticeTime,
                    TeacherFio = psql?.TeacherFio ?? oracle?.TeacherFio,
                    TeacherId = psql?.TeacherId ?? psql?.TeacherId,
                    UniversityName = psql?.SpecialisationDto?.University?.Name,
                    IsFullTime = psql?.IsFullTime,
                    IsNewStandard = psql?.IsNewStandard
                };
            }).ToList();
        }

        /// <summary>
        /// 3
        /// </summary>
        /// <param name="studyGroups"></param>
        public void MergeStudyGroup(IEnumerable<Dto.Oracle.StudyGroup> studyGroups)
        {
            _studyGroups = studyGroups.Select(x => new StudyGroup
            {
                Course = x.Course,
                Name = x.Name,
                Qualification = x.Qualification,
                Year = x.Year,
                EndDate = x.EndDate,
                StartDate = x.StartDate
            }).ToList();
        }

        /// <summary>
        /// 4
        /// </summary>
        /// <param name="classes"></param>
        public void MergeClasses(IEnumerable<Dto.Oracle.Class> classes)
        {
            _classes = classes.Select(x => new Class
            {
                Room = x.Room,
                Time = x.Time,
                TeacherFio = x.TeacherFio,
                TeacherId = x.TeacherId,
                Discipline = _disciplines.FirstOrDefault(discipline => discipline.DiscpId == x.Discipline.DiscpId),
                StudyGroup = _studyGroups.FirstOrDefault(studyGroup => studyGroup.Name == x.StudyGroup.Name),
            }).ToList();
        }

        /// <summary>
        /// 5
        /// </summary>
        public void MergeBookInfoDtos(IEnumerable<Dto.MySql.BookInfoDto> infoDtos)
        {
            List<BookInfoDto> list = new List<BookInfoDto>();
            foreach (var x in infoDtos)
            {
                if (x != null)
                    list.Add(new BookInfoDto
                    {
                        Name = x.Name,
                        IsTaken = x.IsTaken,
                        ReturnDate = x.ReturnDate,
                        TakeDate = x.TakeDate,
                        PersonId = x.Person.StudentId
                    });
            }

            _bookInfoDtos = list;
        }

        /// <summary>
        /// 6
        /// </summary>
        public void MergeBuilding(IEnumerable<Dto.Mongo.Building> buildings)
        {
            _buildings = buildings.Select(x => new Building
            {
                Location = x.Location,
                RoomCount = x.RoomCount,
                MongoId = x.Id.ToString()
            }).ToList();
        }

        /// <summary>
        /// 7
        /// </summary>
        /// <param name="conferenceDtos"></param>
        public void MergeConference(IEnumerable<Dto.MySql.ConferenceDto> conferenceDtos)
        {
            _conferenceDtos = conferenceDtos.Select(x => new ConferenceDto
            {
                Name = x.Name,
                Place = x.Place,
                StartTime = x.StartTime
            }).ToList();
        }

        /// <summary>
        /// 8
        /// </summary>
        public void MergeConferencePaticipantDto(IEnumerable<Dto.MySql.ConferenceParticipationDto> participationDtos)
        {
            _conferenceParticipationDtos = participationDtos.Select(x => new ConferenceParticipationDto
            {
                ConferenceDto = _conferenceDtos.FirstOrDefault(c => c.Name == x.ConferenceDto.Name),
                StudentDto = _studentDtos.FirstOrDefault(s => s.StudentId == x.StudentDto.StudentId)
            }).ToList();
        }

        /// <summary>
        /// 9
        /// </summary>
        /// <param name="projectDtos"></param>
        public void MergeProjectDto(IEnumerable<Dto.MySql.ProjectDto> projectDtos)
        {
            _projectDtos = projectDtos.Select(x => new ProjectDto
            {
                Name = x.Name,
                EndDate = x.EndDate,
                StartDate = x.StartDate,
                Author = _studentDtos.FirstOrDefault(s => s.StudentId == x.Author.StudentId)
            }).ToList();
        }

        /// <summary>
        /// 10
        /// </summary>
        /// <param name="projectStudentsCoAuthorDtos"></param>
        public void MergeProjAndCoAuthor(IEnumerable<Dto.MySql.ProjectStudentsCoAuthorDto> projectStudentsCoAuthorDtos)
        {
            _projectStudentsCoAuthorDtos = projectStudentsCoAuthorDtos.Select(x => new ProjectStudentsCoAuthorDto
            {
                Author = _studentDtos.FirstOrDefault(s => s.StudentId == x.Author.StudentId),
                Project = _projectDtos.FirstOrDefault(p => p.Name == x.Project.Name)
            }).ToList();
        }

        /// <summary>
        /// 11
        /// </summary>
        /// <param name="publicationDtos"></param>
        public void MergePublication(IEnumerable<Dto.MySql.PublicationDto> publicationDtos)
        {
            _publicationDtos = publicationDtos.Select(x => new PublicationDto
            {
                Index = x.Index,
                Lang = x.Lang,
                Name = x.Name,
                Type = x.Type,
                PublicationDate = x.PublicationDate,
                PublisherName = x.PublisherName,
                PublisherPlace = x.PublisherPlace,
                PublisherVolume = x.PublisherVolume,
                Author = _studentDtos.FirstOrDefault(s => s.StudentId == x.Author.StudentId)
            }).ToList();
        }

        /// <summary>
        /// 12
        /// </summary>
        /// <param name="publicationCoauthorDtos"></param>
        public void MergePublicationCoAuthor(IEnumerable<PublicationCoauthorDto> publicationCoauthorDtos)
        {
            _publicationCoauthors = publicationCoauthorDtos.Select(x => new PublicationCoauthor
            {
                Author = _studentDtos.FirstOrDefault(s => s.StudentId == x.Author.StudentId),
                Publication = _publicationDtos.FirstOrDefault(p => p.Name == x.Publication.Name)
            }).ToList();
        }

        /// <summary>
        /// 13
        /// </summary>
        /// <param name="roomDtos"></param>
        public void MergeRoom(IEnumerable<RoomDto> roomDtos)
        {
            _rooms = roomDtos.Select(x => new Room
            {
                Capacity = x.Capacity,
                Number = x.Number,
                DisinfectionDate = x.DisinfectionDate,
                IsInsects = x.IsInsects,
                MaxCapacity = x.MaxCapacity,
                Build = _buildings.FirstOrDefault(b => b.MongoId == x.BuildId.Id.ToString()),
                MongoId = x.Id.ToString()
            }).ToList();
        }

        /// <summary>
        /// 14
        /// </summary>
        /// <param name="rents"></param>
        public void MergeRents(IEnumerable<Dto.Mongo.Rent> rents)
        {
            _rents = rents.Select(x => GenerateRent(x)).ToList();
        }

        private Dto.RuleDto.Rent GenerateRent(Rent x)
        {
            return new Dto.RuleDto.Rent
            {
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Student = _studentDtos.FirstOrDefault(s => s.MongoId == x.Student.Id.ToString()),
                Room = _rooms.FirstOrDefault(r => r.MongoId == x.Room.Id.ToString())
            };
        }

        public void MergeResults(IEnumerable<Dto.Oracle.Result> oresult, IEnumerable<Dto.Psql.Result> presult)
        {
            var data = oresult.Select(x => (x.Student.StudentId, x.Discipline.DiscpId, x.Date))
                .Union(presult.Select(x => (x.Student.StudentId, x.Discipline.DiscpId, x.Date)));
            _results = data.Select(x =>
            {
                var o = oresult.FirstOrDefault(r =>
                    r.Date.Equals(x.Date) && r.Student.StudentId == x.StudentId && r.Discipline.DiscpId == x.DiscpId);
                var p = presult.FirstOrDefault(r =>
                    r.Date.Equals(x.Date) && r.Student.StudentId == x.StudentId && r.Discipline.DiscpId == x.DiscpId);
                return new Result
                {
                    Date = o?.Date ?? p?.Date,
                    Mark = o?.Mark ?? p?.Mark,
                    Student = GenerateStudent(o, p),
                    Discipline = GenerateDiscipline(o, p)
                };
            }).ToList();
        }

        /// <summary>
        /// 15
        /// </summary>
        /// <param name="groupDtos"></param>
        public void MergeStudentGroupDto(IEnumerable<Dto.Oracle.StudentGroupDto> groupDtos)
        {
            _studentGroupDtos = groupDtos.Select(x => new StudentGroupDto
            {
                StudentDto = _studentDtos.FirstOrDefault(s => s.StudentId == x.StudentDto.StudentId),
                StudyGroup = _studyGroups.FirstOrDefault(s => s.Name == x.StudyGroup.Name)
            }).ToList();
        }

        public void GenerateBirthdayPlace()
        {
            var places = new List<BirthdayPlace>();
            foreach (var student in _studentDtos)
            {
                var bp = new BirthdayPlace(student.Location);
                if (places.Count(x => x.City == bp.City && x.Country == bp.Country && x.Street == bp.Street) ==
                    1) continue;
                places.Add(bp);
            }

            _birthdayPlaces = places;
        }

        public void GeneratePubPlaces()
        {
            var places = new List<PublicationPlace>();
            foreach (var publication in _publicationDtos)
            {
                var bp = new PublicationPlace(publication.PublisherPlace);
                if (places.Count(x => x.City == bp.City && x.Country == bp.Country && x.Street == bp.Street) ==
                    1) continue;
                places.Add(bp);
            }

            _publicationPlaces = places;
        }

        public void GenerateTimes()
        {
            var timesList = new List<Time>();


            foreach (var publication in _publicationDtos)
            {
                var t = new Time(publication.PublicationDate);
                var gt = timesList.FirstOrDefault(x => x.Year == t.Year && x.Sem == t.Sem);
                if (gt == null)
                {
                    timesList.Add(t);
                }
            }

            foreach (var @group in _studyGroups)
            {
                var t = new Time(group.EndDate);
                var gt = timesList.FirstOrDefault(x => x.Year == t.Year && x.Sem == t.Sem);
                if (gt == null)
                {
                    timesList.Add(t);
                }
            }

            foreach (var @group in _studyGroups)
            {
                var t = new Time(group.StartDate);
                var gt = timesList.FirstOrDefault(x => x.Year == t.Year && x.Sem == t.Sem);
                if (gt == null)
                {
                    timesList.Add(t);
                }
            }

            var minYear = timesList.Min(x => x.Year);
            var maxYear = timesList.Max(x => x.Year);
            for (var i = minYear; i < maxYear; i++)
            {
                var firstDate = new DateTime(i, 1, 1);
                var secondDate = new DateTime(i, 9, 1);
                var firstTime = new Time(firstDate);
                var secondTime = new Time(secondDate);
                var firstTimeCheck = timesList.FirstOrDefault(x => x.Year == firstTime.Year && x.Sem == firstTime.Sem);
                if (firstTimeCheck == null)
                {
                    timesList.Add(firstTime);
                }

                var secondTimeCheck =
                    timesList.FirstOrDefault(x => x.Year == secondTime.Year && x.Sem == secondTime.Sem);
                if (firstTimeCheck == null)
                {
                    timesList.Add(secondTime);
                }
            }

            _times = timesList;
        }

        public void GenerateFact2()
        {
            var facts = new List<Fact2>();
            foreach (var time in _times)
            {
                facts.AddRange(_birthdayPlaces.Select(x => GenerateFact2Single(x, time)));
            }

            _fact2 = facts;
        }

        private Fact2 GenerateFact2Single(BirthdayPlace birthdayPlace, Time time)
        {
            var fact = new Fact2
            {
                Time = time,
                BirthdayPlace = birthdayPlace
            };


            var studyGroups = _studyGroups.Where(x =>
                x.Year == time.Year.ToString() && time.Sem == 2 ||
                (x.Year == (time.Year - 1).ToString() && time.Sem == 1)).ToList();

            var studentDtos = _studentGroupDtos.Where(x => studyGroups.Contains(x.StudyGroup)).Select(x => x.StudentDto)
                .Distinct().Where(
                    s =>
                    {
                        var place = new BirthdayPlace(s.Location);
                        return place.City == birthdayPlace.City && birthdayPlace.Country == place.Country &&
                               birthdayPlace.Street == place.Street;
                    }).ToList();


            fact.Count = studentDtos.Count;
            return fact;
        }

        public void GenerateFact3()
        {
            var facts = new List<Fact3>();
            foreach (var time in _times)
            {
                facts.AddRange(_publicationPlaces.Select(x => GenerateFact3Single(x, time)));
            }

            _fact3 = facts;
        }

        private Fact3 GenerateFact3Single(PublicationPlace publicationPlace, Time time)
        {
            var fact = new Fact3
            {
                Time = time,
                PublicationPlace = publicationPlace
            };
            var actualPublications = _publicationDtos.Where(x =>
            {
                var t = new Time(x.PublicationDate);
                return t.Year == time.Year && t.Sem == time.Sem;
            }).Where(x =>
            {
                var p = new PublicationPlace(x.PublisherPlace);
                return p.City == publicationPlace.City && p.Country == publicationPlace.Country &&
                       p.Street == publicationPlace.Street;
            }).ToList();

            var actualPublicationsCount = _publicationCoauthors.Count(x => actualPublications.Contains(x.Publication)) +
                                          actualPublications.Count;
            fact.Count = actualPublicationsCount;
            return fact;
        }

        public void GenerateFact1()
        {
            var facts = new List<Fact1>();
            facts.AddRange(_times.Select(x => GenerateFact1Itteration(x)));
            _fact1 = facts;
        }

        public void GenerateFact4()
        {
            var facts = new List<Fact4>();
            foreach (var building in _buildings)
            {
                facts.AddRange(_times.Select(x => GenerateFact4Itteration(x, building)));
            }

            _fact4 = facts;
        }

        private Fact4 GenerateFact4Itteration(Time time, Building building)
        {
            var fact = new Fact4
            {
                Time = time,
                Building = building
            };

            var rents = _rents.Where(x =>
                x.Room.Build == building && ((x.StartDate.Year == time.Year && time.Sem == 2) ||
                                             (x.StartDate.Year == time.Year - 1 && time.Sem == 1))).ToList();
            if (rents.Count == 0)
            {
                fact.PeopleInOneRoom = 0;
            }
            else
            {
                var g = rents.GroupBy(x => x.Room).Where(x => x.Any()).ToList();
                fact.PeopleInOneRoom = g.Select(x => x.Count()).Average();
            }

            var students = rents.Select(x => x.Student).Where(x => x != null).ToList();
            if (students.Count != 0)
            {
            }

            var groupREnumerable = _results
                .Where(x => students
                    .Select(s => s.StudentId)
                    .Contains(x.Student.StudentId))
                .GroupBy(x => x.Student)
                .ToList();
            var studentMarks = groupREnumerable.Select(AverageMark).ToList();
            fact.AverageScoreFive = studentMarks.Count(x => (int)Math.Round(x.Item2) == 5);
            fact.AverageScoreFour = studentMarks.Count(x => (int)Math.Round(x.Item2) == 4);
            fact.AverageScoreThree = studentMarks.Count(x => (int)Math.Round(x.Item2) == 3);
            fact.Losers = groupREnumerable.Count(x => x.Any(m => m.Mark == 2));
            return fact;
        }

        private Fact1 GenerateFact1Itteration(Time time)
        {
            var fact = new Fact1();
            fact.Time = time;
            if (time.Sem == 1)
            {
                fact.CountBlueDiplomas = 0;
                fact.CountRedDiplomas = 0;
            }
            else
            {
                var studyGroups = _studyGroups.Where(x => x.Course is 4 or 6 && x.Year == time.Year.ToString());
                var students = _studentGroupDtos.Where(x => studyGroups.Contains(x.StudyGroup))
                    .Select(x => x.StudentDto)
                    .ToList();
                var groupREnumerable = _results.Where(x => students.Contains(x.Student)).GroupBy(x => x.Student);
                var studentDtos = groupREnumerable.Select(AverageMark).ToList();
                fact.CountRedDiplomas = studentDtos.Count(x => x.Item2 >= 4.75);
                fact.CountBlueDiplomas = studentDtos.Count(x => x.Item2 < 4.75);
            }

            {
                fact.CountPublication = _publicationDtos.Where(x =>
                {
                    var _time = new Time(x.PublicationDate);

                    return _time.Year == time.Year && _time.Sem == time.Sem;
                }).Count();
                fact.ActiveReadTickets = _bookInfoDtos.Where(x =>
                    {
                        var _time = new Time(x.TakeDate);

                        return _time.Year == time.Year && _time.Sem == time.Sem;
                    }).Where(x => _studentDtos.Select(x => x.StudentId).Contains(x.PersonId)).GroupBy(x => x.PersonId)
                    .Count();
                fact.CountConference = _conferenceDtos.Where(x =>
                {
                    var _time = new Time(x.StartTime);

                    return _time.Year == time.Year && _time.Sem == time.Sem;
                }).Count();
                fact.ActiveReadTeacherTickets = _bookInfoDtos.Where(x =>
                    {
                        var _time = new Time(x.TakeDate);

                        return _time.Year == time.Year && _time.Sem == time.Sem;
                    }).Where(x => !_studentDtos.Select(x => x.StudentId).Contains(x.PersonId)).GroupBy(x => x.PersonId)
                    .Count();
                fact.CountInBuilding = _rents.Count(x => x.StartDate.Year == time.Year || x.EndDate.Year == time.Year);
                fact.CountOutOfBuilding = 1000 - fact.CountInBuilding;
            }
            return fact;
        }


        private (StudentDto, double) AverageMark(IGrouping<StudentDto, Result> grouping)
        {
            var average = grouping.Average(x => x.Mark ?? 0);
            return (grouping.Key, average);
        }

        private Discipline GenerateDiscipline(Dto.Oracle.Result oResult, Dto.Psql.Result pResult)
        {
            if (oResult == null && pResult == null) return null;
            return oResult != null
                ? _disciplines.FirstOrDefault(x => x.DiscpId == oResult.Discipline.DiscpId)
                : _disciplines.FirstOrDefault(x => x.DiscpId == pResult.Discipline.DiscpId);
        }

        private StudentDto GenerateStudent(Dto.Oracle.Result oResult, Dto.Psql.Result pResult)
        {
            if (oResult == null && pResult == null) return null;
            return oResult != null
                ? _studentDtos.FirstOrDefault(x => x.StudentId == oResult.Student.StudentId)
                : _studentDtos.FirstOrDefault(x => x.StudentId == pResult.Student.StudentId);
        }
    }
}