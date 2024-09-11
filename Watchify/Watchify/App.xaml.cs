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
            var authClient = MauiProgram.CreateMauiApp().Services.GetService<FirebaseAuthClient>();
            MainPage = new NavigationPage(new Watchify.Views.MainPage());
            
        }
    }
}
