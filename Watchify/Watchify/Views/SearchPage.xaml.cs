namespace Watchify.Views;

public partial class SearchPage : ContentPage
{
	public SearchPage()
	{
		InitializeComponent();
	}
    private async void BackHome(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}