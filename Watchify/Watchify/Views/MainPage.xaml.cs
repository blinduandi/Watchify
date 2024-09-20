namespace Watchify.Views;
using RestSharp;
using System.Net;
using System.Text.Json;
using Watchify.Models;

public partial class MainPage : ContentPage
{
    public class MovieResponse
    {
        public List<Movie> Movie { get; set; }
    }

    string ip;
    public MainPage()
    {
        ip = Environment.GetEnvironmentVariable("IP");
        GenerateButtonsGenere();
        InitializeComponent();
        LoadMoviesAsync();
        AnimateCircle();
        
    }

    private async Task LoadMoviesAsync()
    {
        string apiUrl = "http://" + ip + ":3000/getNewMovies?id=2";

        var client = new RestClient(apiUrl);
        var request = new RestRequest("");

        var response = await client.ExecuteAsync(request);
        //await DisplayAlert("Movies Loaded", response.Content, "OK");

        try
        {
            // Configure JsonSerializerOptions for case-insensitive property names
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var universalList = JsonSerializer.Deserialize<List<Universal>>(response.Content, options);

            if (universalList != null)
            {
                foreach (var universal in universalList)
                {
                    if (universal.Movie != null)
                    {
                        //await DisplayAlert("Movie Loaded", universal.Movie.Title, "OK");

                        var imageButton = new ImageButton
                        {
                            Source = ImageSource.FromUri(new Uri(universal.Movie.Poster_Path)),
                            HeightRequest = 200,
                            WidthRequest = 140,
                            BindingContext = universal.Movie.ID
                        };

                        imageButton.Clicked += MoviePageRedirect;

                        StackNew.Children.Add(imageButton);
                    }
                    else
                    {
                        await DisplayAlert("Error", "Movie data is missing", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "Failed to load movies. The data is empty.", "OK");
            }
        }
        catch (JsonException jsonEx)
        {
            await DisplayAlert("Error", $"JSON deserialization failed: {jsonEx.Message}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }




    private async void GenerateButtonsGenere()
    {
        var options = new RestClientOptions("http://"+ip+":3000/getAllGenres");
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

                var genres = JsonSerializer.Deserialize<List<string>>(jsonResponse);

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

                    genreStackLayout.Children.Add(button);
                }
            }
            else
            {
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

    private async void MoviePageRedirect(object sender, EventArgs e)
    {
        var imageButton = sender as ImageButton;
        int movieId = (int)imageButton.BindingContext;

        await Shell.Current.GoToAsync($"moviepage?movieId={movieId}");

    }
}