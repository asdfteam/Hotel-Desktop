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
using FrontDeskHotel;
namespace FrontDeskHotel
{
    using HttpClientImpl;

    public partial class Rooms : Page
    {

        static readonly HttpClient client = new HttpClient();
        static readonly HttpClientImpl clientImpl = new HttpClientImpl(client);
        public string FixedUri = "http://localhost:5000";

        List<Room> _rooms { get; set; }
        string StatusContainer { get; set; }

        public Rooms()
        {
            StatusContainer = null;
            InitializeComponent();
            this.DataContext = this;
        }

        async public void Room_OnLoaded (Object sender, RoutedEventArgs e )
        {
            var response = await clientImpl.Get(FixedUri + "/rooms");
            var content = response.Content.ReadAsStringAsync().Result;
            var rooms = JsonConvert.DeserializeObject<List <Room>> (content);

            Label roomlist;
            ComboBox changeStatus;

            foreach (var room in rooms)
            {
                roomlist = new Label
                {
                    
                    Content = String.Join(Environment.NewLine,
                                          "Room: " + room.RoomNumber.ToString() + 
                                          "\nStatus: " + room.RoomStatus.ToString() + 
                                          "\nNote: " + room.Note),
                    FontFamily = new FontFamily("Arial Black"),
                    FontSize = 12,
                    BorderThickness = new Thickness (1,1,1,1),
                    BorderBrush = Brushes.Black,
                    Background = Brushes.LightGray,
                    Width = 350,
                    HorizontalAlignment = HorizontalAlignment.Left
                };


                changeStatus = new ComboBox
                {

                    ItemsSource = new List<string> {"CLEANING", "MAINTENANCE", "SERVICE"},
                    FontFamily = new FontFamily("Arial Black"),
                    FontSize = 12,
                    Width = 350,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = Brushes.White,
                    SelectedItem = room.RoomStatus
                    
                };

                changeStatus.SelectionChanged += async (sender, e) =>
                {
                    
                    if (sender is ComboBox dropdown) StatusContainer = dropdown.SelectedItem as string;
                    MessageBoxResult msg = MessageBox.Show("Change status to " + StatusContainer + " on room " + room.RoomNumber + "?", "Change Status", System.Windows.MessageBoxButton.OKCancel);

                    if (msg == MessageBoxResult.OK)
                    {
                        ClearPanelChildren();
                        var response = await clientImpl.Put(FixedUri + "/rooms/" + $"{room.RoomNumber}?newStatus={StatusContainer}");
                        Room_OnLoaded(this, e);
                    }
                    
                };

                Label space = new Label();

                roomPanel.Children.Add(roomlist);
                roomPanel.Children.Add(changeStatus);
                roomPanel.Children.Add(space);
            }
        }

        private void ClearPanelChildren()
        {
            for (int size = roomPanel.Children.Count - 1; size >= 0; size += -1)
            {
                UIElement elements = roomPanel.Children[size];
                if (elements is Label || elements is ComboBox)
                    roomPanel.Children.Remove(elements);

            }

        }
    }
}
