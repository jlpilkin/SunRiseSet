using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Radios;

namespace SunRiseSet
{
    public class SunCalc
    {
        public const double PI = 3.14159265358979323846;
        public const double RAD_TO_DEG = 180.0 / PI;
        public const double DEG_TO_RAD = PI / 180.0;
        private const double SECONDS_UTC_TO_IAT = 37.0;
        private const double SECONDS_IAT_TO_GCT = 32.184;
        public static decimal SECONDS_PER_DAY_D = (decimal)TimeSpan.FromDays(1.0).TotalSeconds;
        public static decimal SECONDS_PER_HOUR_D = (decimal)TimeSpan.FromHours(1.0).TotalSeconds;
        public static decimal SECONDS_PER_MINUTE_D = (decimal)TimeSpan.FromMinutes(1.0).TotalSeconds;
        public static decimal MILLISECONDS_PER_DAY_D = (decimal)TimeSpan.FromDays(1.0).TotalMilliseconds;
        public static double SECONDS_PER_DAY = (double)SECONDS_PER_DAY_D;
        public static double SECONDS_PER_HOUR = (double)SECONDS_PER_HOUR_D;
        public static double SECONDS_PER_MINUTE = (double)SECONDS_PER_MINUTE_D;
        public static double MILLISECONDS_PER_DAY = (double)MILLISECONDS_PER_DAY_D;
        public static decimal JD2000_EPOCH_D = JulianDayD_WithHours(new DateTime(2000, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc));
        public static double JD2000_EPOCH = (double)JD2000_EPOCH_D;

        /**
         * Calculates the Julian Day number from the given date.
         * Based on L. E. Doggett, Ch. 12, "Calendars", p. 604, in Seidelmann 1992.
         * "These algorithms are valid for all Gregorian calendar dates corresponding
         * to JD >= 0, i.e, dates after −4713 (4714 BC) November 23."
         * 
         * Returns the Julian Day number as a decimal where 0.0 is −4713 (4714 BC) November 23 at 12:00:00 UTC.
         */
        public static decimal JulianDayD(long year, long month, long day)
        {
            return (decimal)((1461L * (year + 4800L + (month - 14L) / 12L)) / 4L + (367L * (month - 2L - 12L * ((month - 14L) / 12L))) / 12L - (3L * ((year + 4900L + (month - 14L) / 12L) / 100L)) / 4L + day - 32075L) - 0.5M;
        }

        /**
         * Calculates the Julian Day number from the given date.
         * Based on L. E. Doggett, Ch. 12, "Calendars", p. 604, in Seidelmann 1992.
         * "These algorithms are valid for all Gregorian calendar dates corresponding
         * to JD >= 0, i.e, dates after −4713 (4714 BC) November 23."
         * 
         * Returns the Julian Day number as a decimal where 0.0 is −4713 (4714 BC) November 23 at 12:00:00 UTC.
         */
        public static double JulianDay( long year, long month, long day )
        {
            return (double)JulianDayD(year, month, day);
        }

        /**
         * Calculates the Julian Day number from the given date.
         * Based on L. E. Doggett, Ch. 12, "Calendars", p. 604, in Seidelmann 1992.
         * "These algorithms are valid for all Gregorian calendar dates corresponding
         * to JD >= 0, i.e, dates after −4713 (4714 BC) November 23."
         * 
         * Returns the Julian Day number as a decimal where 0.0 is −4713 (4714 BC) November 23 at 12:00:00 UTC.
         */
        public static decimal JulianDayD(DateTime date)
        {
            return JulianDayD(date.Year, date.Month, date.Day);
        }

        /**
         * Calculates the Julian Day number from the given date.
         * Based on L. E. Doggett, Ch. 12, "Calendars", p. 604, in Seidelmann 1992.
         * "These algorithms are valid for all Gregorian calendar dates corresponding
         * to JD >= 0, i.e, dates after −4713 (4714 BC) November 23."
         * 
         * Returns the Julian Day number as a decimal where 0.0 is −4713 (4714 BC) November 23 at 12:00:00 UTC.
         */
        public static double JulianDay(DateTime date)
        {
            return (double)JulianDayD(date.Year, date.Month, date.Day);
        }

        /**
         * Calculates the Julian Day number from the given date.
         * Based on L. E. Doggett, Ch. 12, "Calendars", p. 604, in Seidelmann 1992.
         * "These algorithms are valid for all Gregorian calendar dates corresponding
         * to JD >= 0, i.e, dates after −4713 (4714 BC) November 23."
         * 
         * Returns the Julian Day number as a decimal where 0.0 is −4713 (4714 BC) November 23 at 12:00:00 UTC.
         */
        public static decimal JulianDayD_WithHours(DateTime date)
        {
            return JulianDayD_AddHours( JulianDayD(date.Year, date.Month, date.Day), date );
        }

        /**
         * Calculates the Julian Day number from the given date.
         * Based on L. E. Doggett, Ch. 12, "Calendars", p. 604, in Seidelmann 1992.
         * "These algorithms are valid for all Gregorian calendar dates corresponding
         * to JD >= 0, i.e, dates after −4713 (4714 BC) November 23."
         * 
         * Returns the Julian Day number as a decimal where 0.0 is −4713 (4714 BC) November 23 at 12:00:00 UTC.
         */
        public static double JulianDay_WithHours(DateTime date)
        {
            return (double)JulianDayD_WithHours(date);
        }

