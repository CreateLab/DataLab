using DataLab.Dto.Oracle;
using Microsoft.EntityFrameworkCore;

namespace DataLab.Dto.RuleDto
{
    public class RukeAppContext : DbContext
    {
        public DbSet<Class> Classes { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<StudentDto> StudentDtos { get; set; }
        public DbSet<StudentGroupDto> StudentGroupDtos { get; set; }
        public DbSet<StudyGroup> StudyGroups { get; set; }
        public DbSet<BookInfoDto> BookInfoDtos { get; set; }
        public DbSet<ConferenceDto> ConferenceDtos { get; set; }
        public DbSet<ProjectDto> ProjectDtos { get; set; }
        public DbSet<PublicationDto> PublicationDtos { get; set; }
        
        public  DbSet<PublicationCoauthor> PublicationCoauthors { get; set; }
        public DbSet<ProjectStudentsCoAuthorDto> ProjectStudentsCoAuthorDtos { get; set; }
        public DbSet<ConferenceParticipationDto> ConferenceParticipationDtos { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Rent> Rents { get; set; }

        public RukeAppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(
                "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=pdb1.localdomain)));User ID=rbdz;Password=rbdz;");
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Model.SetMaxIdentifierLength(30);
        }
    }
}