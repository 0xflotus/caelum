using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using MapKit;
using CoreLocation;

namespace Caelum
{
    public class TableModel : UITableViewSource
    {
        public List<WetterEintrag> Tabellenzeilen { get; }
        public readonly static uint MAX_ANZAHL = 20;
        private Overview overview;
        public TableModel(Overview overview)
        {
            this.overview = overview;
            Tabellenzeilen = new DBA().DatensaetzeHolen();
            if (Tabellenzeilen.Count == 0) Funktionen.actions["Toast"]("Es sind keine Daten vorhanden.");
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var entry = Tabellenzeilen[indexPath.Row];
            var cell = (SpecialTableViewCell)tableView.DequeueReusableCell("tag");
            cell = cell ?? SpecialTableViewCell.Create();
            cell.Model = entry;
            cell.AddGestureRecognizer(new UILongPressGestureRecognizer(() =>
            {
                var dba = new DBA();
                overview.PresentViewController(UIFactory.CreateAlertController(
                    new String[] { "Aktionen", "Was willst du tun?" },
                    new UIAlertAction[] {
                        UIAlertAction.Create("Loeschen", UIAlertActionStyle.Default,
                            action =>
                                {
                                    dba.DatensatzLoeschen((int)cell.Tag);
                                    reloadTable(tableView);
                                }),
                        UIAlertAction.Create("Aktualisieren", UIAlertActionStyle.Default,
                            action =>
                                {
                                    dba.UpdateEntry((int)cell.Tag);
                                    reloadTable(tableView);
                                }),
                        UIAlertAction.Create("Karte", UIAlertActionStyle.Default,
                            action =>
                                {
                                    overview.View = UIFactory.CreateMapView(UIScreen.MainScreen.Bounds, new MKPointAnnotation()
                                    {
                                        Title = entry.City,
                                        Coordinate = new CLLocationCoordinate2D(entry.Latitude, entry.Longitude)
                                    });
                                }),
                        UIAlertAction.Create("Wikipedia", UIAlertActionStyle.Default,
                            action => UIApplication.SharedApplication.OpenUrl(new NSUrl(Link.WP_BASE_URL + entry.City))),
                        UIAlertAction.Create("Abbruch", UIAlertActionStyle.Cancel, null)
                    }), true, null);
            }));
            return cell;
        }
        public override nint RowsInSection(UITableView tableview, nint section) => Tabellenzeilen.Count;
        private void reloadTable(UITableView table)
        {
            table.Source = new TableModel(overview);
            table.ReloadData();
        }
    }
}