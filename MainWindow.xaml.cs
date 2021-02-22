using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.Http;
using HotelLibrary;


namespace FrontDeskHotel
{
    using HttpClientImpl;
    public partial class MainWindow : Window
    {
        static readonly HttpClient client = new HttpClient();
        static readonly HttpClientImpl clientImpl = new HttpClientImpl(client);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void resBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Reservations();

        }

        private void roomBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Rooms();
            
        }
        private void checkBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new CheckInOut();
        }
    }
}