        /**
         * Adds the given number of hours to the given Julian Day number.
         */
        public static decimal JulianDayD_AddHours(decimal julianDay, DateTime dateTime)
        {
            return julianDay
                + (((decimal)dateTime.Hour * SECONDS_PER_HOUR_D) / SECONDS_PER_DAY_D)
                + (((decimal)dateTime.Minute * SECONDS_PER_MINUTE_D) / SECONDS_PER_DAY_D)
                + ((decimal)dateTime.Second / SECONDS_PER_DAY_D)
                + ((decimal)dateTime.Millisecond / MILLISECONDS_PER_DAY_D);
        }

        /**
         * Adds the given number of hours to the given Julian Day number.
         */
        public static decimal JulianDayD_AddHours(double julianDay, DateTime dateTime)
        {
            return JulianDayD_AddHours((decimal)julianDay, dateTime);
        }

        /**
         * Adds the given number of hours to the given Julian Day number.
         */
        public static double JulianDay_AddHours(double julianDay, DateTime dateTime)
        {
            return (double)JulianDayD_AddHours((decimal)julianDay, dateTime);
        }

        /**
         * Adds the given number of hours to the given Julian Day number.
         */
        public static double JulianDay_AddHours(decimal julianDay, DateTime dateTime)
        {
            return (double)JulianDayD_AddHours(julianDay, dateTime);
        }

        /**
         * Converts the given DateTime in UTC to International Atomic Time (IAT).
         **/
        public static DateTime Date_UTC_to_IAT(DateTime date)
        {
            return date.AddSeconds(SECONDS_UTC_TO_IAT); // UTC -> IAT (37 leap seconds as of 2017)
        }

        /**
         * Converts the given DateTime in IAT (International Atomic Time) to Geocentric Coordinate Time (GCT).
         **/
        public static DateTime Date_IAT_to_GCT(DateTime date)
        {
            return date.AddSeconds(SECONDS_IAT_TO_GCT); // IAT -> GCT (32.184 seconds as of 2017)
        }

        /**
         * Converts the given DateTime in UTC to Geocentric Coordinate Time (GCT).
         **/
        public static DateTime Date_UTC_to_GCT(DateTime date)
        {
            return Date_IAT_to_GCT(Date_UTC_to_IAT(date));
        }

        /**
         * Converts the given Julian Day number in UTC to the Julian Day number in GCT.
         **/  
        public static double JulianDayUT_to_JulianDayGCT(double julianDayUT)
        {
            return julianDayUT + (SECONDS_UTC_TO_IAT + SECONDS_IAT_TO_GCT) / SECONDS_PER_DAY;
        }

        /**
         * Converts the given Julian Day number in GCT to the Julian Day number in TT.
         **/
        public static decimal JulianDayTTD_from_GCT( decimal julianDayGct )
        {
            decimal eJD = 2443144.5003725M;
            decimal lG = 6.969290134E-10M;

            return eJD + (julianDayGct - eJD) * (1 - lG);
        }

        /**
         * Converts the given Julian Day number in GCT to the Julian Day number in TT.
         **/
        public static decimal JulianDayTTD_from_GCT( double julianDayGct )
        {
            return JulianDayTTD_from_GCT((decimal)julianDayGct);
        }

        /**
         * Converts the given Julian Day number in GCT to the Julian Day number in TT.
         **/
        public static double JulianDayTT_from_GCT( double julianDayGct )
        {
            return (double)JulianDayTTD_from_GCT(julianDayGct);
        }

        /**
         * Converts the given Julian Day number in GCT to the Julian Day number in TT.
         **/
        public static double JulianDayTT_from_GCT( decimal julianDayGct )
        {
            return (double)JulianDayTTD_from_GCT(julianDayGct);
        }

        /**
         * Calculates the Julian Day number in TT from the given DateTime in UTC.
         **/
        public static decimal JulianDayTTD_with_Hours( DateTime date )
        {
            return JulianDayTTD_from_GCT(JulianDayD_WithHours(Date_UTC_to_GCT(date)));
        }

        /**
         * Calculates the Julian Day number in TT from the given DateTime in UTC.
         **/
        public static double JulianDayTT_with_Hours( DateTime date )
        {
            return (double)JulianDayTTD_with_Hours(date);
        }

        /**
         * Calculates the JD2000 number from the given Julian Day number.
         **/
        public static decimal JulianDay_to_JD2000D( decimal julianDay )
        {
            return julianDay - JD2000_EPOCH_D;
        }

        /**
         * Calculates the JD2000 number from the given Julian Day number.
         **/
        public static decimal JulianDay_to_JD2000D( double julianDay )
        {
            return JulianDay_to_JD2000D((decimal)julianDay);
        }

        /**
         * Calculates the JD2000 number from the given Julian Day number.
         **/
        public static double JulianDay_to_JD2000( decimal julianDay )
        {
            return (double)JulianDay_to_JD2000D(julianDay);
        }

        /**
         * Calculates the JD2000 number from the given Julian Day number.
         **/
        public static double JulianDay_to_JD2000( double julianDay )
        {
            return (double)JulianDay_to_JD2000D(julianDay);
        }

        /*
         * Reduce angle to within 0..360 degrees
         */
        public static double ClampAngle360( double angle )
        {
            return angle - 360.0 * Math.Floor( angle / 360.0 );
        }

