public class GeoLocationResponse
{
    public List<GeoLocationResult> Results { get; set; }
}

public class GeoLocationResult
{
    public Address Address { get; set; }
    public Position Position { get; set; }
}

public class Address
{
    public string Municipality { get; set; }
    public string CountryCode { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
    public string FreeformAddress { get; set; }
}

public class Position
{
    public double Lat { get; set; }
    public double Lon { get; set; }
}