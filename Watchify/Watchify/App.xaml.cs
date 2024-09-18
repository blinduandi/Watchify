using Firebase.Auth;
using Microsoft.Extensions.DependencyInjection;
using Watchify.Features.SignUp;
using Watchify.Views;

namespace Watchify
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            var isLoggedIn = SecureStorage.GetAsync("userLoggedIn").Result;

            if (isLoggedIn == "true")
            {
                Shell.Current.GoToAsync("///main");
            }
            else
            {
                Shell.Current.GoToAsync("///signin"); 
            }
        }
    }
}
