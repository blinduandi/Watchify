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

        }
    }
}
