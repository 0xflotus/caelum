// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Caelum
{
    [Register ("MyTableViewCell")]
    partial class SpecialTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblCity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDirection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblHumidity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblHumSym { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPressure { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPressureSym { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblSymbol { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTemp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTempSym { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblWeatherValue { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblCity != null) {
                lblCity.Dispose ();
                lblCity = null;
            }

            if (lblDate != null) {
                lblDate.Dispose ();
                lblDate = null;
            }

            if (lblDirection != null) {
                lblDirection.Dispose ();
                lblDirection = null;
            }

            if (lblHumidity != null) {
                lblHumidity.Dispose ();
                lblHumidity = null;
            }

            if (lblHumSym != null) {
                lblHumSym.Dispose ();
                lblHumSym = null;
            }

            if (lblPressure != null) {
                lblPressure.Dispose ();
                lblPressure = null;
            }

            if (lblPressureSym != null) {
                lblPressureSym.Dispose ();
                lblPressureSym = null;
            }

            if (lblSymbol != null) {
                lblSymbol.Dispose ();
                lblSymbol = null;
            }

            if (lblTemp != null) {
                lblTemp.Dispose ();
                lblTemp = null;
            }

            if (lblTempSym != null) {
                lblTempSym.Dispose ();
                lblTempSym = null;
            }

            if (lblWeatherValue != null) {
                lblWeatherValue.Dispose ();
                lblWeatherValue = null;
            }
        }
    }
}