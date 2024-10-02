using Firebase.Auth;
using Microsoft.Maui.Controls;
using RestSharp;
using System.Text.Json;
using Watchify.Models;

namespace Watchify.Views;

[QueryProperty(nameof(pageType), "pageType")]
[QueryProperty(nameof(pageMessage), "pageMessage")]
[QueryProperty(nameof(UniversalJson), "universal")]

public partial class ListMovies : ContentPage
{ 
    int loadMore = 0;
    public int start = 0;
    public int stop = 15;
    public int step = 15;
    public string UniversalJson { get; set; }
    public List<Universal> universal;
    public string pageType { get; set; }
    public string pageMessage { get; set; }
    string ip;
    private string userID;
    public ListMovies()
	{
        InitializeComponent();
        userID = SecureStorage.GetAsync("UserID").Result;
        ip = Environment.GetEnvironmentVariable("IP");
	}
    private async void RecomandandionRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/recommandation");
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        universal = JsonSerializer.Deserialize<List<Universal>>(Uri.UnescapeDataString(UniversalJson));


        if (!string.IsNullOrEmpty(pageType))
        {
            if (pageType == "search")
            {
                if (!string.IsNullOrEmpty(pageMessage)) listMoviesTitle.Text = "Search: " + pageMessage;
                else listMoviesTitle.Text = "Search List";
                LoadMoviesSearch(pageMessage);
            }
            else if(pageType == "newMovies")
            {
                listMoviesTitle.Text = "New Movies";
                LoadNewMoviesAsync();
            }
            else if(pageType == "trending")
            {
                listMoviesTitle.Text = "Trending Movies";
                LoadTrendingMoviesAsync();
            }
            else if(pageType == "genre")
            {
                if (!string.IsNullOrEmpty(pageMessage)) listMoviesTitle.Text = "Genre: " + pageMessage;
                else listMoviesTitle.Text = "List By Genre";
                LoadMoviesByGenre(pageMessage);
            }
            else if(pageType == "liked")
            {
                listMoviesTitle.Text = "Liked Movies";
                LoadMoviesLiked();
            }
            else if (pageType == "watch_later")
            {
                listMoviesTitle.Text = "Watch Later";
                LoadMoviesWatched();
            }
        }
    }

    private async Task LoadMoviesWatched()
    {
        userID = await SecureStorage.GetAsync("UserID");

        string apiUrl = $"http://{ip}:3000/getWatchLaterMovies?id={userID}&start={start}&stop={step}";
        //start = stop;
        start += step;

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
            LoadMovies(universalList);

        }
        catch (Exception ex)
        {
            //await DisplayAlert("Error", $"An error occurred load: {ex.Message}", "OK");
        }
    }
    private async Task LoadMoviesLiked()
    {
        userID = await SecureStorage.GetAsync("UserID");

        string apiUrl = $"http://{ip}:3000/getLikedMovies?id={userID}&start={start}&stop={stop}";
        start += step;

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
            LoadMovies(universalList);


        }
        catch (Exception ex)
        {
            //await DisplayAlert("Error", $"An error occurred load: {ex.Message}", "OK");
        }
    }
    public async void LoadMovies(List<Universal> movieList)
    {
        if (movieList == null || !movieList.Any())
        {
            var messageLabel = new Label
            {
                Text = "No more movies available.",
                FontSize = 20,
                TextColor = Colors.Gray,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            LoadMoreButton.IsVisible = false;
            movieScrollView.Children.Add(messageLabel);
            return; 
        }

        var parentStack = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            Spacing = 20
        };

        foreach (var movie in movieList)
        {
            if (movie == null || movie.Movie == null)
            {
                continue; 
            }

            var movieStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BindingContext = movie.Movie.ID
            };

            var tapGestureRecognizer = new TapGestureRecognizer
            {
                CommandParameter = movie.Movie.Title ?? "Unknown Title"  
            };
            tapGestureRecognizer.Tapped += MovieTapped;

            var posterImage = new ImageButton
            {
                Source = !string.IsNullOrEmpty(movie.Movie.Poster_Path) ? movie.Movie.Poster_Path : "default_poster.png",
                HeightRequest = 100,
                WidthRequest = 80
            };

            var detailsStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical
            };

            var movieTitleLabel = new Label
            {
                FontSize = 16,
                Text = !string.IsNullOrEmpty(movie.Movie.Title) ? movie.Movie.Title : "Unknown Title",
                WidthRequest = 200,
                LineBreakMode = LineBreakMode.TailTruncation,
                MaxLines = 1
            };

            var releaseYearLabel = new Label
            {
                Text = movie.Movie.Release_Date!= null ? movie.Movie.Release_Date.ToString() : "Unknown Year"
            };

            var genresLabel = new Label
            {
                Text = (movie.Genres != null && movie.Genres.Genre != null && movie.Genres.Genre.Count > 0)
                    ? $"USA - {movie.Genres.Genre[0].Name}"
                    : "USA - Unknown Genre",
                TextColor = Colors.Gray
            };

            var seeMoreButton = new Button
            {
                Text = "See more",
                BackgroundColor = Colors.Black,
                TextColor = Colors.Orange,
                HorizontalOptions = LayoutOptions.Start,
                Padding = new Thickness(0),
                Margin = new Thickness(0)
            };

            detailsStack.Children.Add(movieTitleLabel);
            detailsStack.Children.Add(releaseYearLabel);
            detailsStack.Children.Add(genresLabel);
            detailsStack.Children.Add(seeMoreButton);

            var ratingLabel = new Label
            {
                Text = movie.Movie.Rating.HasValue ? movie.Movie.Rating.Value.ToString("F1") : "N/A",
                TextColor = Colors.Green,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontSize = 16
            };

            movieStack.Children.Add(posterImage);
            movieStack.Children.Add(detailsStack);
            movieStack.Children.Add(ratingLabel);

            movieStack.GestureRecognizers.Add(tapGestureRecognizer);

            parentStack.Children.Add(movieStack);
        }

        movieScrollView.Children.Add(parentStack);
    }
    private async Task LoadMoviesByGenre(string genre)
    {

        string apiUrl = $"http://{ip}:3000/getMoviesByGenre?genre=" + Uri.EscapeDataString(genre) + $"&start={start}&stop={step}";
        start += step;

        var client = new RestClient(apiUrl);
        var request = new RestRequest();

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (response.Content != "null")
            {

                var movieList = JsonSerializer.Deserialize<List<Universal>>(response.Content, options);
                LoadMovies(movieList);

            }


        }
        else
        {
            Console.WriteLine($"Failed to load movies. Status code: {response.StatusCode}");
        }
    }
    private async Task LoadMoviesSearch(string searchText)
    {

        string apiUrl = $"http://{ip}:3000/searchByName?title=" + Uri.EscapeDataString(searchText) + $"&start={start}&stop={step}";
        start += step;

        var client = new RestClient(apiUrl);
        var request = new RestRequest();

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (response.Content != "null")
            {

                var movieList = JsonSerializer.Deserialize<List<Universal>>(response.Content, options);
                LoadMovies(movieList);

            }


        }
        else
        {
            Console.WriteLine($"Failed to load movies. Status code: {response.StatusCode}");
        }
    }

    private async Task LoadNewMoviesAsync()
    {
        string apiUrl = $"http://{ip}:3000/getNewMovies?id={userID}&start={start}&stop={step}";
        //start = stop;
        start += step;

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

            LoadMovies(universalList);

        }
        catch (Exception ex)
        {
            //await DisplayAlert("Error", $"An error occurred load: {ex.Message}", "OK");
        }
    }
    private async Task LoadTrendingMoviesAsync()
    {
        string apiUrl = $"http://{ip}:3000/getTrendingMovies?id={userID}&start={start}&stop={step}";
        start+= step;
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
            LoadMovies(universalList);
        }
        catch (Exception ex)
        {
            //await DisplayAlert("Error", $"An error occurred load: {ex.Message}", "OK");
        }
    }
    private async void MovieTapped(object sender, EventArgs e)
    {
        var imageButton = sender as StackLayout;
        int movieId = (int)imageButton.BindingContext;
        await Shell.Current.GoToAsync($"/movie?movieId={movieId}");
    }
    private async void LoadMoreBtn(object sender, EventArgs e)
    {
        if (pageType == "search")
        {

            LoadMoviesSearch(pageMessage);
        }
        else if (pageType == "newMovies")
        {
            LoadNewMoviesAsync();
        }
        else if (pageType == "trending")
        {
            LoadTrendingMoviesAsync();
        }
        else if (pageType == "genre")
        {
            LoadMoviesByGenre(pageMessage);
        }
        else if (pageType == "liked")
        {
            LoadMoviesLiked();
        }
        else if (pageType == "watch_later")
        {
            listMoviesTitle.Text = "Watch Later";
            LoadMoviesWatched();
        }
    }
    private async void SeeAllWatched(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/list?pageType=watch_later&pageMessage=none&universal=null");
    }
    private async void BackToSearch(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    private async void HomeRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///main");
    }
}