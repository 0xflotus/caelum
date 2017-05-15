using System;
using Foundation;
using UIKit;
using AnimatedButtons;
using CoreGraphics;
using MaterialControls;
using MapKit;
using System.Collections.Generic;
using BigTed;

namespace Caelum
{
    public static class UIFactory
    {
        static UIColor bgColor = UIColor.Black, textColor = UIColor.White;
        public static UILabel CreateLabel(float x, float y, float width, float height, String text, float size = 17f, 
            UITextAlignment align = UITextAlignment.Left)
        {
            var lbl = new UILabel(new CGRect(x, y, width, height))
            {
                Text = text,
                BackgroundColor = textColor,
                TextColor = bgColor,
                TextAlignment = align
            };
            lbl.Font = lbl.Font.WithSize(size);
            return lbl;
        }

        public static TextSwitch CreateTextSwitch(float x, float y, EventHandler e, bool state = false, String leftTitle = "Off", String rightTitle = "On",
            float width = 80, float height = 30, bool enabled = true)
        {
            var ts = new TextSwitch(new CGRect(x, y, width, height))
            {
                BackgroundColor = bgColor,
                SelectedBackgroundColor = textColor,
                TitleColor = textColor,
                SelectedTitleColor = bgColor,
                LeftTitle = leftTitle,
                RightTitle = rightTitle,
                On = state,
                Enabled = enabled,
            };
            ts.ValueChanged += e;
            return ts;
        }

        public static void MakeWaitDialog(Action action, String message = "Loading...")
        {
            BTProgressHUD.Show(message);
            action();
            BTProgressHUD.Dismiss();
        }

        public static UIAlertController CreateAlertController(String[] titleAndMessage, UIAlertAction[] actions)
        {
            var alert = UIAlertController.Create(titleAndMessage[0], titleAndMessage[1], UIAlertControllerStyle.ActionSheet);
            new List<UIAlertAction>(actions).ForEach(action => alert.AddAction(action));
            return alert;
        }

        public static MKMapView CreateMapView(CGRect bounds, IMKAnnotation annotation)
        {
            var map = new MKMapView()
            {
                MapType = MKMapType.Standard,
                ZoomEnabled = true,
                ScrollEnabled = true
            };
            map.AddAnnotation(annotation);
            return map;
        }

        public static UITableView CreateTableView(float x, float y, float width, float height, UITableViewSource model)
            => new UITableView(new CGRect(x, y, width, height), UITableViewStyle.Plain) { Source = model, RowHeight = 155 };

        public static UITextField CreateTextField(float x, float y, float width, float height, String placeholder = "")
            => new UITextField(new CGRect(x, y, width, height))
            {
                AttributedPlaceholder = new NSAttributedString(placeholder, foregroundColor: UIColor.LightGray),
                BackgroundColor = bgColor,
                TextColor = textColor,
                AutocorrectionType = UITextAutocorrectionType.No,
                SpellCheckingType = UITextSpellCheckingType.No
            };

        public static MDButton CreateMDButton(float x, float y, float width, float height, String title, EventHandler e)
        {
            MDButton btn = new MDButton(new CGRect(x, y, width, height), MDButtonType.Raised, textColor);
            btn.SetTitle(title, UIControlState.Normal);
            btn.SetTitleColor(bgColor, UIControlState.Normal);
            btn.TouchUpInside += e;
            return btn;
        }
    }
}