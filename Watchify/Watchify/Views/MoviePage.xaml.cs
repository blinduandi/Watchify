using RestSharp;
using System.Text.Json;
using Watchify.Models;

namespace Watchify.Views;

[QueryProperty(nameof(MovieId), "movieId")]

public partial class MoviePage : ContentPage
{
    public string MovieId { get; set; }
    string ip;
    string userID;
    public bool watchLate = false;
    public bool likeMovie = false;
    public MoviePage()
    {
        ip = Environment.GetEnvironmentVariable("IP");

        InitializeComponent();
        LoadActionButtons();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Ensure movieId is not null before accessing it
        if (!string.IsNullOrEmpty(MovieId))
        {  
            LoadMovieDetails(MovieId);
            LoadSimilarMovies(MovieId);

        }
        else
        {
            Console.WriteLine("movieId is null");
        }
    }
    private async void RecomandandionRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/recommandation");
    }

    private async void LoadActionButtons()
    {
        try
        {
            userID = await SecureStorage.GetAsync("UserID");
            string apiUrl = $"http://{ip}:3000/getUserInteractions?id={userID}&movieId={MovieId}&interaction=liked";
            var client = new RestClient(apiUrl);
            var request = new RestRequest();

            // Execute the request and get the response
            var response = await client.ExecuteAsync(request);

            // Deserialize the response content into a list using System.Text.Json
            var interactions = JsonSerializer.Deserialize<List<string>>(response.Content);

            // Check if interactions list is null or empty
            if (interactions == null || !interactions.Any())
            {
                // Handle the case where the list is null or empty (e.g., no interactions found)
                LikeBtn.Source = "heart_regular.png";
                likeMovie = false;
                watchBtn.Source = "bookmark_regular.png";
                watchLate = false;
                return;
            }

            // Handle 'liked' interaction
            if (interactions.Contains("liked"))
            {
                LikeBtn.Source = "heart_solid.png";
                likeMovie = true;
            }
            else
            {
                LikeBtn.Source = "heart_regular.png";
                likeMovie = false;
            }

            // Handle 'watch_later' interaction
            if (interactions.Contains("watch_later"))
            {
                watchBtn.Source = "bookmark_solid.png";
                watchLate = true;
            }
            else
            {
                watchBtn.Source = "bookmark_regular.png";
                watchLate = false;
            }
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"Network error: {httpEx.Message}");
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"Deserialization error: {jsonEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async void LoadSimilarMovies(string id)
    {
        //await DisplayAlert("Response Content",MovieId, "OK");

        string apiUrl = $"http://{ip}:3000/getSimilar?movieId={id}";
        //start = stop;
        //stop += step;
        var client = new RestClient(apiUrl);
        var request = new RestRequest("");

        var response = await client.ExecuteAsync(request);

        // Debugging: Display or log the response content
        //await DisplayAlert("Response Content", response.Content, "OK");
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
                if (universal.Movie == null || string.IsNullOrEmpty(universal.Movie.Poster_Path))
                {
                    continue;
                }
                var stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    BindingContext = universal.Movie.ID,
                    Spacing = 10
                };

                var tapGesture = new TapGestureRecognizer
                {
                    CommandParameter = universal.Movie.Title
                };
                tapGesture.Tapped += (s, e) => MoviePageRedirect(s, e);
                stackLayout.GestureRecognizers.Add(tapGesture);

                var image = new Image
                {
                    Source = ImageSource.FromUri(new Uri(universal.Movie.Poster_Path)),
                    HeightRequest = 200
                };

                var label = new Label
                {
                    Text = universal.Movie.Title,
                    HorizontalOptions = LayoutOptions.Start,
                    MaximumWidthRequest = 100
                };

                stackLayout.Children.Add(image);
                stackLayout.Children.Add(label);

                stackSimilar.Children.Add(stackLayout);
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred load(new): {ex.Message}", "OK");
        }
    }
    private async void MoviePageRedirect(object sender, EventArgs e)
    {
        var imageButton = sender as StackLayout;
        int movieId = (int)imageButton.BindingContext;
        await Shell.Current.GoToAsync($"/movie?movieId={movieId}");

    }
    private async Task ManageLike()
    {
        userID = await SecureStorage.GetAsync("UserID");

        if (likeMovie == true)
        {
            string apiUrl = $"http://{ip}:3000/deleteUserInteraction?id={userID}&movieId={MovieId}&interaction=liked";

            var client = new RestClient(apiUrl);
            var request = new RestRequest();

            var response = await client.ExecuteAsync(request);
            LikeBtn.Source = "heart_regular.png";
            likeMovie = false;
        }
        else
        {

            string apiUrl = $"http://{ip}:3000/addUserInteraction?id={userID}&movieId={MovieId}&interaction=liked";

            var client = new RestClient(apiUrl);
            var request = new RestRequest();

            var response = await client.ExecuteAsync(request);
            LikeBtn.Source = "heart_solid.png";
            likeMovie = true;
        }

    }
    private async Task ManageWatchLater(string movieId)
    {
        userID = await SecureStorage.GetAsync("UserID");

        if (watchLate == true)
        {
            string apiUrl = $"http://{ip}:3000/deleteUserInteraction?id={userID}&movieId={movieId}&interaction=watch_later";

            var client = new RestClient(apiUrl);
            var request = new RestRequest();

            var response = await client.ExecuteAsync(request);
            watchBtn.Source = "bookmark_regular.png";
            watchLate = false;
        }
        else
        {

            string apiUrl = $"http://{ip}:3000/addUserInteraction?id={userID}&movieId={MovieId}&interaction=watch_later";

            var client = new RestClient(apiUrl);
            var request = new RestRequest();

            var response = await client.ExecuteAsync(request);
            watchBtn.Source = "bookmark_solid.png";
            watchLate = true;
        }

    }
    private async Task LoadMovieDetails(string movieId)
    {
        try
        {
            string apiUrl = "http://" + ip + ":3000/getData?id=" + movieId;

            var client = new RestClient(apiUrl);
            var request = new RestRequest();

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Universal movieDetails = JsonSerializer.Deserialize<Universal>(response.Content, options);

                if (movieDetails.Movie != null)
                {
                    Random rnd = new Random();
                    MovieRuntimeLabel.Text = "USA, "+movieDetails.Movie.Runtime.ToString() + " min, 18+";
                    MoviePoster.Source = ImageSource.FromUri(new Uri(movieDetails.Movie.Poster_Path));
                    MovieTitleLabel.Text = movieDetails.Movie.Title;
                    MovieRatingLabel.Text = movieDetails.Movie.Rating > 0 ? movieDetails.Movie.Rating.ToString() : "New";
                    MovieOverviewLabel.Text = movieDetails.Movie.Overview;
                    MovieGenreLabel.Text = movieDetails.Genres.Genre[0].Name;
                    MovieRatingLargeLabel.Text = movieDetails.Movie.Rating > 0 ? movieDetails.Movie.Rating.ToString() : "NEW";
                    MovieNumberReviews.Text = rnd.Next(50000, 700000).ToString() + " reviews";
                    if (movieDetails.Video_Path != null && movieDetails.Video_Path.Count > 0)
                    {
                        foreach (var video in movieDetails.Video_Path)
                        {
                            var displayInfo = DeviceDisplay.MainDisplayInfo;
                            double screenWidth = displayInfo.Width / displayInfo.Density;
                            double screenHeight = displayInfo.Height / displayInfo.Density;

                            double trailerWidth = screenWidth * 1;
                            double trailerHeight = screenWidth * 0.75;
                            var videoUrl = video.Url.Replace("watch?v=", "embed/");
                            var youtubeWebView = new WebView
                            {
                                Source = videoUrl,
                                WidthRequest = trailerWidth * 1,
                                HeightRequest = trailerWidth * 0.55,
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                                VerticalOptions = LayoutOptions.CenterAndExpand
                            };

                            TrailerStack.Children.Add(youtubeWebView);
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No videos found for this movie", "OK");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Failed to load movie details. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading movie details: {ex.Message}");
        }
    }



    private void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
    {
        double scrollOffset = e.ScrollY;
        double maxScroll = 400; 
        double newOpacity = 1 - (scrollOffset / maxScroll);

        MoviePoster.Opacity = Math.Max(0, Math.Min(1, newOpacity));
        GoBackButton.Opacity = Math.Max(0, Math.Min(1, newOpacity));
    }
    private async void SeeAllWatched(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/list?pageType=watch_later&pageMessage=none&universal=null");
    }

    private async void watchLater(object sender, EventArgs e)
    {
        var btn = sender as ImageButton;
        ManageWatchLater(MovieId);

    }
    private async void likeButton(object sender, EventArgs e)
    {
        //var btn = sender as ImageButton;
        await ManageLike();
    }
    private async void GoBackHome(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    private async void HomeRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///main");
    }
}