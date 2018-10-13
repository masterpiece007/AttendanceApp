using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace Attendance_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        public SplashScreen()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await appLogo.ScaleTo(1, 2000);
            await appLogo.ScaleTo(0.5, 2000, Easing.Linear);
            await appLogo.ScaleTo(170, 1200, Easing.Linear);

            if (Application.Current.Properties.ContainsKey("rememberMe"))
            {
                var rememberMe = bool.Parse(Application.Current.Properties["rememberMe"].ToString());
                if (rememberMe)
                {
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                }
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }



        }
    }

}