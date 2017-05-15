using CoreLocation;
using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UIKit;

namespace Caelum
{

    public partial class NewEntry : UIViewController
    {
        private UITextField tfCity;
        public NewEntry() : base("NewEntry", null) { }

        public override void DidReceiveMemoryWarning() => base.DidReceiveMemoryWarning();

        private void BtnAdd_TouchUpInside(object sender, EventArgs e)
        {
            tfCity.ResignFirstResponder();

            if (new DBA().DatensaetzeHolen().Count > TableModel.MAX_ANZAHL) { Funktionen.actions["Toast"]("Bitte loesch zuerst einen Eintrag."); return; }

            WetterEintrag we = null;
            var city = tfCity.Text.Trim();

            if (city.Length == 0) Funktionen.actions["Toast"]("Bitte gib etwas ein.");
            else if (new Regex(@"^\w*(\d+)\w*$").IsMatch(city)) Funktionen.actions["Toast"]("Zahlen sind nicht erlaubt.");
            else if (new Regex(@"^\w*(\p{P}+)\w*$").IsMatch(city)) Funktionen.actions["Toast"]("Sonderzeichen sind nicht erlaubt.");
            else
            {
                var doc = new XmlDocument();
                try
                {
                    doc.LoadXml(new WebClient() { Encoding = Encoding.UTF8 }.DownloadString($"{Link.MAINLINK}q={city}&mode=xml&lang=de&appid={Link.KEY}"));
                    var coord = doc.SelectSingleNode("//current/city/coord");
                    double.TryParse(coord.Attributes["lon"].Value, out var lon);
                    double.TryParse(coord.Attributes["lat"].Value, out var lat);
                    float.TryParse(doc.SelectSingleNode("//current/temperature").Attributes["value"].Value, out var temp);
                    int.TryParse(doc.SelectSingleNode("//current/humidity").Attributes["value"].Value, out var humidity);
                    int.TryParse(doc.SelectSingleNode("//current/pressure").Attributes["value"].Value, out var pressure);
                    float.TryParse(doc.SelectSingleNode("//current/wind/speed").Attributes["value"].Value, out var windSpeed);
                    var direction = doc.SelectSingleNode("//current/wind/direction").Attributes["code"].Value;
                    var weatherValue = doc.SelectSingleNode("//current/weather").Attributes["value"].Value;
                    var date = doc.SelectSingleNode("//current/lastupdate").Attributes["value"].Value;
                    var country = doc.SelectSingleNode("//current/city/country").InnerText;
                    we = new WetterEintrag(city, lon, lat, temp, humidity, pressure, windSpeed, direction, weatherValue, date, country);
                }
                catch (Exception)
                {
                    Funktionen.actions["Toast"]("Es ist ein unerwarteter Fehler aufgetreten.");
                    return;
                }

                new DBA().DatensatzEinfügen(we);
                tfCity.Text = "";
                Funktionen.actions["Toast"]($"{we.City} wurde erfolgreich hinzugefuegt.");
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            tfCity = UIFactory.CreateTextField(10, 100, 300, 35, "Stadt");
            Add(tfCity);
            Add(UIFactory.CreateMDButton(10, 145, 140, 30, "Standort",
                (sender, e) =>
                    {
                        var loc = new CLLocationManager();
                        loc.RequestWhenInUseAuthorization();
                        var doc = new XmlDocument();
                        doc.LoadXml(new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(
                            $"{Link.MAINLINK}lat={loc.Location.Coordinate.Latitude}&lon={loc.Location.Coordinate.Longitude}&mode=xml&lang=de&appid={Link.KEY}"));
                        tfCity.Text = doc.SelectSingleNode("//current/city").Attributes["name"].Value;
                        BtnAdd_TouchUpInside(sender, e);
                    }));
            Add(UIFactory.CreateMDButton(170, 145, 140, 30, "Hinzufuegen", BtnAdd_TouchUpInside));
        }
    }
}