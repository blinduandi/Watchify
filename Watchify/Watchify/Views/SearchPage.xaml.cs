using Firebase.Auth;
using Microsoft.Maui.Graphics.Text;
using RestSharp;
using System.Diagnostics.Metrics;
using System.Text.Json;
using Watchify.Models;

namespace Watchify.Views;

public partial class SearchPage : ContentPage
{
    string ip;
    private string userID;
    public int start = 100;
    public int stop = 10;
    public int step = 10;
    private List<Universal> movieList;
    public SearchPage()
	{
        InitializeComponent();
        ip = Environment.GetEnvironmentVariable("IP");
        LoadMoreBtn.IsVisible = false;
        LoadTrendingMoviesAsync();

    }
    private async void BackHome(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    private async void AllResults(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("/list");
    }

    CancellationTokenSource debounceTokenSource = null;

    private async void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        debounceTokenSource?.Cancel(); // Cancel previous request if a new one comes in quickly
        debounceTokenSource = new CancellationTokenSource();
        string searchText = e.NewTextValue;
        await Task.Delay(300); // Wait for 300ms
        if (!debounceTokenSource.Token.IsCancellationRequested)
        {
            await LoadMoviesAsync(searchText);
        }
    }
    private async Task LoadMoviesAsync(string searchText)
    {
        //ResultsStack.Children.Clear();

        await Task.Run(async () =>
        {
            string apiUrl = "http://" + ip + ":3000/searchByName?title=" + Uri.EscapeDataString(searchText) + $"&start=0&stop=6";
            var client = new RestClient(apiUrl);
            var request = new RestRequest();
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                if (response.Content != "null")
                {
                    movieList = JsonSerializer.Deserialize<List<Universal>>(response.Content, options);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ResultsStack.Children.Clear();
                        LoadMoreBtn.IsVisible = true;

                        // Batch UI updates to reduce the number of frame updates
                        var batch = new StackLayout(); // Temporary container for batching updates

                        foreach (var movie in movieList.Take(3))
                        {
                            var movieItem = CreateMovieItem(movie);
                            batch.Children.Add(movieItem); // Add items to the batch
                        }

                        ResultsStack.Children.Add(batch); // Add all items to ResultsStack at once
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() => LoadMoreBtn.IsVisible = false);
                }
            }
            else
            {
                Console.WriteLine($"Failed to load movies. Status code: {response.StatusCode}");
            }
        });
    }


    private StackLayout CreateMovieItem(Universal movie)
    {
        var stackLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            BindingContext = movie.Movie.ID,
        };

        var tapGesture = new TapGestureRecognizer
        {
            CommandParameter = movie.Movie.Title
        };
        tapGesture.Tapped += (s, e) => MoviePageRedirect(s, e);
        stackLayout.GestureRecognizers.Add(tapGesture);

        var imageButton = new ImageButton
        {
            Source = movie.Movie.Poster_Path,
            HeightRequest = 100,
            WidthRequest = 80
        };

        var verticalStack = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
        };

        var titleLabel = new Label { Text = movie.Movie.Title, FontSize = 16, WidthRequest = 150, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 };
        var yearLabel = new Label { Text = movie.Movie.Release_Date, FontSize = 14 };
        var genreLabel = new Label { TextColor = Color.FromRgb(128, 128, 128), Text = string.Join(",action ") };
        var moreButton = new Button
        {
            Text = "See more",
            TextColor = Color.FromRgb(255, 165, 0), // RGB for Orange
            BackgroundColor = Color.FromRgba(0, 0, 0, 0),
            HorizontalOptions = LayoutOptions.Start
        };

        verticalStack.Children.Add(titleLabel);
        verticalStack.Children.Add(yearLabel);
        verticalStack.Children.Add(genreLabel);
        verticalStack.Children.Add(moreButton);

        var ratingLabel = new Label
        {
            Text = movie.Movie.Rating.HasValue ? movie.Movie.Rating.Value.ToString("F1") : "N/A",
            TextColor = Colors.Green,
            HorizontalOptions = LayoutOptions.EndAndExpand,
            FontSize = 16
        };

        stackLayout.Children.Add(imageButton);
        stackLayout.Children.Add(verticalStack);
        stackLayout.Children.Add(ratingLabel);

        return stackLayout;
    }
    private async Task LoadTrendingMoviesAsync()
    {
        userID = await SecureStorage.GetAsync("UserID");

        string apiUrl = $"http://{ip}:3000/getTrendingMovies?id={userID}&start={start}&stop={stop}";

        var client = new RestClient(apiUrl);
        var request = new RestRequest("");

        var response = await client.ExecuteAsync(request);

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Universal> universalList = new List<Universal>();

            universalList = JsonSerializer.Deserialize<List<Universal>>(response.Content, options);


            foreach (var universal in universalList)
            {
                // Create a StackLayout
                var stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    BindingContext = universal.Movie.ID,
                    Spacing = 10
                };

                // Add a TapGestureRecognizer to the StackLayout
                var tapGesture = new TapGestureRecognizer
                {
                    CommandParameter = universal.Movie.Title
                };
                tapGesture.Tapped += (s, e) => MoviePageRedirect(s, e);
                stackLayout.GestureRecognizers.Add(tapGesture);

                // Create the Image element
                var image = new Image
                {
                    Source = ImageSource.FromUri(new Uri(universal.Movie.Poster_Path)),
                    BindingContext = universal.Movie.ID,
                    HeightRequest = 200
                };

                // Create the Label element
                var label = new Label
                {
                    Text = universal.Movie.Title,
                    HorizontalOptions = LayoutOptions.Start,
                    BindingContext = universal.Movie.ID,
                    MaximumWidthRequest = 100
                };

                // Add the Image and Label to the StackLayout
                stackLayout.Children.Add(image);
                stackLayout.Children.Add(label);

                // Add the StackLayout to the parent layout (StackNew in your case)
                StackTrending.Children.Add(stackLayout);
            }

        }
        catch (Exception ex)
        {
            //await DisplayAlert("Error", $"An error occurred load: {ex.Message}", "OK");
        }
    }
    private async void MoviePageRedirect(object sender, EventArgs e)
    {
        var imageButton = sender as StackLayout;
        int movieId = (int)imageButton.BindingContext;
        await Shell.Current.GoToAsync($"///main/search/movie?movieId={movieId}");

    }
    private async void LoadMore(object sender, EventArgs e)
    {
        var universalJson = JsonSerializer.Serialize(movieList);
        await Shell.Current.GoToAsync($"///main/search/list?pageType=search&pageMessage={entry.Text}&universal=null");
    }
    private async void RecomandandionRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/recommandation");
    }
    private async void SeeAllWatched(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/list?pageType=watch_later&pageMessage=none&universal=null");
    }
    private async void HomeRedirect(object sender, EventArgs e)
    {
        //await Shell.Current.GoToAsync("..");
        await Shell.Current.GoToAsync("///main");
    }

}