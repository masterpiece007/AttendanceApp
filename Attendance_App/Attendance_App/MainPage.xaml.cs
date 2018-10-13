using Attendance_App.Models;
using Attendance_App.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace Attendance_App
{
    public partial class MainPage : MasterDetailPage
    {
        private string currentUser;

        public List<MasterPageItem> masterPageItems { get; set; }
        public MainPage()
        {
            InitializeComponent();

            currentUser = Application.Current.Properties["currentUser"].ToString();

            masterPageItems = new List<MasterPageItem>();
            
            var markAttendance = new MasterPageItem()
            {
                Title = "Mark Attendance",
                Icon = "icons8checkfile24.png",
                TargetType = typeof(AttendancePage)
            };
            var attendanceHistory = new MasterPageItem()
            {
                Title = "History",
                Icon = "icons8timemachine24.png",
                TargetType = typeof(AttendanceHistoryPage)
            };
            var login = new MasterPageItem()
            {
                Title = "Account",
                Icon = "icons8password24w.png",
                TargetType = typeof(LoginPage)
            };
          


            masterPageItems.Add(markAttendance);
            masterPageItems.Add(attendanceHistory);
            masterPageItems.Add(login);

            navigationDrawerList.ItemsSource = masterPageItems;
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(AttendancePage)));
            this.BindingContext = new
            {
                Header = "Logged in as " + currentUser
,
                Image = "",
                Footer = "Logged in as " + currentUser
            };

        }

        protected override void OnAppearing()
        {
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromRgb(42, 63, 84);

            base.OnAppearing();
        }

        private void navigationDrawerList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;

            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }
    }
}
