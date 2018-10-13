using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Attendance_App.Utilities
{
    public class ToastMessage
    {
        public static void Show(string message)
        {
                DependencyService.Get<ToastHandler>().Show(message);
        }
    }
}
