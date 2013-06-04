using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace NextBusMetro.Converters
{
    public class ColorConverter
    {
        static Color red = Color.FromArgb(255, 162, 0, 37);
        static Color blue = Color.FromArgb(255, 0, 80, 239);
        static Color green = Color.FromArgb(255, 0, 138, 0);
        static Color yellow = Color.FromArgb(255, 241, 163, 11);
        static Color purple = Color.FromArgb(255, 106, 31, 152);
        static Color pink = Color.FromArgb(255, 204, 51, 204);

        public static Color getColor(string value)
        {
            value = value.ToLower();
            switch(value)
            {
                case "ff1100":
                    return red;
                case "0000ff":
                    return blue;
                case "00cc66":
                    return green;
                case "ffcc00":
                    return yellow;
                case "cc33cc" :
                    return pink;
                case "7849a8":
                    return purple;
               
            }
            return Color.FromArgb(0, 0, 0, 0);
        }
            
    }
}
