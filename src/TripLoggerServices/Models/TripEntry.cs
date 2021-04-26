using Newtonsoft.Json;
using System;

namespace TripLoggerServices.Models
{
    public class TripEntry
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
        [JsonProperty(PropertyName = "tripFrom")]
        public string TripFrom { get; set; }
        [JsonProperty(PropertyName = "tripTo")]
        public string TripTo { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "distance")]
        public DistanceDetail Distance { get; set; }
        [JsonProperty(PropertyName = "createdOn")]
        public DateTime CreatedOn { get; set; }
        [JsonProperty(PropertyName = "modifiedOn")]
        public DateTime ModifiedOn { get; set; }
    }
}
