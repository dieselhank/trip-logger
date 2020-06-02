using System;

namespace TripLoggerServices.Models
{
    public class TripPut
    {
        public DateTime Date { get; set; }
        public string TripFrom { get; set; }
        public string TripTo { get; set; }
        public string Description { get; set; }
        public DistanceDetail Distance { get; set; }
    }
}
