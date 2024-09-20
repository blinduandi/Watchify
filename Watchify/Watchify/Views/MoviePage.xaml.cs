namespace Watchify.Views;

public partial class MoviePage : ContentPage
{
	public MoviePage()
	{
        InitializeComponent();
    }

    private void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
    {
        double scrollOffset = e.ScrollY;
        double maxScroll = 400; 
        double newOpacity = 1 - (scrollOffset / maxScroll);

        MoviePoster.Opacity = Math.Max(0, Math.Min(1, newOpacity));
        GoBackButton.Opacity = Math.Max(0, Math.Min(1, newOpacity));
    }



    private async void GoBackHome(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}