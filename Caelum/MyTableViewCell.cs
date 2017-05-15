using System;

using Foundation;
using UIKit;
using System.IO;
using System.Collections.Generic;

namespace Caelum
{
    public partial class SpecialTableViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MyTableViewCell");
        public static readonly UINib Nib;
        private const String FONTNAME = "WeatherIcons-Regular";
        public WetterEintrag Model { get; set; }

        static SpecialTableViewCell() => Nib = UINib.FromName("MyTableViewCell", NSBundle.MainBundle);

        protected SpecialTableViewCell(IntPtr handle) : base(handle) { }

        public static SpecialTableViewCell Create() => (SpecialTableViewCell)Nib.Instantiate(null, null)[0];

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            String file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), XMLHandler.FILENAME);
            DateTime date = WetterEintrag.StringToDateTime(Model.LastUpdate);
            Dictionary<UILabel, String> labels = new Dictionary<UILabel, String>()
            {
                { lblCity, Model.City + (XMLHandler.IsChecked(file,"//states/country") ? $" ({Model.Country})" : "") },
                { lblDirection, $"{Model.WindSpeed} m/s " + (Model.WindDirection.Length > 0 ? XMLHandler.IsChecked(file, "//states/arrow") 
                    ? WetterEintrag.ConvertDirectionSymbols(Model.WindDirection).ToString() : 
                    $"aus {WetterEintrag.ConvertTuple(Model.WindDirection, XMLHandler.IsChecked(file, "//states/abkuerzung"))}" : "")},
                { lblHumidity, $"{Model.Humidity} %" },
                { lblTemp, XMLHandler.IsChecked(file, "//states/kelvin") ? $"{Model.Temperature} K" :
                    WetterEintrag.KelvinToCelcius(Model.Temperature).ToString("0.00") + " \u00b0C" },
                { lblWeatherValue, Model.WeatherValue },
                { lblPressure, $"{Model.Pressure} hPa" },
                { lblSymbol, WetterEintrag.ConvertWeatherSymbols(Model.WeatherValue, date.Hour > 20 || date.Hour < 5).ToString() },
                { lblDate, XMLHandler.IsChecked(file, "//states/date") ? 
                    $"{date.Day}.{date.Month}.{date.Year} {date.Hour}:{(date.Minute < 10 ? "0" + date.Minute:date.Minute.ToString())} Uhr" : "" },
                { lblTempSym, '\uf055'.ToString() }, { lblPressureSym, '\uf079'.ToString() }, { lblHumSym, '\uf07a'.ToString() },
            };
            foreach (UILabel lbl in labels.Keys) if (labels.TryGetValue(lbl, out var res)) lbl.Text = res;
            lblHumSym.Font = lblTempSym.Font = lblPressureSym.Font = lblDirection.Font = UIFont.FromName(FONTNAME, 17f);
            lblHumSym.Hidden = lblTempSym.Hidden = lblPressureSym.Hidden = !XMLHandler.IsChecked(file, "//states/symbols");
            lblSymbol.Font = lblDirection.Font.WithSize(32f);
            Tag = Model.Id;
        }
    }
}
