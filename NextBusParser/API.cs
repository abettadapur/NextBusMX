using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NextBusParser
{
    public class API
    {
        public static DataSource ds { get { return _ds; } set { _ds = value; } }
        private static DataSource _ds;
        public static bool isInitialized { get { return _isInitialized; } set { _isInitialized = value; } }
        private static bool _isInitialized = false;

        public async static Task initializeRoutes()
        {
            //var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"routeconfig.txt");
            Assembly assem = typeof(API).GetTypeInfo().Assembly;
            var names = assem.GetManifestResourceNames();
            var stream = assem.GetManifestResourceStream("NextBusParser.routeconfig.txt");
            StreamReader reader = new StreamReader(stream);
            
            string routedata = await reader.ReadToEndAsync();

            DataSource ds = JsonConvert.DeserializeObject<DataSource>(routedata);
            _ds = ds;
            reader.Dispose();

            foreach (Route r in ds.Routes)
            {
                foreach (Direction d in r.Directions)
                {
                    for (int i = 0; i < d.Stops.Count; i++)
                    {
                        Stop s = d.Stops[i];
                        s = r.Stops.Where(e => e.Tag.Equals(s.Tag)).First();
                        d.Stops[i] = s;
                    }
                }
            }

            _isInitialized = true;
        }

        public async static Task<int[]> getPrediction(string route, string direction, string stop)
        {
            StringBuilder sb = new StringBuilder("http://desolate-escarpment-6039.herokuapp.com/bus/get?");
            sb.Append("route="+route+"&direction="+direction+"&stop="+stop);
            string url = sb.ToString();

            WebRequest request = WebRequest.CreateHttp(url);
            WebResponse response = await request.GetResponseAsync();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string responseText = reader.ReadToEnd();
            try
            {
                Prediction prediction = JsonConvert.DeserializeObject<Prediction>(responseText);
                return prediction.predictions;
            }
            catch (JsonSerializationException jsex)
            { return null; }

           
            
        }
        class Prediction
        {
            public int[] predictions { get; set; }
        }

        


    }
}
