namespace Points
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new TabbedPg());
            MainPage = new NewPage1();
            //MainPage = new SettingsPage();
            //MainPage = new TestPage();
        }
    }
}
