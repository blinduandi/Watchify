using Firebase.Auth;


namespace Watchify.Views;

public partial class LoginPage : ContentPage
{
    private readonly FirebaseAuthClient _authClient = MauiProgram.CreateMauiApp().Services.GetService<FirebaseAuthClient>();
    // private readonly CurrentUserStore _currentUserStore;


    public LoginPage()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }

    private async void GoToSignUp(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///signup");
    }

    //private async void OnLoginGoogleButtonClicked(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        //var googleSignInService = DependencyService.Get<IGoogleSignInService>();
    //        //await googleSignInService.SignInWithGoogleAsync();
    //        UserCredential userCredential = await _authClient

    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Error", $"Failed to sign in: {ex.Message}", "Ok");
    //    }
    //}

    //private IObservable<Unit> OnLoginGoogleButtonClicked()
    //{
    //    return _authClient.SignInWithGoogle();
    //}


    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        try
        {
            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please fill in both fields.", "Ok");
                return;
            }

            // Authenticate with Firebase
            UserCredential userCredential = await _authClient.SignInWithEmailAndPasswordAsync(email, password);

            // Save login state
            await SecureStorage.SetAsync("userLoggedIn", "true");

            await DisplayAlert("Success", "Successfully signed in!", "Ok");
            await Shell.Current.GoToAsync("///main");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to sign in: {ex.Message}", "Ok");
        }
    }


}