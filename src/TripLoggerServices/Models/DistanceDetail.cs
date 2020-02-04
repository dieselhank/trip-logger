using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TripLoggerServices.Models
{
    public class DistanceDetail
    {
        public double Length { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Units Units { get; set; }
    }
}
