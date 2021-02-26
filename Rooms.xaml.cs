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
using Newtonsoft.Json;
namespace FrontDeskHotel
{
    using HttpClientImpl;

    public partial class Rooms : Page
    {

        static readonly HttpClient client = new HttpClient();
        static readonly HttpClientImpl clientImpl = new HttpClientImpl(client);
        public string FixedUri = "http://localhost:5000";

        List<Room> _rooms { get; set; }

        public Rooms()
        {
            InitializeComponent();
           
        }

        async public void Room_OnLoaded (Object sender, RoutedEventArgs e )
        {
            var response = await clientImpl.Get(FixedUri + "/rooms");
            var content = response.Content.ReadAsStringAsync().Result;
            var rooms = JsonConvert.DeserializeObject<List < Room >> (content);

            Label roomlist;
            ComboBox changeStatus;

            foreach (var room in rooms)
            {
                roomlist = new Label
                {
                    
                    Content = String.Join(Environment.NewLine,
                                          "Room: " + room.RoomNumber.ToString() + 
                                          "\nStatus: " + room.RoomStatus.ToString()),
                    FontFamily = new FontFamily("Arial Black"),
                    FontSize = 12,
                    BorderThickness = new Thickness (1,1,1,1),
                    BorderBrush = Brushes.Black,
                    Background = Brushes.LightGray,
                    MaxWidth = 200,
                    Width = 200,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                
                changeStatus = new ComboBox
                {
                   ItemsSource = new List<string> { "AVAILABLE", "NOE", "CHRISTIAN" },
                   FontFamily = new FontFamily("Arial Black"),
                   SelectedIndex = 0,
                   FontSize = 12,
                   Width = 200,
                   HorizontalAlignment = HorizontalAlignment.Left,
                   Background = Brushes.White


                };

                changeStatus.SelectionChanged += (sender, e) =>
                {
                    MessageBox.Show("Status is changed");
                };
                
                Label space = new Label();

                roomPanel.Children.Add(roomlist);
                roomPanel.Children.Add(changeStatus);
                roomPanel.Children.Add(space);
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
