using System;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.Text;

namespace Caelum
{
    public class DBA
    {
        private const String DBNAME = "caelum.sqlite";
        private SqliteConnection connection;
        private String db_file;

        public DBA()
        {
            db_file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DBNAME);
            DatenbankErzeugen();
        }

        public void DatenbankErzeugen()
        {
            if (!File.Exists(db_file)) SqliteConnection.CreateFile(db_file);
            connection = new SqliteConnection($"Data Source={db_file};");
            HandleConnection(connection, () => DoQuery(connection,
                "CREATE TABLE IF NOT EXISTS entries ("
                + "entry_id INTEGER PRIMARY KEY AUTOINCREMENT, "
                + "city NTEXT, "
                + "country NTEXT, "
                + "longitude REAL, "
                + "latitude REAL, "
                + "temperature REAL, "
                + "humidity INTEGER, "
                + "pressure INTEGER, "
                + "windspeed REAL, "
                + "winddirection NTEXT, "
                + "weathervalue NTEXT, "
                + "lastupdate NTEXT);"));
        }

        /// <summary>
        /// Loescht den Datensatz mit der uebergebenen id. 
        /// 
        /// Wird null uebergeben, wird die komplette Tabelle geleert.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Anzahl der betroffenen Datensaetze</returns>
        public void DatensatzLoeschen(int? id) => HandleConnection(connection, () => DoQuery(connection, "DELETE FROM entries" + (id == null ? ";" : $" WHERE entry_id = {id};")));

        public List<WetterEintrag> DatensaetzeHolen()
        {
            var list = new List<WetterEintrag>();
            HandleConnection(connection, () =>
            {
                var reader = (SqliteDataReader)DoQuery(connection, "SELECT * FROM entries;", true);
                while (reader.Read())
                {
                    int.TryParse(reader["entry_id"].ToString(), out int id);
                    double.TryParse(reader["longitude"].ToString(), out double lon);
                    double.TryParse(reader["latitude"].ToString(), out var lat);
                    float.TryParse(reader["temperature"].ToString(), out var temperature);
                    int.TryParse(reader["humidity"].ToString(), out var humidity);
                    int.TryParse(reader["pressure"].ToString(), out var pressure);
                    float.TryParse(reader["windspeed"].ToString(), out var windspeed);
                    list.Add(new WetterEintrag(id,
                        (String)reader["city"],
                        lon, lat, temperature, humidity,
                        pressure, windspeed,
                        (String)reader["winddirection"],
                        (String)reader["weathervalue"],
                        (String)reader["lastupdate"],
                        (String)reader["country"]));
                }
            });
            return list;
        }

        /// <summary>
        /// Wenn id null ist, werden alle aktualisiert, ansonsten nur der mit der uebergebenen id.
        /// </summary>
        /// <param name="id"></param>
        public void UpdateEntry(int? id) =>
            DatensaetzeHolen().ForEach((entry) =>
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(Link.MAINLINK + $"lat={entry.Latitude}&lon={entry.Longitude}&mode=xml&lang=de&appid={Link.KEY}"));
                HandleConnection(connection, () => DoQuery(connection, "UPDATE entries SET " +
                    $"temperature = {(float.TryParse(doc.SelectSingleNode("//current/temperature").Attributes["value"].Value, out var tem) ? tem : entry.Temperature)}, " +
                    $"humidity = {(int.TryParse(doc.SelectSingleNode("//current/humidity").Attributes["value"].Value, out var hum) ? hum : entry.Humidity)}, " +
                    $"pressure = {(int.TryParse(doc.SelectSingleNode("//current/pressure").Attributes["value"].Value, out var pre) ? pre : entry.Pressure)}, " +
                    $"windspeed = {(int.TryParse(doc.SelectSingleNode("//current/wind/speed").Attributes["value"].Value, out var wind) ? wind : entry.WindSpeed)}, " +
                    $"winddirection = {quoted(doc.SelectSingleNode("//current/wind/direction").Attributes["code"].Value)}, " +
                    $"weathervalue = {quoted(doc.SelectSingleNode("//current/weather").Attributes["value"].Value)}, " +
                    $"lastupdate = {quoted(doc.SelectSingleNode("current/lastupdate").Attributes["value"].Value)} " +
                    $"WHERE {(id == null ? $"city LIKE {quoted(entry.City)}" : $"entry_id = {entry.Id}")};"));
            });

        /// <summary>
        /// Eine Methode zum einfügen eines Wettereintrags in die Datenbank
        /// </summary>
        /// <param name="we">Wettereintrag</param>
        /// <returns></returns>
        public void DatensatzEinfügen(WetterEintrag we)
            => HandleConnection(connection, () => DoQuery(connection,
                "INSERT INTO entries (" +
                "city, country, longitude, latitude, " +
                "temperature, humidity, pressure, " +
                "windspeed, winddirection, weathervalue, lastupdate)" +
                "VALUES " +
                $"({quoted(we.City)}, {quoted(we.Country)}, {we.Longitude}, {we.Latitude}, " +
                $"{we.Temperature}, {we.Humidity}, {we.Pressure}, " +
                $"{we.WindSpeed}, {quoted(we.WindDirection)}, {quoted(we.WeatherValue)}, {quoted(we.LastUpdate)});"));

        private Object DoQuery(SqliteConnection con, String sql, bool reader = false)
        {
            var cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            return reader ? (object)cmd.ExecuteReader() : (object)cmd.ExecuteNonQuery();
        }

        private void HandleConnection(SqliteConnection connect, Action action)
        {
            connect.Open();
            action();
            connect.Close();
        }

        private String quoted(String str) => $"'{str}'";
    }
}