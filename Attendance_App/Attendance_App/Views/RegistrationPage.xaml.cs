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
    public partial class RegistrationPage : ContentPage
    {

        private FirebaseClient firebase;
        private IReadOnlyCollection<FirebaseObject<User>> usersList;

        public RegistrationPage()
        {
            InitializeComponent();
            firebase = new FirebaseClient(App.FirebaseUrl);
        }

        protected async override void OnAppearing()
        {
            try
            {
                usersList = await firebase.Child("Users").OnceAsync<User>();
                base.OnAppearing();
            }
            catch (Exception e)
            {
                ToastMessage.Show(e.Message);
                //await DisplayAlert("message", e.Message, "ok");
                //await DisplayAlert("error", e.StackTrace, "ok");
            }
        }



        private async void btnCreateUser_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(username_.Text) || string.IsNullOrEmpty(password_.Text) || string.IsNullOrEmpty(email_.Text))
                {
                    ToastMessage.Show("All fields are required...");
                }
                else if (!string.Equals(password_.Text, confirmPassword_.Text))
                {
                    ToastMessage.Show("Password mismatch");
                }
                else
                { 
                    var usernameAvailable = IsUsernameAvailable(username_.Text);
                    if (usernameAvailable)
                    {
                        var newUser = new User();
                        newUser.Id = string.Empty;
                        newUser.Username = username_.Text;
                        newUser.Password = password_.Text;
                        newUser.Email = email_.Text;

                        await firebase.Child("Users").PostAsync<User>(newUser);

                        ToastMessage.Show("new user created...");
                        await Navigation.PushModalAsync(new LoginPage());
                    }
                    else
                    {
                        ToastMessage.Show("Username is taken,try again..");
                    }

                }


            }
            catch (Exception ex)
            {
                ToastMessage.Show(ex.Message);
                //await DisplayAlert("error", ex.StackTrace, "ok");
                //await DisplayAlert("message", ex.Message, "ok");
            }

        }

        public bool IsUsernameAvailable(string username)
        {
            try
            {
                var userCount = usersList.Where(a => a.Object.Username == username).Count();
                if (userCount > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {

                ToastMessage.Show(e.Message);
                //DisplayAlert("error", e.StackTrace, "ok");
                //DisplayAlert("message", e.Message, "ok");
                return false;

            }

        }

    }
}