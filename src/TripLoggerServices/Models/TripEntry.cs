using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
