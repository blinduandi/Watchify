using Firebase.Auth;
using Microsoft.Rest;
using RestSharp;
using System.Security.Cryptography;
using System.Text;

namespace Watchify.Views;

public partial class SignUpPage : ContentPage
{
    private readonly FirebaseAuthClient _authClient = MauiProgram.CreateMauiApp().Services.GetService<FirebaseAuthClient>();
    string ip;
    private string userID;
    public int start = 0;
    public int stop = 10;
    public int step = 10;
    public SignUpPage()
    {
        InitializeComponent();
        ip = Environment.GetEnvironmentVariable("IP");
        Shell.SetNavBarIsVisible(this, false);
    }

    private async void GoToSignIn(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///signin");
    }
    

    public static int GetUserIdAsInteger(string userId)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userId));

            int hashValue = BitConverter.ToInt32(bytes, 0);

            return Math.Abs(hashValue);
        }
    }
    private async void RecomandandionRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/recommandation");
    }
    private async void OnSignUpButtonClicked(object sender, EventArgs e)
    {
        try
        {
            string email = emailEntry.Text;
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;

            // Basic validation
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "Ok");
                return;
            }

            // Create a new user with Firebase Authentication
            UserCredential userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password, username);

            string userId = userCredential.User.Uid;
            string emaill = userCredential.User.Info.Email;
            string displayName = userCredential.User.Info.DisplayName;

            string apiUrl = "http://" + ip + ":3000/addUser";

            // Create a new RestClient
            var client = new RestClient(apiUrl);
            var request = new RestRequest("", Method.Post);

            // Convert User ID to integer and create user data object
            var userData = new
            {
                id = GetUserIdAsInteger(userCredential.User.Uid),
                email = emaill,
                nickname = displayName
            };

            request.AddJsonBody(userData);

            // Execute the request and handle the response
            var response = await client.ExecuteAsync(request);

            await SecureStorage.SetAsync("userLoggedIn", "true");
            await SecureStorage.SetAsync("UserID", GetUserIdAsInteger(userCredential.User.Uid).ToString());
            // Navigate to a different page (adjust the route as needed)
            await Shell.Current.GoToAsync("///main");
        }
        catch (FirebaseAuthException firebaseEx)
        {
            await DisplayAlert("Firebase Error", $"Firebase Authentication error: {firebaseEx.Message}", "Ok");
        }
        catch (RestException restEx)
        {
            await DisplayAlert("REST Error", $"REST API error: {restEx.Message}", "Ok");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to sign up: {ex.Message}", "Ok");
        }
    }

}
