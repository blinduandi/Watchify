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

            //MainPage = new AppShell();
            var authClient = MauiProgram.CreateMauiApp().Services.GetService<FirebaseAuthClient>();

            //MainPage = new NavigationPage(new LoginPage(authClient));
            MainPage = new NavigationPage(new SignUpPage(authClient));
            //MainPage = new NavigationPage(new SignUpFormView());
        }
    }
}
