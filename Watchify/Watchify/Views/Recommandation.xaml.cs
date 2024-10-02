using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using Watchify.Models;
using RestSharp;
using Firebase.Auth;
using Microsoft.Maui.Controls.Shapes;

namespace Watchify.Views;

public partial class Recommandation : ContentPage
{
    public ObservableCollection<Movie> Movies { get; set; }

    public ICommand LikeCommand { get; }
    public ICommand DislikeCommand { get; }

    private const int MaxVisibleCards = 1; 
    private string ip;
    private string userID;
    public int start = 0;
    public int stop = 15;
    public int step = 15;
    public Recommandation()
    {
        InitializeComponent();
        ip = Environment.GetEnvironmentVariable("IP");
        Movies = new ObservableCollection<Movie>();
        userID = SecureStorage.GetAsync("UserID").Result;
        // Load trending movies from API
        LoadTrendingMoviesAsync();
    }

    // Load movies from API
    private async Task LoadTrendingMoviesAsync()
    {
        string apiUrl = $"http://{ip}:3000/getTrendingMovies?id={userID}&start={start}&stop={stop}";

        var client = new RestClient(apiUrl);
        var request = new RestRequest();

        var response = await client.ExecuteAsync(request);

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var universalList = JsonSerializer.Deserialize<List<Universal>>(response.Content, options);

            LoadMovies(universalList);
        }
        catch (Exception ex)
        {
            // Handle the error if something goes wrong
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    // Load movies into the observable collection
    private void LoadMovies(List<Universal> universalList)
    {
        foreach (var item in universalList)
        {
            Movies.Add(new Movie
            {
                ID = item.Movie.ID,
                Title = item.Movie.Title,
                Poster_Path = item.Movie.Poster_Path
            });
        }

        // Load the initial cards after fetching movies from API
        LoadMovieCards();
    }

    private void LoadMovieCards()
    {
        cardStack.Children.Clear();

        for (int i = 0; i < Movies.Count && i < MaxVisibleCards; i++)
        {
            var movie = Movies[i];
            var cardView = CreateMovieCard(movie);
            cardStack.Children.Add(cardView);
        }
    }

    private View CreateMovieCard(Movie movie)
    {
        var displayInfo = DeviceDisplay.MainDisplayInfo;
        double screenWidth = displayInfo.Width / displayInfo.Density;
        double screenHeight = displayInfo.Height / displayInfo.Density;

        double cardWidth = screenWidth * 1;
        double cardHeight = screenHeight * 1;
        double iconWidth = screenWidth * 0.05;


        Border dislikeButton = new Border
        {
            StrokeThickness = 2,
            Stroke = Colors.Gray,
            Background = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(16),
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(100)
            },
            Content = new ImageButton
            {
                Source = "heart_crack_solid.png",
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = iconWidth,
                WidthRequest = iconWidth
            }
        };

        Border infoButton = new Border
        {
            StrokeThickness = 2,
            Stroke = Colors.DarkOrange,
            Background = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(16),
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(100)
            },
            Content = new ImageButton
            {
                Source = "img_tv_retro.png",
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = iconWidth,
                WidthRequest = iconWidth
            }
        };

