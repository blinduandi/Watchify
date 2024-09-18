using Watchify.Views;

namespace Watchify
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false);
            Routing.RegisterRoute("signin", typeof(LoginPage));
            Routing.RegisterRoute("signup", typeof(SignUpPage));
            Routing.RegisterRoute("main", typeof(Views.MainPage));
            Routing.RegisterRoute("search", typeof(Views.SearchPage));
            Routing.RegisterRoute("list", typeof(Views.ListMovies));
            Routing.RegisterRoute("movie", typeof(Views.MoviePage));
            Routing.RegisterRoute("profile", typeof(Views.ProfilePage));
            Routing.RegisterRoute("recommandation", typeof(Views.Recommandation));
            Routing.RegisterRoute("watch", typeof(Views.WatchLaterPage));

        }
    }
}
