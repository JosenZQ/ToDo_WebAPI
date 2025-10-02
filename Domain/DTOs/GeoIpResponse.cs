using System.Text.Json.Serialization;

namespace Domain.DTOs
{
    public class GeoIpResponse
    {
        public string country_name { get; set; }
        public string state_prov { get; set; }
        public string city { get; set; }

        [JsonPropertyName("time_zone")]
        public TimeZoneData TimeZone { get; set; }
    }

    public class TimeZoneData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("offset")]
        public double Offset { get; set; }

        [JsonPropertyName("current_time")]
        public string CurrentTime { get; set; }

        [JsonPropertyName("is_dst")]
        public bool IsDst { get; set; }
    }
}
