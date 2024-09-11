using Firebase.Auth;

namespace Watchify.Views;

public partial class SignUpPage : ContentPage
{
    private readonly FirebaseAuthClient _authClient;

    public SignUpPage(FirebaseAuthClient authClient)
    {
        _authClient = authClient;
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }

    private async void OnSignUpButtonClicked(object sender, EventArgs e)
    {
        try
        {
            string email = emailEntry.Text;
            string username = usernameEntry.Text;
           // string phoneNumber = phoneEntry.Text;
            string password = passwordEntry.Text;

            // Basic validation
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) )
            {
                await DisplayAlert("Error", "Please fill in all fields.", "Ok");
                return;
            }

            // Create a new user with Firebase Authentication
            UserCredential userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);

            // Optionally, update user profile with display name
            //await userCredential.User.UpdateProfileAsync(new UserProfile
            //{
            //    DisplayName = username
            //});

            await DisplayAlert("Success", "Successfully signed up!", "Ok");

            // Navigate to a different page (adjust the route as needed)
            await Shell.Current.GoToAsync("//SignIn");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to sign up: {ex.Message}", "Ok");
        }
    }
}
