using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SunRiseSet
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private double _latitude = 0.0;
        private double _longitude = 0.0;
        private double _declinationUtcNow = 0.0;
        private double _rightAscensionUtcNow = 0.0;
        private double _solarDistanceUtcNow = 0.0;
        private double _solarLongitudeUtcNow = 0.0;
        private double _gmstHUtcNow = 0.0;
        private double _gmstUtcNow = 0.0;
        private double _gastHUtcNow = 0.0;
        private double _gastUtcNow = 0.0;
        private double _earthRotationAngleUtcNow = 0.0;
        private double _lhaNow = 0.0;
        private double _altitudeNow = 0.0;
        private double _azimuthNow = 0.0;
        private SunStates _sunStates;
        private object _lock = new object();
        private bool running = true;
        Thread updater;

        public MainWindow()
        {
            this.InitializeComponent();

            latitude.Text = ConfigurationManager.AppSettings["latitude"];
            double lat;
            if (double.TryParse(latitude.Text, out lat) && lat >= -90.0 && lat <= 90.0)
            {
                lock (_lock)
                {
                    _latitude = lat;
                }
                latitude.Background = null;
            }
            else
            {
                latitude.Background = (SolidColorBrush)Application.Current.Resources["ErrorBackground"];
            }

            longitude.Text = ConfigurationManager.AppSettings["longitude"];
            double lon;
            if (double.TryParse(longitude.Text, out lon) && lon >= -180.0 && lon <= 180.0)
            {
                lock (_lock)
                {
                    _longitude = lon;
                }
                longitude.Background = null;
            }
            else
            {
                longitude.Background = (SolidColorBrush)Application.Current.Resources["ErrorBackground"];
            }

            calculateAndUpdateSunPosition();

            _lock = new object();
            updater = new Thread(updaterThread);
            updater.IsBackground = true;
            updater.Start();
        }

        private void latitude_TextChanged(object sender, TextChangedEventArgs e)
        {
            double lat;
            if( double.TryParse( ((TextBox)sender).Text, out lat ) && lat >= -90.0 && lat <= 90.0 )
            {
                lock( _lock )
                {
                    _latitude = lat;
                }
                ((TextBox)sender).Background = null;
                calculateAndUpdateSunPosition();
            }
            else
            {
                ((TextBox)sender).Background = (SolidColorBrush)Application.Current.Resources["ErrorBackground"];
            }
        }

        private void longitude_TextChanged(object sender, TextChangedEventArgs e)
        {
            double lon;
            if( double.TryParse( ((TextBox)sender).Text, out lon ) && lon >= -180.0 && lon <= 180.0 )
            {
                lock( _lock )
                {
                    _longitude = lon;
                }
                ((TextBox)sender).Background = null;
                calculateAndUpdateSunPosition();
            }
            else
            {
                ((TextBox)sender).Background = (SolidColorBrush)Application.Current.Resources["ErrorBackground"];
            }
        }

        void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            sender.ClearColor = ((SolidColorBrush)Application.Current.Resources["CanvasBackground"]).Color;

            double declination;
            lock( _lock )
            {
                declination = _declinationUtcNow;
            }
            args.DrawingSession.DrawEllipse(155, 115, 80, 30, ((SolidColorBrush)Application.Current.Resources["CanvasElipseOutline"]).Color, 3);
            args.DrawingSession.DrawText(declination.ToString("F10"), 100, 100, ((SolidColorBrush)Application.Current.Resources["CanvasTextColor"]).Color);
            sender.Invalidate();
        }

        void calculateAndUpdateSunPosition()
        {
            double latitude;
            double longitude;

            lock( _lock )
            {
                latitude = _latitude;
                longitude = _longitude;
            }

            calculateAndUpdateSunPosition(latitude, longitude);
        }

        void calculateAndUpdateSunPosition(double latitude, double longitude)
        {
            // Get the current time
            //DateTime nowLocal = new DateTime(2024, 3, 20, 3, 7, 0, DateTimeKind.Utc);
            //DateTime nowLocal = new DateTime(2024, 6, 20, 20, 51, 0, DateTimeKind.Utc);
            //DateTime nowLocal = new DateTime(2024, 9, 22, 12, 44, 0, DateTimeKind.Utc);
            //DateTime nowLocal = new DateTime(2024, 12, 21, 9, 20, 0, DateTimeKind.Utc);
            //DateTime nowLocal = new DateTime(2017, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //DateTime nowLocal = new DateTime(2024, 3, 9, 23, 59, 59, 999, DateTimeKind.Utc);
            //DateTime nowLocal = new DateTime(2024, 3, 10, 20, 18, 49, 51, DateTimeKind.Utc); // Solar noon of 03-10-2024
            DateTime nowLocal = DateTime.Now;
            DateTime nowUtc = nowLocal.ToUniversalTime();

            // Calculate the total number of days (including fractional days) since J2000 for the current UTC time
            double julianDayUtc0h = SunCalc.JulianDay(nowUtc);
            double jd2000UtcNow = SunCalc.JulianDay_to_JD2000(SunCalc.JulianDay_AddHours(julianDayUtc0h, nowUtc));
            double julianDayTT = SunCalc.JulianDayTT_with_Hours(nowUtc);

            // Compute local sideral time of this moment
            double gmstHUtcNow = SunCalc.GMST_H(julianDayTT, julianDayUtc0h, nowUtc);
            double gmstUtcNow = SunCalc.HoursToSeconds(gmstHUtcNow);
            double gastHUtcNow = SunCalc.GAST_H(julianDayTT, julianDayUtc0h, nowUtc);
            double gastUtcNow = SunCalc.HoursToSeconds(gastHUtcNow);

            double earthRotationAngleUtcNow = SunCalc.ClampAngle360(SunCalc.ERA(jd2000UtcNow));

            // Compute the Sun's ecliptic longitude and distance
            SunLongitudeDistance sunLongitudeDistanceUtcNow = SunCalc.GetSunLongitudeDistance(jd2000UtcNow);

            // Compute the Sun's right ascension and declination
            SunRightAscensionDeclination sunRightAscensionDeclinationUtcNow = SunCalc.GetSunRightAscensionDeclination(jd2000UtcNow, sunLongitudeDistanceUtcNow);

            // Compute the Sun's local hour angle, altitude, and azimuth
            LHAAltitudeAzimuth lhaAltitudeAzimuth = SunCalc.GetSunLHAAltitudeAzimuth(
                gastHUtcNow,
                latitude,
                longitude,
                sunRightAscensionDeclinationUtcNow.RightAscension,
                sunRightAscensionDeclinationUtcNow.Declination
            );

            // Compute the Sun's state at various times of the day
            SunStates sunStates = SunCalc.GetSunStates(
                latitude,
                longitude,
                julianDayUtc0h,
                jd2000UtcNow,
                gastHUtcNow,
                nowUtc,
                sunRightAscensionDeclinationUtcNow,
                sunLongitudeDistanceUtcNow
            );

            // Update the values
            lock (_lock)
            {
                _declinationUtcNow = sunRightAscensionDeclinationUtcNow.Declination;
                _rightAscensionUtcNow = sunRightAscensionDeclinationUtcNow.RightAscension;
                _solarDistanceUtcNow = sunLongitudeDistanceUtcNow.Distance;
                _solarLongitudeUtcNow = sunLongitudeDistanceUtcNow.Longitude;
                _gmstHUtcNow = gmstHUtcNow;
                _gmstUtcNow = gmstUtcNow;
                _gastHUtcNow = gastHUtcNow;
                _gastUtcNow = gastUtcNow;
                _earthRotationAngleUtcNow = earthRotationAngleUtcNow;
                _lhaNow = lhaAltitudeAzimuth.LHA;
                _altitudeNow = lhaAltitudeAzimuth.Altitude;
                _azimuthNow = lhaAltitudeAzimuth.Azimuth;
                _sunStates = sunStates;
            }

            // Update the UI
            DispatcherQueue.TryEnqueue(updateUI);
        }

        void updateUI()
        {
            double declinationUtcNow;
            double rightAscensionUtcNow;
            double solarDistanceUtcNow;
            double solarLongitudeUtcNow;
            double gmstUtcNow;
            double gastUtcNow;
            double earthRotationAngleUtcNow;
            double lhaNow;
            double altitudeNow;
            double azimuthNow;
            lock (_lock)
            {
                declinationUtcNow = _declinationUtcNow;
                rightAscensionUtcNow = _rightAscensionUtcNow;
                solarDistanceUtcNow = _solarDistanceUtcNow;
                solarLongitudeUtcNow = _solarLongitudeUtcNow;
                gmstUtcNow = _gmstUtcNow;
                gastUtcNow = _gastUtcNow;
                earthRotationAngleUtcNow = _earthRotationAngleUtcNow;
                lhaNow = _lhaNow;
                altitudeNow = _altitudeNow;
                azimuthNow = _azimuthNow;
            }

            HoursMinutesSeconds gmstHMS = getGMST_HMS(gmstUtcNow);
            HoursMinutesSeconds gastHMS = getGMST_HMS(gastUtcNow);

            declinationAngleValue.Text = declinationUtcNow.ToString("F10");
            rightAscensionValue.Text = rightAscensionUtcNow.ToString("F10");
            solarDistanceValue.Text = solarDistanceUtcNow.ToString("F10");
            solarLongitudeValue.Text = solarLongitudeUtcNow.ToString("F10");
            gmstValue.Text =
                string.Format("{0:D2}", gmstHMS.Hours) + ":" +
                string.Format("{0:D2}", gmstHMS.Minutes) + ":" +
                string.Format("{0:D2}", gmstHMS.Seconds) + "." +
                string.Format("{0:D3}", gmstHMS.Milliseconds);
            earthRotationAngleValue.Text = earthRotationAngleUtcNow.ToString("F10");
            gastValue.Text =
                string.Format("{0:D2}", gastHMS.Hours) + ":" +
                string.Format("{0:D2}", gastHMS.Minutes) + ":" +
                string.Format("{0:D2}", gastHMS.Seconds) + "." +
                string.Format("{0:D3}", gastHMS.Milliseconds);
            lhaValue.Text = lhaNow.ToString("F10");
            altitudeValue.Text = altitudeNow.ToString("F10");
            azimuthValue.Text = azimuthNow.ToString("F10");

            var tz = DateTimeZoneProviders.Tzdb.GetSystemDefault(); // Get the system's time zone
            sunriseValue.Text = new ZonedDateTime(Instant.FromDateTimeUtc(_sunStates.Sunrise.DateTimeUTC), tz).ToString("h':'mm':'ss' 'tt' 'x", CultureInfo.InvariantCulture);
            if (_sunStates.Sunrise.IsInvalid)
            {
                sunriseLabel.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
                sunriseValue.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
            }
            else
            {
                sunriseLabel.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
                sunriseValue.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
            }

            sunsetValue.Text = new ZonedDateTime(Instant.FromDateTimeUtc(_sunStates.Sunset.DateTimeUTC), tz).ToString("h':'mm':'ss' 'tt' 'x", CultureInfo.InvariantCulture);
            if (_sunStates.Sunset.IsInvalid)
            {
                sunsetLabel.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
                sunsetValue.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
            }
            else
            {
                sunsetLabel.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
                sunsetValue.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
            }

            solarNoonValue.Text = new ZonedDateTime(Instant.FromDateTimeUtc(_sunStates.SolarNoon.DateTimeUTC), tz).ToString("h':'mm':'ss' 'tt' 'x", CultureInfo.InvariantCulture);
            solarMidnightValue.Text = new ZonedDateTime(Instant.FromDateTimeUtc(_sunStates.SolarMidnight.DateTimeUTC), tz).ToString("h':'mm':'ss' 'tt' 'x", CultureInfo.InvariantCulture);
            civilDawnValue.Text = new ZonedDateTime(Instant.FromDateTimeUtc(_sunStates.CivilDawn.DateTimeUTC), tz).ToString("h':'mm':'ss' 'tt' 'x", CultureInfo.InvariantCulture);
            if (_sunStates.CivilDawn.IsInvalid)
            {
                civilDawnLabel.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
                civilDawnValue.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
            }
            else
            {
                civilDawnLabel.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
                civilDawnValue.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
            }

            civilDuskValue.Text = new ZonedDateTime(Instant.FromDateTimeUtc(_sunStates.CivilDusk.DateTimeUTC), tz).ToString("h':'mm':'ss' 'tt' 'x", CultureInfo.InvariantCulture);
            if (_sunStates.CivilDusk.IsInvalid)
            {
                civilDuskLabel.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
                civilDuskValue.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
            }
            else
            {
                civilDuskLabel.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
                civilDuskValue.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
            }

            nauticalDawnValue.Text = new ZonedDateTime(Instant.FromDateTimeUtc(_sunStates.NauticalDawn.DateTimeUTC), tz).ToString("h':'mm':'ss' 'tt' 'x", CultureInfo.InvariantCulture);
            if (_sunStates.NauticalDawn.IsInvalid)
            {
                nauticalDawnLabel.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
                nauticalDawnValue.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
            }
            else
            {
                nauticalDawnLabel.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
                nauticalDawnValue.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
            }

            nauticalDuskValue.Text = new ZonedDateTime(Instant.FromDateTimeUtc(_sunStates.NauticalDusk.DateTimeUTC), tz).ToString("h':'mm':'ss' 'tt' 'x", CultureInfo.InvariantCulture);
            if (_sunStates.NauticalDusk.IsInvalid)
            {
                nauticalDuskLabel.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
                nauticalDuskValue.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
            }
            else
            {
                nauticalDuskLabel.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
                nauticalDuskValue.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
            }   

            astronomicalDawnValue.Text = new ZonedDateTime(Instant.FromDateTimeUtc(_sunStates.AstronomicalDawn.DateTimeUTC), tz).ToString("h':'mm':'ss' 'tt' 'x", CultureInfo.InvariantCulture);
            if (_sunStates.AstronomicalDawn.IsInvalid)
            {
                astronomicalDawnLabel.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
                astronomicalDawnValue.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
            }
            else
            {
                astronomicalDawnLabel.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
                astronomicalDawnValue.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
            }

            astronomicalDuskValue.Text = new ZonedDateTime(Instant.FromDateTimeUtc(_sunStates.AstronomicalDusk.DateTimeUTC), tz).ToString("h':'mm':'ss' 'tt' 'x", CultureInfo.InvariantCulture);
            if (_sunStates.AstronomicalDusk.IsInvalid)
            {
                astronomicalDuskLabel.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
                astronomicalDuskValue.Foreground = (SolidColorBrush)Application.Current.Resources["ErrorText"];
            }
            else
            {
                astronomicalDuskLabel.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
                astronomicalDuskValue.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultText"];
            }
        }

        HoursMinutesSeconds getGMST_HMS(double gmstValue)
        {
            HoursMinutesSeconds hoursMinutesSeconds = new HoursMinutesSeconds();

            double remainder = gmstValue % TimeSpan.FromDays(1.0).TotalSeconds;

            hoursMinutesSeconds.Hours = (int)(remainder / TimeSpan.FromHours(1.0).TotalSeconds);
            remainder = remainder % TimeSpan.FromHours(1.0).TotalSeconds;
            hoursMinutesSeconds.Minutes = (int)(remainder / TimeSpan.FromMinutes(1.0).TotalSeconds);
            remainder = remainder % TimeSpan.FromMinutes(1.0).TotalSeconds;
            hoursMinutesSeconds.Seconds = (int)(remainder / TimeSpan.FromSeconds(1.0).TotalSeconds);
            remainder = remainder % TimeSpan.FromSeconds(1.0).TotalSeconds;
            hoursMinutesSeconds.Milliseconds = (int)(remainder * TimeSpan.FromSeconds(1.0).TotalMilliseconds);

            return hoursMinutesSeconds;
        }

        bool isRunning()
        {
            lock( _lock )
            {
                return running;
            }
        }

        void updaterThread()
        {
            while( isRunning() )
            {
                calculateAndUpdateSunPosition();
                Thread.Sleep(10);
            }
        }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            running = false;
            updater.Join();
        }
    }
}
