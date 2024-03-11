namespace ExamenNicIan.Models
{
    public class Restaurant
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public required double Latitude { get; set; }
            public required double Longitude { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string Postcode { get; set; }
            public string Country { get; set; }
            public string? Cuisine { get; set; }

    }
}
