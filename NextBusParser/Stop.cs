using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextBusParser
{
   public class Stop
    {
       [JsonProperty("tag")]
       public string Tag { get; set; }
       [JsonProperty("title")]
       public string Title { get; set; }
       [JsonProperty("lat")]
       public double Latitude { get; set; }
       [JsonProperty("lon")]
       public double Longitude { get; set; }
       [JsonProperty("stopid")]
       public int Id { get; set; }
    }
}
