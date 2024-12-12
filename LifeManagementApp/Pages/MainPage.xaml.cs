using System;
using LifeManagementApp.Services;
using Microsoft.Maui.Controls;
using LifeManagementApp.Pages;
using System.Collections.ObjectModel;

namespace LifeManagementApp
{
    public partial class MainPage : ContentPage
    {
        private readonly WeatherService _weatherService;
        private static ObservableCollection<string> _todoTasks = new ObservableCollection<string>(); // Глобальный список задач
        private string _selectedCity = "Helsinki"; // Значение по умолчанию

        public MainPage(WeatherService weatherService)
        {
            InitializeComponent();
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService), "Weather Service is required");

            // Инициализация списка городов
            CityPicker.ItemsSource = new[] { "Helsinki", "Tampere", "Turku", "Oulu", "Vaasa" }; // Добавлен Vaasa
            CityPicker.SelectedIndexChanged += OnCitySelected;

            // Установка значения по умолчанию
            CityPicker.SelectedItem = _selectedCity;

            // Обновление количества задач
            UpdateTodoCount();
        }

        private void OnCitySelected(object sender, EventArgs e)
        {
            if (CityPicker.SelectedItem != null)
            {
                _selectedCity = CityPicker.SelectedItem.ToString();
            }
        }

        private async void OnWeatherUpdateClicked(object sender, EventArgs e)
        {
            try
            {
                // Получение координат для выбранного города
                var (latitude, longitude) = GetCityCoordinates(_selectedCity);

                // Получение данных о погоде
                var weatherData = await _weatherService.GetWeatherDataAsync(latitude, longitude);

                if (weatherData?.CurrentWeather != null && weatherData.Daily != null)
                {
                    TemperatureLabel.Text = $"{weatherData.CurrentWeather.Temperature}°C";
                    MinTemperatureLabel.Text = $"{weatherData.Daily.Temperature_2m_Min[0]}°C";
                    MaxTemperatureLabel.Text = $"{weatherData.Daily.Temperature_2m_Max[0]}°C";
                    WindspeedLabel.Text = $"{weatherData.CurrentWeather.WindSpeed} m/s";

                    // Предупреждение о высокой скорости ветра
                    if (weatherData.CurrentWeather.WindSpeed.HasValue && weatherData.CurrentWeather.WindSpeed.Value > 4)
                    {
                        await DisplayAlert("Warning", $"Wind speed in {_selectedCity} is expected to exceed 4 m/s today!", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Unable to retrieve weather data. Please check your internet connection or API settings.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unable to retrieve weather data: {ex.Message}", "OK");
            }
        }

        private (double, double) GetCityCoordinates(string city)
        {
            return city switch
            {
                "Helsinki" => (60.1699, 24.9384),
                "Tampere" => (61.4991, 23.7871),
                "Turku" => (60.4518, 22.2666),
                "Oulu" => (65.0121, 25.4651),
                "Vaasa" => (63.096, 21.6158), // Координаты Вааса
                _ => (60.1699, 24.9384) // По умолчанию Helsinki
            };
        }

        private async void OnTodoPageClicked(object sender, EventArgs e)
        {
            // Переход на страницу задач
            var todoPage = new TodoPage(_todoTasks);
            await Navigation.PushAsync(todoPage);
            UpdateTodoCount();
        }

        private void UpdateTodoCount()
        {
            TodoCountLabel.Text = _todoTasks.Count.ToString();
        }
    }
}





