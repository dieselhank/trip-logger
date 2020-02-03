using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TripLoggerServices.Models
{
    public class DistanceDetail
    {
        public double Distance { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Units Units { get; set; }
    }
}
