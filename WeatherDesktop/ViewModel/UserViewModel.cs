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
    public class UserViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<UserDTO> Users { get; set; }

        public UserViewModel()
        {
            Task.Run(async () => 
            {
                var httpClient = new HttpClient();
                var result = await httpClient.GetFromJsonAsync<ICollection<UserDTO>>("http://localhost:5000/user");
                if (result == null)
                {
                    MessageBox.Show("Данные о пользователях пусты");
                    return;
                }
                Users = new ObservableCollection<UserDTO>(result);
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
