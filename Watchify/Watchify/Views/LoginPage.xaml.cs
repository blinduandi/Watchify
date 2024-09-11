using Firebase.Auth;


namespace Watchify.Views;

public partial class LoginPage : ContentPage
{
    private readonly FirebaseAuthClient _authClient;
   // private readonly CurrentUserStore _currentUserStore;


    public LoginPage(FirebaseAuthClient authClient)
	{
        _authClient = authClient;
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
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

            await DisplayAlert("Success", "Successfully signed in!", "Ok");

            // Navigate to a different page (adjust the route as needed)
            await Shell.Current.GoToAsync("//ReportBug");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to sign in: {ex.Message}", "Ok");
        }
    }

}