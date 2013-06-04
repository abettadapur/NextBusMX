using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusParser
{
    public class DataSource
    {
        [JsonProperty("route")]
        public List<Route> Routes { get; set; }
    }
}
