using Microsoft.EntityFrameworkCore;

namespace DataLab.Dto.Psql
{
    public class PsqlAppContext:DbContext
    {
        public DbSet<DisciplineDto> Disciplines { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<SpecialisationDto> Specialisations { get; set; }
        public DbSet<StudentDto> StudentDtos { get; set; }
        public DbSet<University> Universities { get; set; }

        public PsqlAppContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=admin");
        }
    }
}