        /*
         * Reduce angle to within -180..+180 degrees
         */
        public static double ClampAngle180( double angle )
        {
            return angle - 360.0 * Math.Floor( angle / 360.0 + 0.5 );
        }

        /**
         * Converts hours to seconds.
         **/
        public static double HoursToSeconds(double hours)
        {
            return hours * TimeSpan.FromHours(1.0).TotalSeconds;
        }

        /*
         * Converts degrees to hours.
         */
        public static double DegreesToHours(double degrees)
        {
            return degrees / 15.0;
        }

        /*
         * Converts hours to degrees.
         **/
        public static double HoursToDegrees(double hours)
        {
            return hours * 15.0;
        }

        /**
         * Converts degrees to radians.
         */
        public static double DegreesToRadians(double degrees)
        {
            return degrees * DEG_TO_RAD;
        }

        /**
         * Converts radians to degrees.
         */
        public static double RadiansToDegrees(double radians)
        {
            return radians * RAD_TO_DEG;
        }

        /**
         * Determines the Greenwich Mean Sidereal Time from the Julian Day in TT,
         * the Julian Date at UT0 and the UTC time.  Returns the GMST in total hours.
         * 
         * Reference: https://aa.usno.navy.mil/faq/GAST
         **/
        public static double GMST_H(double julianDayTT, double julianDayUT0, DateTime utcNow)
        {
            double totalHours = (
                    TimeSpan.FromHours(utcNow.Hour)
                    + TimeSpan.FromMinutes(utcNow.Minute)
                    + TimeSpan.FromSeconds(utcNow.Second)
                    + TimeSpan.FromMilliseconds(utcNow.Millisecond)
                ).TotalHours;

            double DTT = (julianDayTT - JD2000_EPOCH);
            double DUT = (julianDayUT0 - JD2000_EPOCH);
            double T = DTT / 36525.0;

            double gmstBase = 6.697375 + 0.065707485828 * DUT + 1.0027379 * totalHours + 0.0854103 * T + 0.0000258 * T * T;
            double gmstResult = gmstBase % 24.0;
            if (gmstResult < 0.0)
            {
                gmstResult += 24.0;
            }
            return gmstResult;
        }

        /**
         * Determines the Greenwich Mean Sidereal Time from the Julian Day in TT,
         * the Julian Date at UT0 and the UTC time.  Returns the GMST in total seconds.
         * 
         * Reference: https://aa.usno.navy.mil/faq/GAST
         **/
        public static double GMST( double julianDayTT, double julianDayUT0, DateTime utcNow )
        {
            return HoursToSeconds( GMST_H( julianDayTT, julianDayUT0, utcNow ) );
        }

        /**
         * Determines the Greenwich Apparent Sidereal Time from the Julian Day in TT,
         * the Julian Date at UT0 and the UTC time.  Returns the GAST in total hours.
         * 
         * Reference: https://aa.usno.navy.mil/faq/GAST
         **/
        public static double GAST_H( double julianDayTT, double julianDayUT0, DateTime utcNow )
        {
            double jd2000TT = julianDayTT - JD2000_EPOCH;
            double omega = 125.04 - 0.052954 * jd2000TT;
            double L = 280.47 + 0.98565 * jd2000TT;
            double deltaPsi = -0.000319 * Math.Sin(omega * DEG_TO_RAD) - 0.000024 * Math.Sin(2 * L * DEG_TO_RAD);
            double epsilon = 23.4393 - 0.0000004 * jd2000TT;
            double eqeq = deltaPsi * Math.Cos(epsilon * DEG_TO_RAD);

            double gastResult = ( GMST_H( julianDayTT, julianDayUT0, utcNow ) + eqeq ) % 24.0;
            if (gastResult < 0.0)
            {
                gastResult += 24.0;
            }
            return gastResult;
        }

        /**
         * Determines the Greenwich Apparent Sidereal Time from the Julian Day in TT,
         * the Julian Date at UT0 and the UTC time.  Returns the GAST in total seconds.
         * 
         * Reference: https://aa.usno.navy.mil/faq/GAST
         **/
        public static double GAST( double julianDayTT, double julianDayUT0, DateTime utcNow )
        {
            return HoursToSeconds( GAST_H( julianDayTT, julianDayUT0, utcNow ) );
        }

        /**
         * Determines the Earth rotation angle (ERA) in degrees at the given Julian UT Date since J2000.
         * 
         * Explanatory Supplement to the Astronomical Almanac, 3rd Edition: Urban & Seidelmann 2013, p. 78.
         */
        public static double ERA( double jd2000UT )
        {
            return ClampAngle360( ( ( 2.0 * PI ) * ( 0.7790572732640 + 1.00273781191135448 * ( jd2000UT ) ) ) * RAD_TO_DEG );
        }

