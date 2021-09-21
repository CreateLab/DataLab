using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLab.Dto.MySql
{
    public class MySqlDbContext:DbContext
    {
        public DbSet<BookInfoDto> BookInfoDtos { get; set; }
        public DbSet<ConferenceDto> ConferenceDtos { get; set; }
        public DbSet<ProjectDto> ProjectDtos { get; set; }
       
        public DbSet<StudentDto> StudentDtos { get; set; }
        public DbSet<PublicationDto> PublicationDtos { get; set; }
        public DbSet<ProjectStudentsCoAuthorDto> ProjectStudentsCoAuthorDtos { get; set; }
        public DbSet<PublicationCoauthorDto> PublicationCoauthorDtos { get; set; }
        
        public DbSet<ConferenceParticipationDto> ConferenceParticipationDtos { get; set; } 
        public MySqlDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=admin;password=admin;database=usersdb5;", 
                new MySqlServerVersion(new Version(8, 0, 26))
            );
        }
    }
}