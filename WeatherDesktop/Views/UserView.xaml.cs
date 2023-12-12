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
using WeatherDesktop.ViewModel;

namespace WeatherDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {
        public UserView()
        {
            InitializeComponent();

            DataContext = new UserViewModel();
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            var userDTO = lb.SelectedItem as UserDTO;
            UserID.Text = userDTO.ID.ToString();
            UserName.Text = userDTO.Name.ToString();
            UserCities.Items.Clear();
            foreach (var city in userDTO.Cities)
                UserCities.Items.Add(city);
        }
    }
}
