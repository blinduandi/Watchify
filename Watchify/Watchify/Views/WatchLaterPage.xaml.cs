using Watchify.Models;

namespace Watchify.Views;


public partial class WatchLaterPage : ContentPage
{
    int loadMore = 0;
    public int start = 0;
    public int stop = 15;
    public int step = 15;
    public string UniversalJson { get; set; }
    public List<Universal> universal;
    string ip;
    private string userID;
    public WatchLaterPage()
	{
		InitializeComponent();
	}
    private async void RecomandandionRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/recommandation");
    }
    private async void BackToSearch(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    private async void SeeAllWatched(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"/list?pageType=watch_later&pageMessage=none&universal=null");
    }
    private async void HomeRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///main");
    }
    private async void LoadMoreBtn(object sender, EventArgs e)
    {       

    }
}