        /**
         * Computes the Sun's ecliptic longitude and distance
         * 
         * Reference: https://aa.usno.navy.mil/faq/sun_approx
         **/
        public static SunLongitudeDistance GetSunLongitudeDistance( double jdJ2000UT )
        {
            SunLongitudeDistance result = new SunLongitudeDistance();

            double L = ClampAngle360( 280.459 + 0.98564736 * jdJ2000UT ); // mean longitude, degree
            double g = ClampAngle360( 357.529 + 0.98560028 * jdJ2000UT ); // mean anomaly, degree
            result.Longitude = ClampAngle360( L + 1.915 * Math.Sin( g * DEG_TO_RAD ) + 0.020 * Math.Sin( 2 * g * DEG_TO_RAD ) ); // true longitude, degree
            result.Distance = 1.00014 - 0.01671 * Math.Cos( g * DEG_TO_RAD ) - 0.00014 * Math.Cos( 2 * g * DEG_TO_RAD ); // distance, AU

            return result;
        }

        /**
         * Calculate the Sun's right ascension and declination
         * 
         * Reference: https://aa.usno.navy.mil/faq/sun_approx
         **/
        public static SunRightAscensionDeclination GetSunRightAscensionDeclination(double jdJ2000UT, SunLongitudeDistance sunLongitudeDistance)
        {
            SunRightAscensionDeclination result = new SunRightAscensionDeclination();

            double ecl = 23.439 - 0.00000036 * jdJ2000UT; // obliquity of the ecliptic, degree

            result.RightAscension = ClampAngle360(
                Math.Atan2(
                    Math.Sin( sunLongitudeDistance.Longitude * DEG_TO_RAD ) * Math.Cos( ecl * DEG_TO_RAD ),
                    Math.Cos( sunLongitudeDistance.Longitude * DEG_TO_RAD )
                ) * RAD_TO_DEG
            ); // right ascension, degree
            result.Declination = Math.Asin( Math.Sin( sunLongitudeDistance.Longitude * DEG_TO_RAD ) * Math.Sin( ecl * DEG_TO_RAD ) ) * RAD_TO_DEG; // declination, degree

            return result;
        }

        /**
         * Calculate the Sun's Local Hour Angle, Altitude and Azimuth
         * 
         * Reference: https://aa.usno.navy.mil/faq/alt_az
         */
        public static LHAAltitudeAzimuth GetSunLHAAltitudeAzimuth(double gastH, double latitude, double longitude, double rightAscensionDegrees, double declination)
        {
            LHAAltitudeAzimuth result = new LHAAltitudeAzimuth();

            double LHA = ClampAngle360(HoursToDegrees(gastH - DegreesToHours(rightAscensionDegrees)) + longitude);
            result.LHA = LHA;

            double sinAltitude = Math.Sin(declination * DEG_TO_RAD) * Math.Sin(latitude * DEG_TO_RAD) + Math.Cos(declination * DEG_TO_RAD) * Math.Cos(latitude * DEG_TO_RAD) * Math.Cos(LHA * DEG_TO_RAD);
            double altitude = Math.Asin(sinAltitude) * RAD_TO_DEG;
            result.Altitude = altitude;

            double azimuth = ClampAngle360(
                Math.Atan2(
                    Math.Sin(LHA * DEG_TO_RAD),
                    Math.Cos(LHA * DEG_TO_RAD) * Math.Sin(latitude * DEG_TO_RAD) - Math.Tan(declination * DEG_TO_RAD) * Math.Cos(latitude * DEG_TO_RAD)
                ) * RAD_TO_DEG + 180.0
            );
            result.Azimuth = azimuth;

            return result;
        }

