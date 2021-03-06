namespace DataLab.Dto.RuleDto
{
    public class PublicationPlace
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public PublicationPlace()
        {
            
        }
        public PublicationPlace(string address)
        {
            // пр. Центральная, 098, Барнаул, Бруней
            var data = address.Split(",");
            Street = data[0];
            City = data[2];
            Country = data[3];
        }
    }
}