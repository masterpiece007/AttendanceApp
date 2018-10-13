using Attendance_App.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Attendance_App
{
    public partial class App : Application
    {
        public static string FirebaseUrl = "";

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new SplashScreen());
        }

        protected override void OnStart()
        {
            FirebaseUrl = "https://attendanceapp-98af4.firebaseio.com/";
           
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
