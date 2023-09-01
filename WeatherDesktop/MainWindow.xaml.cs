using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ShowWeather_Click(object sender, RoutedEventArgs e)
        {
            var httpClient = new HttpClient();
            var result = await httpClient.GetFromJsonAsync<ICollection<WeatherDTO>>("http://localhost:5000/weatherforecast");
            if(result == null)
            {
                MessageBox.Show("Данные о погоде пусты");
                return;
            }
            WeatherList.Items.Clear();
            foreach (var weather in result)
                WeatherList.Items.Add(weather);
        }
    }
}
