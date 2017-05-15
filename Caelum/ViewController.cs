using MaterialControls;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Caelum
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            List<UIView> views = new List<UIView>()
            {
                UIFactory.CreateLabel(10, 100, 300, 30, "Welcome on Caelum", 30f, UITextAlignment.Center),
                UIFactory.CreateMDButton(80, 160, 160, 30, "Neuer Eintrag", btn_TouchUpInside),
                UIFactory.CreateMDButton(80, 200, 160, 30, "Uebersicht", btn_TouchUpInside),
                UIFactory.CreateMDButton(80, 240, 160, 30, "Einstellungen", btn_TouchUpInside),
            };
            views.ForEach(view => Add(view));
        }

        private void btn_TouchUpInside(object sender, EventArgs e)
            => NavigationController.PushViewController(((MDButton)sender).Title(UIControlState.Normal) == "Neuer Eintrag" ?
                (UIViewController)new NewEntry() : ((MDButton)sender).Title(UIControlState.Normal) == "Einstellungen" ?
                (UIViewController)new Settings() : (UIViewController)new Overview(), true);

        public override void DidReceiveMemoryWarning() => base.DidReceiveMemoryWarning();
    }
}