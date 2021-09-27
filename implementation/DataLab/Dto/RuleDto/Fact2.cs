using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataLab.Dto.RuleDto
{
    public class Fact2
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public BirthdayPlace BirthdayPlace { get; set; }
        public Time Time { get; set; }
    }
}