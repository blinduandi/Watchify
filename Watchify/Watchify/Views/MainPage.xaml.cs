namespace Watchify.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        AnimateCircle();
    }

    private async void AnimateCircle()
    {
        while (true)
        {
            await GradientCircle.ScaleTo(1.2, 1000, Easing.Linear);
            await GradientCircle.ScaleTo(1.0, 1000, Easing.Linear);
        }
    }


}