using System;
using System.Collections.Generic;
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
using System.Net.Http;

namespace FrontDeskHotel
{

    public partial class Rooms : Page
    {

        static readonly HttpClient client = new HttpClient();
        public string FixedUri = "http://localhost:5000";

        List<Room> rooms { get; set; }
        List<Reservation> reservations { get; set; }
        public Rooms()
        {
            InitializeComponent();
           
        }

        public void Room_OnLoaded (Object sender, RoutedEventArgs e )
        {
            roomsFrame.Content = "Liste med rom som trenger room service og vedlikehold vises her";
        }
    }
}
