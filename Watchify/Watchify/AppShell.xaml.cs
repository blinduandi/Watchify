namespace Watchify
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false);  // Ensure nav bar is hidden

        }
    }
}
