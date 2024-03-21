using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRiseSet
{
    public class SunState
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateTimeUTC { get; set; }
        public double JD2000Utc { get; set; }
        public double GastH { get; set; }
        public double Precision { get; set; }
        public SunLongitudeDistance SunLongitudeDistance { get; set; }
        public SunRightAscensionDeclination SunRightAscensionDeclination { get; set; }
        public LHAAltitudeAzimuth LHAAltitudeAzimuth { get; set; }
        public SunPosition SunPosition { get; set; }
    }
}
