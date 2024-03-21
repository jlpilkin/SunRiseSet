using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRiseSet
{
    public class SunStates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public SunState SolarNoon { get; set; }
        public SunState SolarMidnight { get; set; }
        public SunState Sunrise { get; set; }
        public SunState Sunset { get; set; }
        public SunState CivilDawn { get; set; }
        public SunState CivilDusk { get; set; }
        public SunState NauticalDawn { get; set; }
        public SunState NauticalDusk { get; set; }
        public SunState AstronomicalDawn { get; set; }
        public SunState AstronomicalDusk { get; set; }
    }
}
