using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.Logging;
using Watchify.Features.SignUp;
using Watchify.Pages;
using Watchify.Views;
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
                    fonts.AddFont("SUSE-Bold.ttf", "SuseBold");
                    fonts.AddFont("SUSE-ExtraBold.ttf", "SuseExtraBold");
                    fonts.AddFont("SUSE-SemiBold.ttf", "SuseSemiBold");
                    fonts.AddFont("SUSE-Medium.ttf", "SuseMedium");
                    fonts.AddFont("SUSE-Regular.ttf", "SuseRegular");
                    fonts.AddFont("SUSE-Thin.ttf", "SuseThin");
                    fonts.AddFont("SUSE-Light.ttf", "SuseLight");
                    fonts.AddFont("SUSE-ExtraLight.ttf", "SuseExtraLight");
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

            builder.Services.AddSingleton<MainPage>();
           // builder.Services.AddSingleton<MoviePage>();
           // builder.Services.AddSingleton<SearchPage>();
            builder.Services.AddTransient<SearchPage>();
            builder.Services.AddTransient<MoviePage>();

            Environment.SetEnvironmentVariable("IP", "172.20.10.14");

            return builder.Build();
        }
    }
}
