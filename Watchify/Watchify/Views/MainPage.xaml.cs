namespace Watchify.Views;
using RestSharp;
using System.Net;
using System.Text.Json;
using Watchify.Models;


public partial class MainPage : ContentPage
{

    string ip;
    private string userID;
    public int start = 0;
    public int stop = 10;
    public int step = 10;
    public MainPage()
    {
        InitializeComponent();
        ip = Environment.GetEnvironmentVariable("IP");
        Task task = StartAsync();
    }


    public async Task StartAsync()
    {
       // Hide main content
        MainContent.IsVisible = false;

        // Show the loading screen
        LoadingScreen.IsVisible = true;

        try
        {
            var generateButtonsTask = Task.Run(() => GenerateButtonsGenere());
            //var loadMoviesTask = Task.Run(() => LoadMoviesNew());
            var loadTrendingMoviesTask = Task.Run(() => LoadTrendingMoviesAsync());
            var loadLiked = Task.Run(() =>  LoadMoviesLiked());
            var loadWatch = Task.Run(() =>  LoadMoviesWatched());
            await Task.WhenAll(generateButtonsTask, loadTrendingMoviesTask,loadLiked,loadWatch);
            await LoadMoviesNew();

        }
        finally
        {

            Thread.Sleep(1000);

            LoadingScreen.IsVisible = false;

            MainContent.IsVisible = true;
        }

        AnimateCircle();
    }



    private async Task GenerateButtonsGenere()
    {

        var options = new RestClientOptions("http://" + ip + ":3000/getAllGenres");
        var client = new RestClient(options);
        var request = new RestRequest("");
        request.AddHeader("accept", "application/json");

        try
        {
            var response = await client.GetAsync(request);
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
                    button.Clicked += async (sender, e) =>
                    {
                        await RedirectGenre(sender);
                    };

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

    private async Task AnimateCircle()
    {
        while (true)
        {
            
                 await GradientCircle.ScaleTo(1.2, 1000, Easing.Linear);
                 await GradientCircle.ScaleTo(1.0, 1000, Easing.Linear);
                 await Task.Delay(16);
        }
    }

    private async void SearchClick(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchPage());
        //await Shell.Current.GoToAsync("/search");
    }

    private async void ProfilePage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("/profile");
    }

    private async void MoviePage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("/movie");
    }

    private async Task LoadMoviesWatched()
    {
        userID = await SecureStorage.GetAsync("UserID");

        string apiUrl = $"http://{ip}:3000/getWatchLaterMovies?id={userID}&start={start}&stop={stop}";
        //start = stop;
        //stop += step;
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

                StackWatch.Children.Add(stackLayout);
            }

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
        //start = stop;
        //stop += step;
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

                StackLiked.Children.Add(stackLayout);
            }

        }
        catch (Exception ex)
        {
            //await DisplayAlert("Error", $"An error occurred load: {ex.Message}", "OK");
        }
    }
    private async Task LoadMoviesNew()
    {
        

        userID = await SecureStorage.GetAsync("UserID");


        string apiUrl = $"http://{ip}:3000/getNewMovies?id={userID}&start={start}&stop={stop}";
        //start = stop;
        //stop += step;
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
                tapGesture.Tapped += (s, e) => MoviePageRedirect(s,e);
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

                StackNew.Children.Add(stackLayout);
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred load(new): {ex.Message}", "OK");
        }
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
        await Shell.Current.GoToAsync($"///main/movie?movieId={movieId}");

    }
    private async Task RedirectGenre(object sender)
    {
        var button = sender as Button; // Cast sender to Button
        if (button != null)
        {
            string genre = button.Text; // Get the text of the button
            await Shell.Current.GoToAsync($"/list?pageType=genre&pageMessage={genre}&universal=null");
        }
    }
    private async void SeeAllTrending(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/list?pageType=trending&pageMessage=none&universal=null");
    }
    private async void SeeAllNew(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/list?pageType=newMovies&pageMessage=none&universal=null");
    }
    private async void SeeAllLiked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/list?pageType=liked&pageMessage=none&universal=null");
    }
    private async void SeeAllWatched(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/list?pageType=watch_later&pageMessage=none&universal=null");
    }
    private async void RecomandandionRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/recommandation");
    }
    private async void HomeRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
        await Shell.Current.GoToAsync($"///main");
    }
}