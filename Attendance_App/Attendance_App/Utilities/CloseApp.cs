using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Attendance_App.Utilities
{
    public class CloseApp
    {
        public static void Done()
        {
                DependencyService.Get<ICloseApplication>().CloseApp();
        }
    }
}
