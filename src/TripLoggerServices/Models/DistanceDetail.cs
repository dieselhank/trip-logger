using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TripLoggerServices.Models
{
    public class DistanceDetail
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "length")]
        public double Length { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "units")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Units Units { get; set; }
    }
}
