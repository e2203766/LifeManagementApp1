using LifeManagementApp.Services;

namespace LifeManagementApp
{
    public partial class App : Application
    {
        public App(WeatherService weatherService)
        {
            InitializeComponent();

            // Обёртывание MainPage в NavigationPage
            MainPage = new NavigationPage(new MainPage(weatherService));
        }
    }
}