        /**
         * Determines positions of all Sun states for the given latitude, longitude, Julian Day, UTC time and current Sun positions.
         */
        public static SunStates GetSunStates(
            double latitude,
            double longitude,
            double julianDayUtc0h,
            double jd2000UtcNow,
            double gastHUtcNow,
            DateTime utcNow,
            SunRightAscensionDeclination sunRightAscensionDeclinationUtcNow,
            SunLongitudeDistance sunLongitudeDistanceUtcNow,
            double precisionSolarNoonMidnight = 0.00001,
            double precisionSunriseSunset = 0.0001,
            int maxIterations = 10,
            int multiplier = 4
        )
        {
            SunState sunStateSolarNoon = GetSunStateSolarNoonMidnight(
                SunPosition.Noon,
                latitude,
                longitude,
                jd2000UtcNow,
                gastHUtcNow,
                sunRightAscensionDeclinationUtcNow,
                utcNow,
                precisionSolarNoonMidnight,
                maxIterations
            );

            SunState sunStateSolarMidnight = GetSunStateSolarNoonMidnight(
                SunPosition.Midnight,
                latitude,
                longitude,
                jd2000UtcNow,
                gastHUtcNow,
                sunRightAscensionDeclinationUtcNow,
                utcNow,
                precisionSolarNoonMidnight,
                maxIterations
            );

            SunState sunStateAstronomicalDawn = GetSunStateSunriseSunset(
                latitude,
                longitude,
                julianDayUtc0h,
                sunLongitudeDistanceUtcNow,
                sunStateSolarNoon,
                sunStateSolarMidnight,
                SunPosition.AstronomicalDawn,
                precisionSunriseSunset,
                maxIterations,
                multiplier
            );

            SunState sunStateNauticalDawn = GetSunStateSunriseSunset(
                latitude,
                longitude,
                julianDayUtc0h,
                sunLongitudeDistanceUtcNow,
                sunStateSolarNoon,
                sunStateSolarMidnight,
                SunPosition.NauticalDawn,
                precisionSunriseSunset,
                maxIterations,
                multiplier
            );

            SunState sunStateCivilDawn = GetSunStateSunriseSunset(
                latitude,
                longitude,
                julianDayUtc0h,
                sunLongitudeDistanceUtcNow,
                sunStateSolarNoon,
                sunStateSolarMidnight,
                SunPosition.CivilDawn,
                precisionSunriseSunset,
                maxIterations,
                multiplier
            );

            SunState sunStateSunrise = GetSunStateSunriseSunset(
                latitude,
                longitude,
                julianDayUtc0h,
                sunLongitudeDistanceUtcNow,
                sunStateSolarNoon,
                sunStateSolarMidnight,
                SunPosition.Sunrise,
                precisionSunriseSunset,
                maxIterations,
                multiplier
            );

            SunState sunStateSunset = GetSunStateSunriseSunset(
                latitude,
                longitude,
                julianDayUtc0h,
                sunLongitudeDistanceUtcNow,
                sunStateSolarNoon,
                sunStateSolarMidnight,
                SunPosition.Sunset,
                precisionSunriseSunset,
                maxIterations,
                multiplier
            );

            SunState sunStateCivilDusk = GetSunStateSunriseSunset(
                latitude,
                longitude,
                julianDayUtc0h,
                sunLongitudeDistanceUtcNow,
                sunStateSolarNoon,
                sunStateSolarMidnight,
                SunPosition.CivilDusk,
                precisionSunriseSunset,
                maxIterations,
                multiplier
            );

            SunState sunStateNauticalDusk = GetSunStateSunriseSunset(
                latitude,
                longitude,
                julianDayUtc0h,
                sunLongitudeDistanceUtcNow,
                sunStateSolarNoon,
                sunStateSolarMidnight,
                SunPosition.NauticalDusk,
                precisionSunriseSunset,
                maxIterations,
                multiplier
            );

            SunState sunStateAstronomicalDusk = GetSunStateSunriseSunset(
                latitude,
                longitude,
                julianDayUtc0h,
                sunLongitudeDistanceUtcNow,
                sunStateSolarNoon,
                sunStateSolarMidnight,
                SunPosition.AstronomicalDusk,
                precisionSunriseSunset,
                maxIterations,
                multiplier
            );

            return new SunStates()
            {
                Latitude = latitude,
                Longitude = longitude,
                SolarNoon = sunStateSolarNoon,
                SolarMidnight = sunStateSolarMidnight,
                Sunrise = sunStateSunrise,
                Sunset = sunStateSunset,
                CivilDawn = sunStateCivilDawn,
                CivilDusk = sunStateCivilDusk,
                NauticalDawn = sunStateNauticalDawn,
                NauticalDusk = sunStateNauticalDusk,
                AstronomicalDawn = sunStateAstronomicalDawn,
                AstronomicalDusk = sunStateAstronomicalDusk
            };
        }

        /**
         * Determines the approximate GAST at solar noon given the longitude and the right ascension of the Sun.
         * Solar noon (the point where the Sun is directly at the opposite cardinal direction of the latitude hemisphere)
         * is when the LHA (Local hour angle) is 0 degrees.  Typically, this is when the Sun is the highest in the sky.
         */
        private static double GetApproximateGastHSolarNoon(double longitude, double rightAscensionDegrees)
        {
            double gastDegrees = ClampAngle360(rightAscensionDegrees - longitude);
            return DegreesToHours(gastDegrees);
        }

        /**
         * Determines the approximate GAST at solar noon given the longitude and the right ascension of the Sun.
         * Solar noon (the point where the Sun is directly at the same cardinal direction of the latitude hemisphere)
         * is when the LHA (Local hour angle) is 0 degrees.  Typically, this is when the Sun is the lowest in the sky.
         */
        private static double GetApproximateGastHSolarMidnight(double longitude, double rightAscensionDegrees)
        {
            double gastDegrees = ClampAngle360(rightAscensionDegrees - longitude + 180.0);
            return DegreesToHours(gastDegrees);
        }

        /**
         * Determines the approximate GAST at the given SunPosition given the longitude and the right ascension of the Sun.
         */
        private static double GetApproximateGastHSolarNoonMidnight(SunPosition sunPositionTarget, double longitude, double rightAscensionDegrees)
        {
            switch( sunPositionTarget )
            {
                case SunPosition.Midnight:
                    return GetApproximateGastHSolarMidnight(longitude, rightAscensionDegrees);
                case SunPosition.Noon:
                    return GetApproximateGastHSolarNoon(longitude, rightAscensionDegrees);
                default:
                    throw new ArgumentException("Invalid SunPosition value");
            }
        }

        /**
         * Determines the target altitude for the given SunPosition other than Solar Noon and Solar Midnight.
         **/
        private static double GetTargetAltitude(SunPosition sunPosition, SunLongitudeDistance sunLongitudeDistance)
        {
            double solarApparentRadiusDegrees = 0.2666 / sunLongitudeDistance.Distance;
            switch (sunPosition)
            {
                case SunPosition.Sunrise:
                case SunPosition.Sunset:
                    return -0.833 + 0.2666 - solarApparentRadiusDegrees;    // Position of the Sun when it is just below the horizon
                case SunPosition.CivilDawn:
                case SunPosition.CivilDusk:
                    return -6.0;                                            // Definition of civil twilight
                case SunPosition.NauticalDawn:
                case SunPosition.NauticalDusk:
                    return -12.0;                                           // Definition of nautical twilight
                case SunPosition.AstronomicalDawn:
                case SunPosition.AstronomicalDusk:
                    return -18.0;                                           // Definition of astronomical twilight
                default:
                    throw new ArgumentException("Invalid SunPosition value");
            }
        }

