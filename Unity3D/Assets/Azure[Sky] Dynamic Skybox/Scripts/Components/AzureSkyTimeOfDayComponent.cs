using System;

namespace UnityEngine.AzureSky
{
    [Serializable]
    public class AzureSkyTimeOfDayComponent
    {
        //Used by Calendar.
        public int selectableDayInt = 15;
        public int dayOfYear = 1;
        public string[] selectableDayList = new string[]
        {
            "0", "1", "2", "3", "4" , "5" , "6", "7" , "8", "9", "10",
            "11", "12", "13", "14" , "15" , "16", "17" , "18", "19", "20",
            "21", "22", "23", "24" , "25" , "26", "27" , "28", "29", "30",
            "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41"
        };

        //Used by Calendar and Time of Day tab.
        public float hour = 6.0f;
        public int day = 15;
        public int month = 9;
        public int year = 1903;
        public int dayInMonth = 30;
        public float utc = 0;
        public float latitude = 0;
        public float longitude = 0;

        //Used by Time of Day tab.
        public int timeMode = 0;
        public float dayCycle = 10.0f;//In minutes
        public bool setTimeByCurve = false;
        public float hourByCurve = 6.0f;
        public AnimationCurve dayCycleCurve = AnimationCurve.Linear(0, 0, 24, 24);
        public float lst;
        private float m_radians, m_radLatitude, m_sinLatitude, m_cosLatitude;

        //Internal uses.
        private Vector2 m_hourAndMinutes;
        private Vector3 m_date;
        private DateTime m_dateTime;
        private int m_daysInMonth = 30;
        private int m_dayOfWeek = 0;
        private int m_fixLeapYear = 0;

        /// <summary>
		/// Used by AzureSkyController and AzureSkyControllerEditor scripts to correctly adjusts the day buttons and day profiles on the calendar.
        /// </summary>
        /// <param name="dayList">List containing 42 strings with the day numbers to rename the calendar buttons.</param>
        public void UpdateCalendar (string[] dayList)
        {
            m_daysInMonth = DateTime.DaysInMonth(year, month);
            if (day > m_daysInMonth) { day = m_daysInMonth; }
            if (day < 1) { day = 1; }

            m_dateTime = new DateTime(year, month, 1);
            m_dayOfWeek = (int)m_dateTime.DayOfWeek;
            selectableDayInt = day - 1 + m_dayOfWeek;
            
            m_dateTime = new DateTime(year, month, day);
            for (int i = 0; i < dayList.Length; i++)
            {
                if (i < m_dayOfWeek || i >= (m_dayOfWeek + m_daysInMonth))
                {
                    dayList[i] = "";
                    continue;
                }
                m_dateTime = new DateTime(year, month, (i - m_dayOfWeek) + 1);
                dayList[i] = m_dateTime.Day.ToString();
            }
        }

		/// <summary>
		/// Get the current day of the week and return an integer between 0 and 6.
		/// </summary>
		/// <returns></returns>
		public int GetDayOfWeek ()
		{
			m_dateTime = new DateTime(year, month, day);
			return (int)m_dateTime.DayOfWeek;
		}

        /// <summary>
		/// Used by AzureSkyControllerEditor script to get the day of the week from a specific day of the month. Returns an integer between 0 and 6.
        /// </summary>
        /// <param name="dayNumber">Number of the day in the month.</param>
        /// <returns></returns>
        public int GetDayOfWeek (int dayNumber)
        {
            m_dateTime = new DateTime(year, month, dayNumber);
            return (int)m_dateTime.DayOfWeek;
        }

        /// <summary>
        /// Returns the current day number of a 366-day year.
        /// The return value is fix when it is not leap year, this method is ideal for accessing, adding, or removing day profiles from the calendar.
        /// If you want to get the correct number for the day in the year, then use System.DateTime.DayOfYear instead.
        /// </summary>
        /// <returns></returns>
        public int GetDayOfYear ()
        {
            m_dateTime = new DateTime(year, month, day);
            dayOfYear = m_dateTime.DayOfYear;
            if ((dayOfYear > 59 && !IsLeapYear()))
            {
                m_fixLeapYear = 1;
            }
            else { m_fixLeapYear = 0; }
            return m_dateTime.DayOfYear - 1 + m_fixLeapYear;
        }

