using Attendance_App.Models;
using Attendance_App.Utilities;
using Firebase.Database;
using Firebase.Database.Query;
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
	public partial class AttendancePage : ContentPage
	{
        private string currentUser;
        private IReadOnlyCollection<FirebaseObject<Attendance>> attendanceList;
        public string TodayDate { get; set; }

        private FirebaseClient firebase;

      
        public AttendancePage ()
		{
			InitializeComponent ();
            this.BindingContext = new {
                TodayDate_ = DateTime.Now.ToShortDateString()
        };
          
        }

        protected async override void OnAppearing()
        {

            try
            {
                TodayDate = DateTime.Now.ToShortDateString();
                animationView.IsVisible = false;
                firebase = new FirebaseClient(App.FirebaseUrl);

                currentUser = Application.Current.Properties["currentUser"].ToString();

                attendanceList =  await firebase.Child("Attendance").OnceAsync<Attendance>();

                if (IsWeekEnd())
                {
                    ToastMessage.Show("cannot mark attendance on weekends");
                    MarkAttendance.IsEnabled = false;
                }
                if (IsMarkedToday(attendanceList))
                {
                    ToastMessage.Show("cannot mark attendance twice a day");
                    MarkAttendance.IsEnabled = false;
                }


                base.OnAppearing();
            }
            catch (Exception ex)
            {
                //await DisplayAlert("error", ex.StackTrace, "ok");
                //await DisplayAlert("message", ex.Message, "ok");
                ToastMessage.Show(ex.Message);

            }

        }





        private async void MarkAttendance_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!IsMarkedToday(attendanceList) || !IsWeekEnd())
                {
                    var attendance = new Attendance();
                    attendance.Id = string.Empty;
                    attendance.Date = DateTime.Now.ToShortDateString();
                    attendance.IsPresent = true;
                    attendance.Username = currentUser;

                    await firebase.Child("Attendance").PostAsync<Attendance>( attendance);
                    ToastMessage.Show("Attendance marked successfully...");
                    MarkAttendance.IsEnabled = false;
                    animationView.IsVisible = true;
                    

                }
                else
                {
                    MarkAttendance.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                //await DisplayAlert("error", ex.StackTrace, "ok");
                //await DisplayAlert("message", ex.Message, "ok");
                ToastMessage.Show(ex.Message);
            }

        }

        public bool IsMarkedToday(IReadOnlyCollection<FirebaseObject<Attendance>> attendanceList)
        {
            var tickCount = attendanceList.Where(a => a.Object.Date == DateTime.Now.Date.ToShortDateString() && a.Object.Username == currentUser && a.Object.IsPresent == true).Count();
            if (tickCount > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsWeekEnd()
        {
            var today = DateTime.Now.DayOfWeek;
            if (today == DayOfWeek.Saturday || today == DayOfWeek.Sunday)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}