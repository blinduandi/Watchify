namespace Watchify.Views;
using RestSharp;
using System.Net;
using System.Text.Json;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        GenerateButtonsGenere();
        InitializeComponent();
        AnimateCircle();
        
    }

    private async void GenerateButtonsGenere()
    {
        // API URL for genres
        var options = new RestClientOptions("http://3.66.76.45:3000/getAllGenres");
        var client = new RestClient(options);
        var request = new RestRequest("");
        request.AddHeader("accept", "application/json");

        try
        {
            var response = await client.GetAsync(request);
            Console.WriteLine(response);
            if (response.IsSuccessful)
            {

                string jsonResponse = response.Content;

                // Deserialize the JSON response into a list of genres
                var genres = JsonSerializer.Deserialize<List<string>>(jsonResponse);

                // Add buttons for each genre to the StackLayout
                foreach (var genre in genres)
                {
                    var button = new Button
                    {
                        Text = genre,
                        BackgroundColor = Colors.Black,
                        TextColor = Colors.White,
                        BorderColor = Colors.White,
                        BorderWidth = 0.5,
                        HeightRequest = 50
                    };

                    // Add the button to the StackLayout
                    genreStackLayout.Children.Add(button);
                }
            }
            else
            {
                // Display the status code and error message if the request fails
                await DisplayAlert("Error", $"Status Code: {response.StatusCode}\nError: {response.ErrorMessage}", "OK");
            }
        }
        catch (HttpRequestException httpEx)
        {
            await DisplayAlert("Error", $"HTTP Request Error: {httpEx.Message}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void AnimateCircle()
    {
        while (true)
        {
            await GradientCircle.ScaleTo(1.2, 1000, Easing.Linear);
            await GradientCircle.ScaleTo(1.0, 1000, Easing.Linear);
        }
    }

    private async void SearchClick(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("/search");
    }

    private async void ProfilePage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("/profile");
    }

    private async void MoviePage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("/movie");
    }

}