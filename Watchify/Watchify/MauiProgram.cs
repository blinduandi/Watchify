using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.Logging;
using Watchify.Features.SignUp;
using Watchify.Pages;
//using UIKit;

namespace Watchify
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<SignUpFormViewModel>();
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<SignUpView>(
                s => new SignUpView(s.GetRequiredService<SignUpViewModel>()));

            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyDLAZrnW93jfrH8xDBDy8nj-i83zWMEejk",
                AuthDomain = "watchify-f7cb6.firebaseapp.com",
                Providers = new FirebaseAuthProvider[] {
                    new EmailProvider(),
                    new GoogleProvider(),
                    new FacebookProvider(),
                    new AppleProvider()
                }
            }));

            


            return builder.Build();
        }
    }
}
