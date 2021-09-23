using DataLab.Dto.Psql;
using Microsoft.EntityFrameworkCore;

namespace DataLab.Dto.Oracle
{
    public class OracleAppContext:DbContext
    {
        public DbSet<Class> Classes { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<StudentDto> StudentDtos { get; set; }
        public DbSet<StudentGroupDto> StudentGroupDtos { get; set; }
        public DbSet<StudyGroup> StudyGroups { get; set; }

        public OracleAppContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=11521))(CONNECT_DATA=(SERVICE_NAME=orclpdb1)));User ID=rbdz;Password=rbdz;");
        }
    }
}