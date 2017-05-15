using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Caelum
{

    public class WetterEintrag
    {
        const float KELVINKONSTANTE = 273.15f;

        static List<String> weatherPossible = new List<string>()
        {
            "klarer himmel",
            "leichtes nieseln",
            "überwiegend bewölkt",
            "regenschauer",
            "sehr starker regen",
            "ein paar wolken",
            "leichter regen",
            "wolkenbedeckt",
            "leichte schneeschauer",
            "trüb",
            "dunst",
        };

        static Dictionary<String, char> weathers = new Dictionary<string, char>()
            {
                { weatherPossible[0],  '\uf00d'},
                { weatherPossible[1],  '\uf01c'},
                { weatherPossible[2], '\uf013' },
                { weatherPossible[3], '\uf01a'},
                { weatherPossible[4], '\uf019' },
                { weatherPossible[5], '\uf002' },
                { weatherPossible[6], '\uf006' },
                { weatherPossible[7], '\uf07d'},
                { weatherPossible[8], '\uf00a'},
                { weatherPossible[9], '\uf003' },
                { weatherPossible[10], '\uf003' },
            };

        static Dictionary<String, char> weathersByNight = new Dictionary<string, char>()
        {
                { weatherPossible[0],  '\uf02e'},
                { weatherPossible[1],  '\uf029'},
                { weatherPossible[2], '\uf086' },
                { weatherPossible[3], '\uf034'},
                { weatherPossible[4], '\uf036' },
                { weatherPossible[5], '\uf083' },
                { weatherPossible[6], '\uf0b4' },
                { weatherPossible[7], '\uf080'},
                { weatherPossible[8], '\uf02a'},
                { weatherPossible[9], '\uf04a' },
                { weatherPossible[10], '\uf04a' },
        };

        static Dictionary<String, char> directions = new Dictionary<string, char>
        {
            { "N", '\uf044' },
            { "W", '\uf04d' },
            { "S", '\uf058' },
            { "E", '\uf048' },
            { "NW", '\uf088' },
            { "NE", '\uf043' },
            { "NNW", '\uf088' },
            { "NNE", '\uf043' },
            { "SW", '\uf057' },
            { "SE", '\uf087' },
            { "SSW", '\uf057' },
            { "SSE", '\uf087' },
            { "ENE", '\uf048' },
            { "ESE", '\uf048' },
            { "WSW", '\uf04d' },
            { "WNW", '\uf04d' }
        };

        static List<(String abkInt, String abkGer, String nameGer)> tuples = new List<(string, string, string)>
        {
            ("N", "N", "Norden"),
            ("S", "S", "Sueden"),
            ("W", "W", "Westen"),
            ("E", "O", "Osten"),
            ("NE", "NO", "Nordosten"),
            ("NW", "NW", "Nordwesten"),
            ("NNW", "NNW", "Nordnordwesten"),
            ("NNE", "NNO", "Nordnordosten"),
            ("SE", "SO", "Suedosten"),
            ("SW", "SW", "Suedwesten"),
            ("SSW", "SSW", "Suedsuedwesten"),
            ("SSE", "SSO", "Suedsuedosten"),
            ("ENE", "ONO", "Ostnordosten"),
            ("WSW", "WSW", "Westsuedwesten"),
            ("ESE", "OSO", "Ostsuedosten"),
            ("WNW", "WNW", "Westnordwesten"),
        };

        public readonly int Id;
        public readonly String City;
        public readonly double Longitude;
        public readonly double Latitude;
        public readonly float Temperature;
        public readonly int Humidity;
        public readonly int Pressure;
        public readonly float WindSpeed;
        public readonly String WindDirection;
        public readonly String WeatherValue;
        public readonly String LastUpdate;
        public readonly String Country;

        public WetterEintrag(int id, String city, double lon, double lat, float temp, int hum, int press, float speed, String direction, String val,
            String date, String country)
        {
            Id = id;
            City = city;
            Longitude = lon;
            Latitude = lat;
            Temperature = temp;
            Humidity = hum;
            Pressure = press;
            WindSpeed = speed;
            WindDirection = direction;
            WeatherValue = val;
            LastUpdate = date;
            Country = country;
        }
        public WetterEintrag(String city, double lon, double lat, float temp, int hum, int press, float speed, String direction, String val,
            String date, String country) : this(0, city, lon, lat, temp, hum, press, speed, direction, val, date, country) { }

        public static DateTime StringToDateTime(String time)
        {
            String[] arr = new Regex(@"-|T|:").Split(time);
            return new DateTime(int.Parse(arr[0]), int.Parse(arr[1]), int.Parse(arr[2]), int.Parse(arr[3]), int.Parse(arr[4]), 0);
        }
        public static float KelvinToCelcius(float kelvin) => kelvin - KELVINKONSTANTE;
        public static char ConvertWeatherSymbols(String weather, bool night)
            => night ? weathersByNight.TryGetValue(weather.ToLower(), out var late) ?
                late : '\uf081' : weathers.TryGetValue(weather.ToLower(), out var res) ? res : '\uf031';
        public static char ConvertDirectionSymbols(String direction) => directions.TryGetValue(direction.ToUpper(), out var res) ? res : ' ';
        public static String ConvertTuple(String s, bool abk) => abk ? tuples.Find(t => t.abkInt == s).abkGer : tuples.Find(t => t.abkInt == s).nameGer;
    }
}