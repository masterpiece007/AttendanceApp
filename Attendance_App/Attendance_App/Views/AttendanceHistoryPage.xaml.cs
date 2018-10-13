using Attendance_App.Models;
using Attendance_App.Utilities;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Attendance_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttendanceHistoryPage : ContentPage
    {
        private string currentUser;
        private IReadOnlyCollection<FirebaseObject<Attendance>> attendanceFromFirebase;
        private FirebaseClient firebase;

        public bool isResultFound { get; set; } = false;
        public ObservableCollection<Attendance> Attendance { get; set; }
        public string SelectedDate { get; set; }

        public AttendanceHistoryPage()
        {
            InitializeComponent();
            NoResultFoundLabel.IsVisible = false;
        }

        protected async override void OnAppearing()
        {
            try
            {
                Attendance = new ObservableCollection<Attendance>();
                firebase = new FirebaseClient(App.FirebaseUrl);

                var hasCurrentUser = Application.Current.Properties.ContainsKey("currentUser");
                if (hasCurrentUser)
                {
                    currentUser = Application.Current.Properties["currentUser"].ToString();
                    attendanceFromFirebase = await firebase.Child("Attendance").OnceAsync<Attendance>();
                }
                if (attendanceFromFirebase.Count <= 0)
                {
                    isResultFound = false;
                }
                else
                {
                    isResultFound = true;
                }
                AttendanceList.ItemsSource = GetAttendanceList();
                base.OnAppearing();
            }
            catch (Exception ex)
            {

                ToastMessage.Show(ex.Message);
                //await DisplayAlert("error", ex.StackTrace, "ok");
                //await DisplayAlert("message", ex.Message, "ok");

            }

        }

        private void BtnSearch_Clicked(object sender, EventArgs e)
        {
            AttendanceList.ItemsSource = GetAttendanceList(SelectedDate);
        }

        public ObservableCollection<Attendance> GetAttendanceList(string searchDate = "")
        {
            try
            {

                if (!string.IsNullOrEmpty(searchDate) && !string.IsNullOrWhiteSpace(searchDate))
                {


                    var modifiedList = new List<Attendance_>();
                    var attendance_ = new Attendance_();
                    var currentUserAndSearchAttendance = attendanceFromFirebase.Where(a => a.Object.Date == searchDate && a.Object.Username == currentUser).ToList();
                    if (currentUserAndSearchAttendance.Count <= 0)
                    {
                        NoResultFoundLabel.IsVisible = true;
                    }
                    else
                    {
                        NoResultFoundLabel.IsVisible = false;

                    }
                    foreach (var item in currentUserAndSearchAttendance)
                    {
                        attendance_.Date = DateTime.Parse(item.Object.Date);
                        attendance_.Username = item.Object.Username;
                        attendance_.IsPresent = item.Object.IsPresent;
                        modifiedList.Add(attendance_);
                    }
                    var date = DateTime.Parse(searchDate);
                    var filteredSearch = modifiedList.Where(a => a.Date >= date).ToList();

                    Attendance = new ObservableCollection<Attendance>();
                    var entry = new Attendance();
                    foreach (var data in filteredSearch)
                    {

                        entry.Username = currentUser;
                        entry.IsPresent = data.IsPresent;
                        entry.Date = data.Date.ToShortDateString();

                        Attendance.Add(entry);
                    }
                    return Attendance;
                }
                else
                {
                    Attendance = new ObservableCollection<Attendance>();
                    var entry = new Attendance();
                    var currentUserAttendance = attendanceFromFirebase.Where(a => a.Object.Username == currentUser).ToList();
                    if (currentUserAttendance.Count <= 0)
                    {
                        NoResultFoundLabel.IsVisible = true;
                    }
                    else
                    {
                        NoResultFoundLabel.IsVisible = false;

                    }
                    foreach (var data in currentUserAttendance)
                    {
                        entry.Username = currentUser;
                        entry.IsPresent = data.Object.IsPresent;
                        entry.Date = data.Object.Date;

                        Attendance.Add(entry);
                    }
                    return Attendance;
                }
            }
            catch (Exception ex)
            {
                ToastMessage.Show(ex.Message);
                //DisplayAlert("error", ex.StackTrace, "ok");
                //DisplayAlert("message", ex.Message, "ok");
                return null;
            }        
        }

        private void StartDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            SelectedDate = e.NewDate.ToShortDateString();
            AttendanceList.ItemsSource = GetAttendanceList(SelectedDate);

        }

        private async void AttendanceList_Refreshing(object sender, EventArgs e)
        {
            AttendanceList.ItemsSource = await firebase.Child("Attendance").OnceAsync<Attendance>();
            AttendanceList.EndRefresh();
                
        }
    }
}