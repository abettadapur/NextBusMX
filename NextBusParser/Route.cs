using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusParser
{
    public class Route
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("-color")]
        public string Color { get; set; }
        [JsonProperty("stop")]
        public List<Stop> Stops { get; set; }
        [JsonProperty("direction")]
        public List<Direction> Directions {get; set;}
        

    }
}
