using System;
using System.Collections.Generic;
using System.Text;

namespace TripLoggerServices.Models
{
    public class TripEntry
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsRoundTrip { get; set; }
        public string TripFrom { get; set; }
        public string TripTo { get; set; }
        public string Description { get; set; }
        public DistanceDetail Distance { get; set; }
    }
}
