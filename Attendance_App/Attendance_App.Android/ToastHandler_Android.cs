﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Attendance_App.Droid;
using Attendance_App.Utilities;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ToastHandler_Android))]
namespace Attendance_App.Droid
{
    public class ToastHandler_Android : ToastHandler
    {
        public void Show(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}