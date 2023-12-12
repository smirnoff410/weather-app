using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WeatherDesktop.ViewModel
{
    public class CityViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CityDTO> Cities { get; set; }

        public CityViewModel()
        {
            Task.Run(async () =>
            {
                var httpClient = new HttpClient();
                var result = await httpClient.GetFromJsonAsync<ICollection<CityDTO>>("http://localhost:5000/city");
                if (result == null)
                {
                    MessageBox.Show("Данные о пользователях пусты");
                    return;
                }
                Cities = new ObservableCollection<CityDTO>(result);
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
