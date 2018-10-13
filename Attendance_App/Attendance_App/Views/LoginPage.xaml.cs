using Attendance_App.Models;
using Attendance_App.Utilities;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Attendance_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
      
        private FirebaseClient firebase;
        private IReadOnlyCollection<FirebaseObject<User>> usersList;

        private bool isTapped { get; set; }
        private bool isRemembered { get; set; }
        private string username { get; set; }
        private string password { get; set; }
        private string url { get; set; }


        public LoginPage()
        {

            InitializeComponent();
            firebase = new FirebaseClient(App.FirebaseUrl);

            animationView.IsVisible = false;


        }

        protected async override void OnAppearing()
        {
            usersList = await firebase.Child("Users").OnceAsync<User>();

           
            _rememberMe.Text = "Remember me!";
            _rememberMe.TextColor = Color.Blue;
            isTapped = true;
            base.OnAppearing();
        }



        private async Task btnSignIn_Clicked(object sender, EventArgs e)
        {
            if (isTapped)
            {
                isRemembered = true;
                //Application.Current.Properties["username"] = username_.Text;
                Application.Current.Properties["currentUser"] = username_.Text;
                Application.Current.Properties["rememberMe"] = isRemembered;
                await Application.Current.SavePropertiesAsync();
            }
            else
            {
                isRemembered = false;
                Application.Current.Properties["rememberMe"] = isRemembered;
                Application.Current.Properties["currentUser"] = username_.Text;

                await Application.Current.SavePropertiesAsync();

            }
           
            if (!string.IsNullOrEmpty(username_.Text) && !string.IsNullOrEmpty(password_.Text))
            {
                animationView.IsVisible = true;
                var auth = isAuthenticated(username_.Text, password_.Text);
                if (auth)
                {
                    animationView.IsVisible = false;
                    ToastMessage.Show("Login successful..");
                    await Navigation.PushModalAsync(new MainPage());
                }
                else
                {
                    animationView.IsVisible = false;
                    ToastMessage.Show("Invalid username or password...");
                }
            }


            
        }

        private void rememberMe_Tapped(object sender, EventArgs e)
        {
            isTapped = !isTapped;
            if (isTapped)
            {
                _rememberMe.Text = "Remember me!";
                _rememberMe.TextColor = Color.Blue;
                isRemembered = true;
            }
            else
            {
                _rememberMe.Text = "Never mind!";
                _rememberMe.TextColor = Color.SkyBlue;
                isRemembered = false;
            }

        }

        private void newUser_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RegistrationPage());
        }

        public bool isAuthenticated(string username, string password)
        {
            try
            {
                var isAuth = usersList.Where(a => a.Object.Username == username && a.Object.Password == password).Count();
                if (isAuth > 0)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception e)
            {
                ToastMessage.Show(e.Message);
                //DisplayAlert("message", e.Message, "ok");
                //DisplayAlert("error", e.StackTrace, "ok");

                return false;
            }
          
        }

        private void mainPage_Tapped(object sender, EventArgs e)
        {
           
            Navigation.PushModalAsync(new MainPage());
        }
    }
}