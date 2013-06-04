using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextBusParser
{
    public class Direction
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("stop")]
        public List<Stop> Stops { get; set; }
    }
}
