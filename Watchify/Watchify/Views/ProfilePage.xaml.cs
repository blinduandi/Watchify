using Firebase.Auth;

namespace Watchify.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

    private async void ToHome(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    private async void HomeRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///main");
    }
    private async Task RemoveUserFromStorageAsync()
    {
        try
        {
            // Remove the userLoggedIn and UserID from SecureStorage
            await SecureStorage.SetAsync("userLoggedIn", string.Empty);
            await SecureStorage.SetAsync("UserID", string.Empty);


            // Optionally, display a success message
            await DisplayAlert("Success", "User data removed successfully.", "Ok");
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., if the device does not support SecureStorage)
            await DisplayAlert("Error", $"Failed to remove user data: {ex.Message}", "Ok");
        }
    }
    private async void RecomandandionRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/recommandation");
    }
    private async void SeeAllWatched(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/list?pageType=watch_later&pageMessage=none&universal=null");
    }
    private async void LogOut(object sender, EventArgs e)
    {
        await RemoveUserFromStorageAsync();

        await Shell.Current.GoToAsync($"///signin");
    }
}