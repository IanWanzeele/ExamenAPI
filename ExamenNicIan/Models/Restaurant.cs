using Newtonsoft.Json;

namespace ExamenNicIan.Models
{

    public class Restaurant
    {
        public float version { get; set; }
        public string generator { get; set; }
        public Osm3s osm3s { get; set; }
        public Element[] elements { get; set; }
    }

    public class Osm3s
    {
        public DateTime timestamp_osm_base { get; set; }
        public string copyright { get; set; }
    }

    public class Element
    {
        public string type { get; set; }
        public long id { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public Tags tags { get; set; }
        
    }

    public class Tags
    {
        
        [JsonProperty("addr:housenumber")]
        public string addrhousenumber { get; set; }
        [JsonProperty("addr:street")]
        public string addrstreet { get; set; }
        public string amenity { get; set; }
        public string cuisine { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public string wheelchair { get; set; }
        [JsonProperty("addr:city")]
        public string addrcity { get; set; }
        [JsonProperty("addr:postcode")]
        public string addrpostcode { get; set; }
        public string stars { get; set; }
        public string description { get; set; }
        public string email { get; set; }
        public string opening_hourskitchen { get; set; }
        public string opening_hours { get; set; }
        [JsonProperty("addr:country")]
        public string addrcountry { get; set; }
        [JsonProperty("addr:province")]
        public string addrprovince { get; set; }
        [JsonProperty("addr:suburb")]
        public string addrsuburb { get; set; }
        public string toiletswheelchair { get; set; }
        public string changing_table { get; set; }
        public string changing_tablecount { get; set; }
        public string changing_tablefee { get; set; }
        public string changing_tablelocation { get; set; }
        public string delivery { get; set; }
        public string highchair { get; set; }
        public string kids_area { get; set; }
        public string kids_areaoutdoor { get; set; }
        public string level { get; set; }
        public string microbrewery { get; set; }
        public string outdoor_seating { get; set; }
        public string toilets { get; set; }
        public string toiletsaccess { get; set; }
        public string contactfacebook { get; set; }
        public string internet_access { get; set; }
        public string _operator { get; set; }
        public string tourism { get; set; }
        public string shop { get; set; }
        public string dietvegan { get; set; }
        public string dietvegetarian { get; set; }
        public string organic { get; set; }
        public string takeaway { get; set; }
        public string addrunit { get; set; }
        public string pub { get; set; }
        public string servicebicyclediy { get; set; }
        public string servicebicyclepump { get; set; }
        public string contactmobile { get; set; }
    }

}