        /// <summary>
        /// Return true if the current year is a leap year.
        /// </summary>
        /// <returns></returns>
        public bool IsLeapYear ()
        {
            return DateTime.IsLeapYear(year);
        }

        /// <summary>
		/// Converts the timeline in hours and minutes and returns as a Vector2(hours, minutes).
        /// </summary>
        /// <returns></returns>
        public Vector2 GetTime ()
        {
            if (setTimeByCurve)
            {
                m_hourAndMinutes.x = Mathf.Floor(hourByCurve);
                m_hourAndMinutes.y = 60.0f * (hourByCurve - m_hourAndMinutes.x);
                m_hourAndMinutes.y = Mathf.Floor(m_hourAndMinutes.y);
            }
            else
                {
                    m_hourAndMinutes.x = Mathf.Floor(hour);
                    m_hourAndMinutes.y = 60.0f * (hour - m_hourAndMinutes.x);
                    m_hourAndMinutes.y = Mathf.Floor(m_hourAndMinutes.y);
                }
            return m_hourAndMinutes;
        }

        /// <summary>
        /// Get the current date and returns as a Vector3(month, day, year)
        /// </summary>
        /// <returns></returns>
        public Vector3 GetDate ()
        {
            m_date.x = month;
            m_date.y = day;
            m_date.z = year;
            return m_date;
        }

        /// <summary>
        /// Get the current system time and returns as a Float.
        /// </summary>
        /// <returns></returns>
        public float GetSystemTime ()
        {
            return DateTime.Now.Hour + ((1.0f / 60.0f) * DateTime.Now.Minute);
        }

        /// <summary>
		/// Used by AzureSkyController script to convert current system time into a float and apply to timeline.
        /// </summary>
        public void ApplySystemTime ()
        {
            hour = DateTime.Now.Hour + ((1.0f / 60.0f) * DateTime.Now.Minute);
        }

        /// <summary>
		/// Used by AzureSkyController script to apply the current system date.
        /// </summary>
        public void ApplySystemDate ()
        {
            month = DateTime.Now.Month;
            day   = DateTime.Now.Day;
            year  = DateTime.Now.Year;
        }