        /**
         * Determines the Sun states for either Solar Noon or Solar Midnight at the given target SunPosition.
         **/
        private static SunState GetSunStateSolarNoonMidnight(
            SunPosition sunPositionTarget,
            double latitude,
            double longitude,
            double jd2000UtcNow,
            double gastHUtcNow,
            SunRightAscensionDeclination sunRightAscensionDeclinationUtcNow,
            DateTime utcNow,
            double precision = 0.00001,
            int maxIterations = 10
        )
        {
            // Get the approximate GAST at the target SunPosition
            double gastHTarget = GetApproximateGastHSolarNoonMidnight(sunPositionTarget, longitude, sunRightAscensionDeclinationUtcNow.RightAscension);

            // Calculate the target Sun states
            double diffGastH = gastHTarget - gastHUtcNow;
            double jd2000UtcTarget = jd2000UtcNow + diffGastH / TimeSpan.FromDays(1).TotalHours;

            // Iterate to find the precise GAST at the target SunPosition
            int numIterations = 0;
            SunLongitudeDistance sunLongitudeDistanceUtcTarget = GetSunLongitudeDistance(jd2000UtcTarget);
            SunRightAscensionDeclination sunRightAscensionDeclinationUtcTarget = GetSunRightAscensionDeclination(jd2000UtcTarget, sunLongitudeDistanceUtcTarget);
            while (Math.Abs(diffGastH) > precision && numIterations < maxIterations)
            {
                sunLongitudeDistanceUtcTarget = GetSunLongitudeDistance(jd2000UtcTarget);
                sunRightAscensionDeclinationUtcTarget = GetSunRightAscensionDeclination(jd2000UtcTarget, sunLongitudeDistanceUtcTarget);
                double gastHTargetNew = GetApproximateGastHSolarNoonMidnight(sunPositionTarget, longitude, sunRightAscensionDeclinationUtcTarget.RightAscension);
                diffGastH = gastHTargetNew - gastHTarget;
                gastHTarget = gastHTargetNew;
                jd2000UtcTarget = jd2000UtcTarget + diffGastH / TimeSpan.FromDays(1).TotalHours;
                numIterations++;
            }
            LHAAltitudeAzimuth lhaAltitudeAzimuthTarget = GetSunLHAAltitudeAzimuth(
                gastHTarget,
                latitude,
                longitude,
                sunRightAscensionDeclinationUtcTarget.RightAscension,
                sunRightAscensionDeclinationUtcTarget.Declination
            );

            return new SunState
            {
                Latitude = latitude,
                Longitude = longitude,
                DateTimeUTC = utcNow.AddDays(jd2000UtcTarget - jd2000UtcNow),
                JD2000Utc = jd2000UtcTarget,
                GastH = gastHTarget,
                Precision = diffGastH,
                SunLongitudeDistance = sunLongitudeDistanceUtcTarget,
                SunRightAscensionDeclination = sunRightAscensionDeclinationUtcTarget,
                LHAAltitudeAzimuth = lhaAltitudeAzimuthTarget,
                SunPosition = sunPositionTarget
            };
        }

        /**
         * Determines the Sun states for any states other than Solar Noon or Solar Midnight at the given target SunPosition.
         **/
        private static SunState GetSunStateSunriseSunset(
            double latitude,
            double longitude,
            double julianDayUtc0h,
            SunLongitudeDistance sunLongitudeDistanceUtcNow,
            SunState sunStateSolarNoon,
            SunState sunStateSolarMidnight,
            SunPosition sunPositionTarget,
            double precision = 0.0001,
            int maxIterations = 10,
            int multiplier = 4
        )
        {
            // Keep track of the UTC date and time of the target altitude
            DateTime dtTarget = sunStateSolarNoon.DateTimeUTC;

            // Calculate the target altitude
            double targetAltitude = GetTargetAltitude(sunPositionTarget, sunLongitudeDistanceUtcNow);

            if (
                targetAltitude < sunStateSolarMidnight.LHAAltitudeAzimuth.Altitude
                || targetAltitude > sunStateSolarNoon.LHAAltitudeAzimuth.Altitude
            )
            {
                return new SunState()
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    DateTimeUTC = dtTarget,
                    JD2000Utc = julianDayUtc0h,
                    GastH = GAST_H(julianDayUtc0h, julianDayUtc0h, dtTarget),
                    Precision = 0,
                    SunLongitudeDistance = sunLongitudeDistanceUtcNow,
                    SunRightAscensionDeclination = GetSunRightAscensionDeclination(julianDayUtc0h, sunLongitudeDistanceUtcNow),
                    LHAAltitudeAzimuth = GetSunLHAAltitudeAzimuth(
                        GAST_H(julianDayUtc0h, julianDayUtc0h, dtTarget),
                        latitude,
                        longitude,
                        GetSunRightAscensionDeclination(julianDayUtc0h, sunLongitudeDistanceUtcNow).RightAscension,
                        GetSunRightAscensionDeclination(julianDayUtc0h, sunLongitudeDistanceUtcNow).Declination
                    ),
                    SunPosition = sunPositionTarget,
                    IsInvalid = true
                };

            }