        Border saveButton = new Border
        {
            StrokeThickness = 2,
            Stroke = Colors.Yellow,
            Background = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(16),
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(100)
            },
            Content = new ImageButton
            {
                Source = "bookmark_regular.png",
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = iconWidth,
                WidthRequest = iconWidth
            }
        };

        Border likeButton = new Border
        {
            StrokeThickness = 2,
            Stroke = Colors.Red,
            Background = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(16),
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(100)
            },
            Content = new ImageButton
            {
                Source = "heart_solid.png",
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = iconWidth,
                WidthRequest = iconWidth
            }
        };


        // Handle button clicks
        //dislikeButton.Clicked += (s, e) => OnDislikeClicked(movie);
        //saveButton.Clicked += (s, e) => OnSaveForLaterClicked(movie);
        //likeButton.Clicked += (s, e) => OnLikeClicked(movie);

        var card = new Frame
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = cardWidth,
            HeightRequest = cardHeight,
            Content = new StackLayout
            {
                Spacing = 10,
                Children =
         {
             new Image
             {
                 Source = movie.Poster_Path,
                 Aspect = Aspect.AspectFit,
                 VerticalOptions = LayoutOptions.Start,
                 HorizontalOptions = LayoutOptions.Center,
                 HeightRequest = cardHeight * 0.5
             },
             new Label
             {
                 Text = movie.Title,
                 FontSize = 24,
                 TextColor = Colors.White,
                 VerticalOptions = LayoutOptions.Start,
                 HorizontalOptions = LayoutOptions.Center,
                 HorizontalTextAlignment = TextAlignment.Center,
             },
             new Label
             {
                 Text = movie.Overview, //"The Batman is a 2022 American superhero film based on the DC Comics character Batman. Directed by Matt Reeves from a screenplay..", // Display movie description
                 FontSize = 16,
                 TextColor = Colors.White,
                 VerticalOptions = LayoutOptions.Start,
                 HorizontalOptions = LayoutOptions.Start,
                 HorizontalTextAlignment = TextAlignment.Center,
                 Margin = new Thickness(0, 0, 0, 10) // Padding for description
             },
             new StackLayout
             {
                 Orientation = StackOrientation.Horizontal,
                 HorizontalOptions = LayoutOptions.Center,
                 Spacing = 15,
                 Children =
                 {
                     dislikeButton,
                     infoButton,
                     saveButton,
                     likeButton
                 }
             }
         }
            }
        };

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += (s, e) => OnPanUpdated(s as View, e, movie);
        card.GestureRecognizers.Add(panGesture);

        return card;
    }
    private async void OnPanUpdated(View view, PanUpdatedEventArgs e, Movie movie)
    {
        if (e.StatusType == GestureStatus.Running)
        {
            view.TranslationX = e.TotalX;
            view.Rotation = e.TotalX / 10;
        }
        else if (e.StatusType == GestureStatus.Completed)
        {
            if (Math.Abs(view.TranslationX) > 100)
            {
                if (view.TranslationX > 0)
                {
                    await OnLike(movie, view);
                }
                else
                {
                    await OnDislike(movie, view);
                }
            }
            else
            {
                await view.TranslateTo(0, 0, 250, Easing.SpringOut);
                await view.RotateTo(0, 250, Easing.SpringOut);
            }
        }
    }

    private async Task OnLike(Movie movie, View view)
    {
        await view.TranslateTo(500, 0, 250, Easing.Linear);
        RemoveCard(view, movie);
        //await DisplayAlert("Liked", $"You liked {movie.Title}", "OK");
        string apiUrl = $"http://{ip}:3000/addUserInteraction?id={userID}&movieId={movie.ID}&interaction=liked";

        var client = new RestClient(apiUrl);
        var request = new RestRequest();

        var response = await client.ExecuteAsync(request);
    }

    private async Task OnDislike(Movie movie, View view)
    {
        await view.TranslateTo(-500, 0, 250, Easing.Linear);
        RemoveCard(view, movie);
        //await DisplayAlert("Disliked", $"You disliked {movie.Title}", "OK");
        string apiUrl = $"http://{ip}:3000/addUserInteraction?id={userID}&movieId={movie.ID}&interaction=disliked";

        var client = new RestClient(apiUrl);
        var request = new RestRequest();

        var response = await client.ExecuteAsync(request);
    }

    private void RemoveCard(View view, Movie movie)
    {
        cardStack.Children.Remove(view);
        Movies.Remove(movie);

        if (Movies.Count > MaxVisibleCards - 1)
        {
            var nextMovie = Movies[MaxVisibleCards - 1];
            var cardView = CreateMovieCard(nextMovie);
            cardStack.Children.Add(cardView);
        }
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
        await Shell.Current.GoToAsync($"///main");
    }
}
