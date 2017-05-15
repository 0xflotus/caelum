using System;
using ToastIOS;
using UIKit;
using Xamarin.Themes;

namespace Caelum
{
    public partial class Overview : UIViewController
    {
        public Overview() : base("Overview", null) { }

        public override void DidReceiveMemoryWarning() => base.DidReceiveMemoryWarning();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Add(UIFactory.CreateTableView(0, 0, 310, (float)UIScreen.MainScreen.Bounds.Height, new TableModel(this)));
        }
    }
}