            // Calculate the hour angle to approximate the Sun's at the target altitude
            double targetHourAngle = RadiansToDegrees(
                Math.Acos(
                    (
                        Math.Sin(DegreesToRadians(targetAltitude)) - Math.Sin(DegreesToRadians(latitude)) * Math.Sin(DegreesToRadians(sunStateSolarNoon.SunRightAscensionDeclination.Declination))
                    ) /
                    (
                        Math.Cos(DegreesToRadians(latitude)) * Math.Cos(DegreesToRadians(sunStateSolarNoon.SunRightAscensionDeclination.Declination))
                    )
                )
            );

            // Determine where to start the search for the target hour angle
            double jDateStart = sunStateSolarNoon.JD2000Utc;

            // If the target hour angle is infinite or NaN, then set it to a small value
            bool isSearchingFromMidnight = false;
            if (double.IsInfinity(targetHourAngle) || double.IsNaN(targetHourAngle))
            {
                targetHourAngle = 0.1;

                double diffSolarMidnight = Math.Abs(targetAltitude - sunStateSolarMidnight.LHAAltitudeAzimuth.Altitude);
                double diffSolarNoon = Math.Abs(targetAltitude - sunStateSolarNoon.LHAAltitudeAzimuth.Altitude);

                // If the target altitude is closer to the Sun's position at midnight, then start the search at midnight
                if (diffSolarMidnight < diffSolarNoon)
                {
                    jDateStart = sunStateSolarMidnight.JD2000Utc;
                    dtTarget = sunStateSolarMidnight.DateTimeUTC;
                    isSearchingFromMidnight = true;
                }
            }

            // Calculate the approximate Julian Day for the target altitude
            double jDateDiff = targetHourAngle / 360.0;
            if (isSearchingFromMidnight)
            {
                jDateDiff = -jDateDiff;
            }
            double jd2000UtcTarget = 0.0;
            switch (sunPositionTarget)
            {
                case SunPosition.Sunrise:
                    jd2000UtcTarget = jDateStart - jDateDiff;
                    break;
                case SunPosition.Sunset:
                    jd2000UtcTarget = jDateStart + jDateDiff;
                    break;
                case SunPosition.CivilDawn:
                    jd2000UtcTarget = jDateStart - jDateDiff;
                    break;
                case SunPosition.CivilDusk:
                    jd2000UtcTarget = jDateStart + jDateDiff;
                    break;
                case SunPosition.NauticalDawn:
                    jd2000UtcTarget = jDateStart - jDateDiff;
                    break;
                case SunPosition.NauticalDusk:
                    jd2000UtcTarget = jDateStart + jDateDiff;
                    break;
                case SunPosition.AstronomicalDawn:
                    jd2000UtcTarget = jDateStart - jDateDiff;
                    break;
                case SunPosition.AstronomicalDusk:
                    jd2000UtcTarget = jDateStart + jDateDiff;
                    break;
                default:
                    throw new ArgumentException("Invalid SunPosition value");
            }
            double julianDayUtcTarget = jd2000UtcTarget + JD2000_EPOCH;
            double julianDayTTTarget = JulianDayTT_from_GCT(JulianDayUT_to_JulianDayGCT(julianDayUtcTarget));
            dtTarget = dtTarget.AddDays(jd2000UtcTarget - jDateStart);

            // Calculate the approximate GAST for the target altitude
            double julianDayCheck0h = julianDayUtc0h;
            if (julianDayTTTarget < julianDayCheck0h)
            {
                julianDayCheck0h -= 1.0;
            }
            else if (julianDayTTTarget >= julianDayCheck0h + 1.0)
            {
                julianDayCheck0h += 1.0;
            }
            double gastHTarget = GAST_H(julianDayTTTarget, julianDayCheck0h, dtTarget);

            // Calculate the Sun's approximate position at the target altitude
            SunLongitudeDistance sunLongitudeDistanceUtcTarget = GetSunLongitudeDistance(jd2000UtcTarget);
            SunRightAscensionDeclination sunRightAscensionDeclinationUtcTarget = GetSunRightAscensionDeclination(jd2000UtcTarget, sunLongitudeDistanceUtcTarget);
            LHAAltitudeAzimuth lhaAltitudeAzimuthTarget = GetSunLHAAltitudeAzimuth(
                gastHTarget,
                latitude,
                longitude,
                sunRightAscensionDeclinationUtcTarget.RightAscension,
                sunRightAscensionDeclinationUtcTarget.Declination
            );

            // Get the altitude change rate per GAST degree (derivative of the altitude function)
            double altitudeRatePerGastHDenominator = Math.Sqrt(
                1 -
                Math.Pow(
                    Math.Cos(DegreesToRadians(sunRightAscensionDeclinationUtcTarget.Declination)) *
                    Math.Cos(DegreesToRadians(latitude)) *
                    Math.Cos(DegreesToRadians(lhaAltitudeAzimuthTarget.LHA)) +
                    Math.Sin(DegreesToRadians(sunRightAscensionDeclinationUtcTarget.Declination)) *
                    Math.Sin(DegreesToRadians(latitude)),
                    2
                )
            );
            double altitudeRatePerGastH = 0.0;
            if (altitudeRatePerGastHDenominator != 0.0)
            {
                altitudeRatePerGastH = RadiansToDegrees(
                    -(
                        (
                            Math.Cos(DegreesToRadians(sunRightAscensionDeclinationUtcTarget.Declination)) *
                            Math.Cos(DegreesToRadians(latitude)) *
                            Math.Sin(DegreesToRadians(lhaAltitudeAzimuthTarget.LHA))
                        ) /
                        altitudeRatePerGastHDenominator
                    )
                );
            }
            else
            {
                altitudeRatePerGastH = 90.0;
            }

