using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HotelLibrary;
using System.Linq;


namespace FrontDeskHotel
{
    using HttpClientImpl;
    public partial class Reservations : Page
    {

        static readonly HttpClient client = new HttpClient();
        static readonly HttpClientImpl clientImpl = new HttpClientImpl(client);

        public string FixedUri = "http://localhost:5000";
        List<Room> rooms { get; set; }
        List<Reservation> reservations { get; set; }


        public Reservations()
        {
            InitializeComponent();
        }

        private void addResBtn_Click(object sender, RoutedEventArgs e)
        {
            resFrame.Content = null;
            resFrame.Content = new AddReservation();
        }

        private void deleteResBtn_Click(object sender, RoutedEventArgs e)
        {
            resFrame.Content = null;
            resFrame.Content = new DeleteReservation();
        }

        private void Reservation_OnLoaded(object sender, RoutedEventArgs e)
        {
            var response = clientImpl.Get(FixedUri + "/rooms?status=AVAILABLE");

            resFrame.Content = "LISTE med reservasjoner vises her";
            

            
            
        }
    }
}
