using AnimatedButtons;
using BigTed;
using System;
using System.Collections.Generic;
using System.IO;
using UIKit;

namespace Caelum
{
    public partial class Settings : UIViewController
    {
        private String file;
        private bool stateArrow, stateKelvin, stateDate, stateCountry, stateAbkuerzung, stateSymbols;
        public Settings() : base("Settings", null)
        {
            file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), XMLHandler.FILENAME);
            Action action = () => Funktionen.actions["Toast"]("Keine Datei vorhanden.");
            (File.Exists(file) ? () =>
            {
                try
                {
                    stateArrow = XMLHandler.IsChecked(file, "//states/arrow");
                    stateKelvin = XMLHandler.IsChecked(file, "//states/kelvin");
                    stateDate = XMLHandler.IsChecked(file, "//states/date");
                    stateCountry = XMLHandler.IsChecked(file, "//states/country");
                    stateAbkuerzung = XMLHandler.IsChecked(file, "//states/abkuerzung");
                    stateSymbols = XMLHandler.IsChecked(file, "//states/symbols");
                }
                catch (NullReferenceException)
                {
                    Funktionen.actions["Toast"]("Die Datei war fehlerhaft.");
                    stateArrow = stateKelvin = stateDate = stateCountry = stateAbkuerzung = stateSymbols = false;
                }
            }
            : action)();
        }

        public override void DidReceiveMemoryWarning() => base.DidReceiveMemoryWarning();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            List<UIView> views = new List<UIView>()
            {
                UIFactory.CreateLabel(10, 80, 220, 30, "Windrichtung als Pfeile"),
                UIFactory.CreateLabel(10, 120, 220, 30, "Uhrzeit anzeigen"),
                UIFactory.CreateLabel(10, 160, 220, 30, "Temperatur in"),
                UIFactory.CreateLabel(10, 200, 220, 30, "Land anzeigen"),
                UIFactory.CreateLabel(10, 240, 220, 30, "Windrichtung abkuerzen"),
                UIFactory.CreateLabel(10, 280, 220, 30, "Symbole anzeigen"),
                UIFactory.CreateTextSwitch(210, 80, (sender, e) => { stateArrow = ((TextSwitch)sender).On; }, stateArrow),
                UIFactory.CreateTextSwitch(210, 120, (sender, e) => { stateDate = ((TextSwitch)sender).On; }, stateDate),
                UIFactory.CreateTextSwitch(210, 160, (sender, e) => { stateKelvin = ((TextSwitch)sender).On; }, stateKelvin, leftTitle: "\u00b0C", rightTitle: "K"),
                UIFactory.CreateTextSwitch(210, 200, (sender, e) => { stateCountry = ((TextSwitch)sender).On; }, stateCountry),
                UIFactory.CreateTextSwitch(210, 240, (sender, e) => { stateAbkuerzung = ((TextSwitch)sender).On; }, stateAbkuerzung),
                UIFactory.CreateTextSwitch(210, 280, (sender, e) => { stateSymbols = ((TextSwitch)sender).On; }, stateSymbols),
                UIFactory.CreateMDButton(170, 350, 130, 30, "Delete All",
                    (sender, e) =>
                        {
                            UIFactory.MakeWaitDialog(() => new DBA().DatensatzLoeschen(null));
                            Funktionen.actions["Toast"]("Es wurden alle Eintraege geloescht.");
                        }),
                UIFactory.CreateMDButton(170, 390, 130, 30, "Update All",
                    (sender, e) =>
                        {
                            UIFactory.MakeWaitDialog(() => new DBA().UpdateEntry(null));
                            Funktionen.actions["Toast"]("Es wurden alle Eintraege aktualisiert.");
                        }),
            };
            views.ForEach(view => Add(view));
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            XMLHandler.WriteXML(file, new Dictionary<String, bool>()
            {
                { "arrow", stateArrow },
                { "kelvin", stateKelvin },
                { "date", stateDate },
                { "country", stateCountry },
                { "abkuerzung", stateAbkuerzung },
                { "symbols", stateSymbols },
            });
        }
    }
}