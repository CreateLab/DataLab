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

        public IEnumerable<StudentGroupDto> StudentGroupDtos => _studentGroupDtos;

        public IEnumerable<StudentDto> StudentDtos => _studentDtos;

        public IEnumerable<Discipline> Disciplines => _disciplines;

        public IEnumerable<StudyGroup> StudyGroups => _studyGroups;

        public IEnumerable<Class> Classes => _classes;

        public IEnumerable<BookInfoDto> BookInfoDtos => _bookInfoDtos;

        public IEnumerable<Building> Buildings => _buildings;

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
                        Student = _studentDtos.FirstOrDefault(s => s.StudentId == x.Student.StudentId)
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
            _rents = rents.Select(x => new Dto.RuleDto.Rent
            {
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Student = _studentDtos.FirstOrDefault(s => s.MongoId == x.Student.Id.ToString()),
                Room = _rooms.FirstOrDefault(r => r.MongoId == x.Room.Id.ToString())
            }).ToList();
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

        public void MergeStudentGroupDto(IEnumerable<Dto.Oracle.StudentGroupDto> groupDtos)
        {
            _studentGroupDtos = groupDtos.Select(x => new StudentGroupDto
            {
                StudentDto = _studentDtos.FirstOrDefault(s => s.StudentId == x.StudentDto.StudentId),
                StudyGroup = _studyGroups.FirstOrDefault(s => s.Name == x.StudyGroup.Name)
            }).ToList();
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