using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;
using WeatherDesktop.ViewModel;

namespace WeatherDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для CityView.xaml
    /// </summary>
    public partial class CityView : UserControl
    {
        private readonly CityViewModel _cityViewModel;
        private readonly JsonSerializerOptions _jsonSettings = new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        public CityView()
        {
            InitializeComponent();

            _cityViewModel = new CityViewModel();
            DataContext = _cityViewModel;
        }

        private void CityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if(lb.SelectedItem != null)
            {
                var userDTO = lb.SelectedItem as CityDTO;
                CityID.Text = userDTO.ID.ToString();
                CityName.Text = userDTO.Name.ToString();
            }
        }

        private async void CreateCity_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var httpClient = new HttpClient();
            var stringContent = JsonSerializer.Serialize(new { Name = CityName.Text }, _jsonSettings);
            StringContent content = new(stringContent, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync("http://localhost:5000/city/create", content);
            var response = await result.Content.ReadAsStringAsync();
            var newCity = JsonSerializer.Deserialize<CityDTO>(response, _jsonSettings);
            _cityViewModel.Cities.Add(newCity);
        }

        private void UpdateCity_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private async void DeleteCity_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var httpClient = new HttpClient();
            var result = await httpClient.DeleteAsync($"http://localhost:5000/city/delete/{CityID.Text}");
            if(result.IsSuccessStatusCode)
            {
                var city = _cityViewModel.Cities.First(x => x.ID == Guid.Parse(CityID.Text));
                var selectedIndex = CityList.SelectedIndex;
                if(selectedIndex != 0)
                    CityList.SelectedIndex = selectedIndex - 1;
                else if(_cityViewModel.Cities.Count > 1)
                    CityList.SelectedIndex = selectedIndex + 1;
                else
                {
                    CityID.Text = string.Empty;
                    CityName.Text = string.Empty;
                }
                _cityViewModel.Cities.Remove(city);
            }
        }
    }
}
