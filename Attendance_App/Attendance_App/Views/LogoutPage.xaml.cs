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
	public partial class LogoutPage : ContentPage
	{
		public LogoutPage ()
		{
			InitializeComponent ();
		}
        protected  override void OnAppearing()
        {
            
            base.OnAppearing();
            Logout();
        }

        public async void Logout()
        {
            Application.Current.Properties["currentUser"] = "";
            Application.Current.Properties["rememberMe"] = false;
            await Application.Current.SavePropertiesAsync();
        }
    }
}