            // Iterate to find the Sun's position for the target altitude
            double diffAltitude = targetAltitude - lhaAltitudeAzimuthTarget.Altitude;
            int numIterations = 0;
            int localMultiplier = multiplier;
            double diffGastH = 0.0;
            while (Math.Abs(diffAltitude) > precision && numIterations < maxIterations)
            {
                // Get the amount of GAST to change to reach the target altitude
                diffGastH = diffAltitude / altitudeRatePerGastH * localMultiplier;
                gastHTarget = gastHTarget + diffGastH;

                // Get the new J2000 UTC and the Sun's position at the new GAST
                jd2000UtcTarget = jd2000UtcTarget + diffGastH / TimeSpan.FromDays(1).TotalHours;
                sunLongitudeDistanceUtcTarget = GetSunLongitudeDistance(jd2000UtcTarget);
                sunRightAscensionDeclinationUtcTarget = GetSunRightAscensionDeclination(jd2000UtcTarget, sunLongitudeDistanceUtcTarget);
                lhaAltitudeAzimuthTarget = GetSunLHAAltitudeAzimuth(
                    gastHTarget,
                    latitude,
                    longitude,
                    sunRightAscensionDeclinationUtcTarget.RightAscension,
                    sunRightAscensionDeclinationUtcTarget.Declination
                );

                // Update the UTC date and time based on the new GAST
                dtTarget = dtTarget.AddHours(diffGastH);

                // If the new altitude is further from the target, reduce the multiplier
                if (Math.Abs(targetAltitude - lhaAltitudeAzimuthTarget.Altitude) > Math.Abs(diffAltitude))
                {
                    gastHTarget = gastHTarget - diffGastH;
                    jd2000UtcTarget = jd2000UtcTarget - diffGastH / TimeSpan.FromDays(1).TotalHours;
                    sunLongitudeDistanceUtcTarget = GetSunLongitudeDistance(jd2000UtcTarget);
                    sunRightAscensionDeclinationUtcTarget = GetSunRightAscensionDeclination(jd2000UtcTarget, sunLongitudeDistanceUtcTarget);
                    lhaAltitudeAzimuthTarget = GetSunLHAAltitudeAzimuth(
                        gastHTarget,
                        latitude,
                        longitude,
                        sunRightAscensionDeclinationUtcTarget.RightAscension,
                        sunRightAscensionDeclinationUtcTarget.Declination
                    );
                    dtTarget = dtTarget.AddHours(-diffGastH);
                    numIterations++;
                    if (localMultiplier > 1)
                    {
                        localMultiplier -= 1;
                    }
                    else
                    {
                        altitudeRatePerGastH *= 2.0;
                    }
                    continue;
                }

                // Get the target altitude at the new solar longitude and distance.
                targetAltitude = GetTargetAltitude(sunPositionTarget, sunLongitudeDistanceUtcTarget);

                // Get the difference in altitude vs. the target
                diffAltitude = targetAltitude - lhaAltitudeAzimuthTarget.Altitude;

                // Get the altitude change rate per GAST degree (derivative of the altitude function)
                altitudeRatePerGastHDenominator = Math.Sqrt(
                    1 -
                    Math.Pow(
                        Math.Cos(DegreesToRadians(sunRightAscensionDeclinationUtcTarget.Declination)) *
                        Math.Cos(DegreesToRadians(latitude)) *
                        Math.Cos(DegreesToRadians(lhaAltitudeAzimuthTarget.LHA)) +
                        Math.Sin(DegreesToRadians(sunRightAscensionDeclinationUtcTarget.Declination)) *
                        Math.Sin(DegreesToRadians(latitude)),
                        2
                    )
                );
                altitudeRatePerGastH = 0.0;
                if (altitudeRatePerGastHDenominator != 0.0)
                {
                    altitudeRatePerGastH = RadiansToDegrees(
                        -(
                            (
                                Math.Cos(DegreesToRadians(sunRightAscensionDeclinationUtcTarget.Declination)) *
                                Math.Cos(DegreesToRadians(latitude)) *
                                Math.Sin(DegreesToRadians(lhaAltitudeAzimuthTarget.LHA))
                            ) /
                            altitudeRatePerGastHDenominator
                        )
                    );
                }
                else
                {
                    altitudeRatePerGastH = 90.0;
                }

                numIterations++;
            }

            return new SunState()
            {
                Latitude = latitude,
                Longitude = longitude,
                DateTimeUTC = dtTarget,
                JD2000Utc = jd2000UtcTarget,
                GastH = gastHTarget,
                Precision = diffGastH,
                SunLongitudeDistance = sunLongitudeDistanceUtcTarget,
                SunRightAscensionDeclination = sunRightAscensionDeclinationUtcTarget,
                LHAAltitudeAzimuth = lhaAltitudeAzimuthTarget,
                SunPosition = sunPositionTarget
            };
        }
    }
}
