using System;

namespace TripLoggerServices.Models
{
    public class TripEntry
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "tripFrom")]
        public string TripFrom { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "tripTo")]
        public string TripTo { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "distance")]
        public DistanceDetail Distance { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "createdOn")]
        public DateTime CreatedOn { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "modifiedOn")]
        public DateTime ModifiedOn { get; set; }
    }
}