        /// <summary>
		/// Starts the next day and resets the time based on the Repeat Mode set in the Inspector. This method is commonly called at midnight by AzureSkyController script.
        /// </summary>
        /// <param name="repeatMode">Place here the Repeat Mode setted on the Options tab.</param>
        public void StartNextDay (int repeatMode)
        {
            if(repeatMode != 1)day += 1;
            hour = 0;
            if(day > m_daysInMonth)
            {
                day = 1;
                if (repeatMode != 2)
                {
                    month += 1;
                    if (month > 12)
                    {
                        month = 1;
                        if (repeatMode != 3)
                        {
                            year += 1;
                            if (year > 9999)
                            {
                                year = 1;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
		/// Skips to the next day and ignores the Repeat Mode set in the Inspector.
        /// </summary>
		/// <param name="keepTime">Keep the current time?</param>
		public void JumpToNextDay ( bool keepTime = true)
        {
            day += 1;
			if (!keepTime)
				hour = 0;
            if (day > m_daysInMonth)
            {
                day = 1;
                month += 1;
                if (month > 12)
                {
                    month = 1;
                    year += 1;
                    if (year > 9999)
                    {
                        year = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Go to a custom date.
        /// </summary>
        /// <param name="month">The number of the month you want to go.</param>
        /// <param name="day">The number of the day you want to go.</param>
        /// <param name="year">The number of the year you want to go.</param>
        public void GotoDate (int month, int day, int year)
        {
            this.month = month;
            this.day = day;
            this.year = year;
        }

        /// <summary>
        /// Go to a custom time.
        /// </summary>
        /// <param name="hours">The number of the hour you want to go.</param>
        /// <param name="minutes">The number of the minute you want to go.</param>
        public void GotoTime (int hours, int minutes)
        {
            this.hour = hours + ((1.0f / 60.0f) * minutes);
        }

        /// <summary>
        /// Used by AzureSkyController script to apply the time progression.
        /// </summary>
        /// <returns></returns>
        public float GetDayLength ()
        {
            if (dayCycle > 0.0f)
                return (24.0f / 60.0f) / dayCycle;
            else
                return 0.0f;
        }

		/// <summary>
		/// Used by AzureSkyController script to calculate the time of day based on the curve time.
		/// </summary>
        public void CalculateTimeByCurve ()
        {
            hourByCurve = dayCycleCurve.Evaluate(hour);
        }

		/// <summary>
		/// Used by AzureSkyController script to set the simple sun position.
		/// </summary>
		/// <returns></returns>
		public float SetSimpleSunPosition ()
		{
			if (setTimeByCurve)
			{
				return ((hourByCurve + utc) * 360.0f / 24.0f) - 90.0f;
			}
			else
			{
				return ((hour + utc) * 360.0f / 24.0f) - 90.0f;
			}
		}

		/// <summary>
		/// Used by AzureSkyController script to set the simple moon position.
		/// </summary>
		/// <returns></returns>
		public Quaternion SetSimpleMoonPosition ( Transform sun)
		{
			return sun.transform.rotation * Quaternion.Euler(0, -180, 0);
		}

        /// <summary>
		/// Used by AzureSkyController script to set the realistic sun position based on Time, Date and Location.
        /// </summary>
        /// <returns></returns>
        public Vector3 SetRealisticSunPosition ()
        {
            m_radians = (Mathf.PI * 2.0f) / 360.0f;//Used to convert degress to radians.
            m_radLatitude = m_radians * latitude;
            m_sinLatitude = Mathf.Sin(m_radLatitude);
            m_cosLatitude = Mathf.Cos(m_radLatitude);

            float hour = this.hour - utc;
            //Time Scale.
            //---------------------------------------------------------------------------------------------------
            //d = 367*y - 7 * ( y + (m+9)/12 ) / 4 + 275*m/9 + D - 730530
            //d = d + UT/24.0
            float d = 367 * year - 7 * (year + (month + 9) / 12) / 4 + 275 * month / 9 + day - 730530;
            d = d + hour / 24.0f;

            //Tilt of earth's axis.
            //---------------------------------------------------------------------------------------------------
            //obliquity of the ecliptic.
            float ecliptic = 23.4393f - 3.563E-7f * d;
            //Need convert to radians before apply sine and cosine.
            float radEcliptic = m_radians * ecliptic;
            float sinEcliptic = Mathf.Sin(radEcliptic);
            float cosEcliptic = Mathf.Cos(radEcliptic);

            //Orbital elements of the Sun.
            //---------------------------------------------------------------------------------------------------
            //float N = 0.0;
            //float i = 0.0;
            float w = 282.9404f + 4.70935E-5f * d;
            //float a = 1.000000f;
            float e = 0.016709f - 1.151E-9f * d;
            float M = 356.0470f + 0.9856002585f * d;

            //Eccentric anomaly.
            //---------------------------------------------------------------------------------------------------
            //E = M + e*(180/pi) * sin(M) * ( 1.0 + e * cos(M) ) in degress.
            //E = M + e * sin(M) * ( 1.0 + e * cos(M) ) in radians.
            //Need convert to radians before apply sine and cosine.
            float radM = m_radians * M;
            float sinM = Mathf.Sin(radM);
            float cosM = Mathf.Cos(radM);

            //Need convert to radians before apply sine and cosine.
            float radE = radM + e * sinM * (1.0f + e * cosM);
            float sinE = Mathf.Sin(radE);
            float cosE = Mathf.Cos(radE);

            //Sun's distance (r) and its true anomaly (v).
            //---------------------------------------------------------------------------------------------------
            //Xv = r * cos (v) = cos (E) - e
            //Yv = r * sen (v) = sqrt (1,0 - e * e) * sen (E)
            float xv = cosE - e;
            float yv = Mathf.Sqrt(1.0f - e * e) * sinE;

            //V = atan2 (yv, xv)
            //R = sqrt (xv * xv + yv * yv)
            float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);
            float r = Mathf.Sqrt(xv * xv + yv * yv);

            //Sun's true longitude.
            //---------------------------------------------------------------------------------------------------
            float radLongitude = m_radians * (v + w);
            float sinLongitude = Mathf.Sin(radLongitude);
            float cosLongitude = Mathf.Cos(radLongitude);

            float xs = r * cosLongitude;
            float ys = r * sinLongitude;

            //Equatorial coordinates.
            //---------------------------------------------------------------------------------------------------
            float xe = xs;
            float ye = ys * cosEcliptic;
            float ze = ys * sinEcliptic;

            //Sun's Right Ascension(RA) and Declination(Dec).
            //---------------------------------------------------------------------------------------------------
            float RA = Mathf.Atan2(ye, xe);
            float Dec = Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));
            float sinDec = Mathf.Sin(Dec);
            float cosDec = Mathf.Cos(Dec);

            //The Sidereal Time.
            //---------------------------------------------------------------------------------------------------
            float Ls = v + w;

            float GMST0 = Ls + 180.0f;
            float UT = 15.0f * hour;//Universal Time.
            float GMST = GMST0 + UT;
            float LST = m_radians * (GMST + longitude);

            //Store local sideral time.
            lst = LST;

            //Azimuthal coordinates.
            //---------------------------------------------------------------------------------------------------
            float HA = LST - RA;
            float sinHA = Mathf.Sin(HA);
            float cosHA = Mathf.Cos(HA);

            float x = cosHA * cosDec;
            float y = sinHA * cosDec;
            float z = sinDec;

            float xhor = x * m_sinLatitude - z * m_cosLatitude;
            float yhor = y;
            float zhor = x * m_cosLatitude + z * m_sinLatitude;

            //az  = atan2( yhor, xhor ) + 180_degrees
            //alt = asin( zhor ) = atan2( zhor, sqrt(xhor*xhor+yhor*yhor) )
            float azimuth = Mathf.Atan2(yhor, xhor) + m_radians * 180.0f;
            float altitude = Mathf.Asin(zhor);

            //Zenith angle.
            //Zenith=90°−α  Where α is the elevation angle.
            float zenith = 90.0f * m_radians - altitude;

            //Converts from Spherical(radius r, zenith-inclination θ, azimuth φ) to Cartesian(x,y,z) coordinates.
            //https://en.wikipedia.org/wiki/Spherical_coordinate_system
            //---------------------------------------------------------------------------------------------------
            //x​​​​ = r sin(θ)cos(φ)​​
            //​y​​​​ = r sin(θ)sin(φ)
            //z = r cos(θ)
            Vector3 ret;

            //radius = 1
            ret.z = Mathf.Sin(zenith) * Mathf.Cos(azimuth);
            ret.x = Mathf.Sin(zenith) * Mathf.Sin(azimuth);
            ret.y = Mathf.Cos(zenith);

            return ret * -1.0f;
        }

		/// <summary>
		/// Used by AzureSkyController script to set the realistic moon position based on Time, Date and Location.
		/// </summary>
		/// <returns></returns>
        public Vector3 SetRealisticMoonPosition ()
        {
            float hour = this.hour - utc;

            //Time Scale.
            //---------------------------------------------------------------------------------------------------
            //d = 367*y - 7 * ( y + (m+9)/12 ) / 4 + 275*m/9 + D - 730530
            //d = d + UT/24.0
            float d = 367 * year - 7 * (year + (month + 9) / 12) / 4 + 275 * month / 9 + day - 730530;
            d = d + hour / 24.0f;

            //Tilt of earth's axis.
            //---------------------------------------------------------------------------------------------------
            //obliquity of the ecliptic.
            float ecliptic = 23.4393f - 3.563E-7f * d;
            //Need convert to radians before apply sine and cosine.
            float radEcliptic = m_radians * ecliptic;
            float sinEcliptic = Mathf.Sin(radEcliptic);
            float cosEcliptic = Mathf.Cos(radEcliptic);

            //Orbital elements of the Moon.
            //---------------------------------------------------------------------------------------------------
            float N = 125.1228f - 0.0529538083f * d;
            float i = 5.1454f;
            float w = 318.0634f + 0.1643573223f * d;
            float a = 60.2666f;
            float e = 0.054900f;
            float M = 115.3654f + 13.0649929509f * d;

            //Eccentric anomaly.
            //---------------------------------------------------------------------------------------------------
            //E = M + e*(180/pi) * sin(M) * ( 1.0 + e * cos(M) )
            float radM = m_radians * M;
            float E = radM + e * Mathf.Sin(radM) * (1f + e * Mathf.Cos(radM));

            //Planet's distance and true anomaly.
            //---------------------------------------------------------------------------------------------------
            //xv = r * cos(v) = a * ( cos(E) - e )
            //yv = r * sin(v) = a * ( sqrt(1.0 - e*e) * sin(E) )
            float xv = a * (Mathf.Cos(E) - e);
            float yv = a * (Mathf.Sqrt(1f - e * e) * Mathf.Sin(E));
            //V = atan2 (yv, xv)
            //R = sqrt (xv * xv + yv * yv)
            float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);
            float r = Mathf.Sqrt(xv * xv + yv * yv);

            //Moon position in 3D space.
            //---------------------------------------------------------------------------------------------------
            float radLongitude = m_radians * (v + w);
            float sinLongitude = Mathf.Sin(radLongitude);
            float cosLongitude = Mathf.Cos(radLongitude);

            //Geocentric (Earth-centered) coordinates.
            //---------------------------------------------------------------------------------------------------
            //xh = r * ( cos(N) * cos(v+w) - sin(N) * sin(v+w) * cos(i) )
            //yh = r * ( sin(N) * cos(v+w) + cos(N) * sin(v+w) * cos(i) )
            //zh = r * ( sin(v+w) * sin(i) )
            float radN = m_radians * N;
            float radI = m_radians * i;

            float xh = r * (Mathf.Cos(radN) * cosLongitude - Mathf.Sin(radN) * sinLongitude * Mathf.Cos(radI));
            float yh = r * (Mathf.Sin(radN) * cosLongitude + Mathf.Cos(radN) * sinLongitude * Mathf.Cos(radI));
            float zh = r * (sinLongitude * Mathf.Sin(radI));

            //float xg = xh; //No needed to the moon.
            //float yg = yh;
            //float zg = zh;

            //Equatorial coordinates.
            //---------------------------------------------------------------------------------------------------
            float xe = xh;
            float ye = yh * cosEcliptic - zh * sinEcliptic;
            float ze = yh * sinEcliptic + zh * cosEcliptic;

            //Planet's Right Ascension (RA) and Declination (Dec).
            //---------------------------------------------------------------------------------------------------
            float RA = Mathf.Atan2(ye, xe);
            float Dec = Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));

            //The Sidereal Time.
            //---------------------------------------------------------------------------------------------------
            //It is already calculated for the sun and stored in the lst, it is not necessary to calculate again for the moon.
            //float Ls = ls;

            //float GMST0 = Ls + 180.0f;
            //float UT    = 15.0f * hour;
            //float GMST  = GMST0 + UT;
            //float LST   = radians * (GMST + Azure_Longitude);

            //Azimuthal coordinates.
            //---------------------------------------------------------------------------------------------------
            float HA = lst - RA;

            float x = Mathf.Cos(HA) * Mathf.Cos(Dec);
            float y = Mathf.Sin(HA) * Mathf.Cos(Dec);
            float z = Mathf.Sin(Dec);

            float xhor = x * m_sinLatitude - z * m_cosLatitude;
            float yhor = y;
            float zhor = x * m_cosLatitude + z * m_sinLatitude;

            //az  = atan2( yhor, xhor ) + 180_degrees
            //alt = asin( zhor ) = atan2( zhor, sqrt(xhor*xhor+yhor*yhor) )
            float azimuth = Mathf.Atan2(yhor, xhor) + m_radians * 180.0f;
            float altitude = Mathf.Asin(zhor);

            //Zenith angle.
            //Zenith = 90°−α  where α is the elevation angle.
            float zenith = 90.0f * m_radians - altitude;

            //Converts from Spherical(radius r, zenith-inclination θ, azimuth φ) to Cartesian(x,y,z) coordinates.
            //https://en.wikipedia.org/wiki/Spherical_coordinate_system
            //---------------------------------------------------------------------------------------------------
            //x​​​​ = r sin(θ)cos(φ)​​
            //​y​​​​ = r sin(θ)sin(φ)
            //z = r cos(θ)
            Vector3 ret;

            //radius = 1
            ret.z = Mathf.Sin(zenith) * Mathf.Cos(azimuth);
            ret.x = Mathf.Sin(zenith) * Mathf.Sin(azimuth);
            ret.y = Mathf.Cos(zenith);

            return ret * -1.0f;
        }
    }
}