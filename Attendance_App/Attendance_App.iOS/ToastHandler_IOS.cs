using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Attendance_App.iOS;
using Attendance_App.Utilities;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastHandler_IOS))]
namespace Attendance_App.iOS
{
    public class ToastHandler_IOS : ToastHandler
    {
       
        const double SHORT_DELAY = 2.0;

        NSTimer alertDelay;
        UIAlertController alert;

      
        public void Show(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }
}