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
using Newtonsoft.Json;
using System.Net.Http;

namespace FrontDeskHotel
{
    using HttpClientImpl;

    public partial class AddReservation : Page
    {

        static readonly HttpClient client = new HttpClient();
        static readonly HttpClientImpl clientImpl = new HttpClientImpl(client);

        public string FixedUri = "http://localhost:5000";

        public string nameInput { get; set; }

        public List<Reservation> reservations { get; set; }

        public AddReservation()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

            var name = nameInput;
            var roomtype = roomtypeCombo.SelectionBoxItem;
            DateTime? time = datepickerFrom.SelectedDate;
            DateTime? time2 = datepickerTo.SelectedDate;

            if (name == null || time == null || time2 == null)
            {
                MessageBox.Show("Invalid input! Try again!");
            }
            else
            {

                MessageBoxResult msg = MessageBox.Show("Name: " + name+
                                                        "\nRoomtype: " + roomtype +
                                                        "\nFrom: " + time.Value.ToShortDateString().ToString() + 
                                                        "\nTo: " + time2.Value.ToShortDateString().ToString() + 
                                                        "\nConfirm reservation?", "Confirm Reservation", System.Windows.MessageBoxButton.OKCancel); 
                {
                    if (msg == MessageBoxResult.Yes)
                    {
                        // add to database
                    }
                    else
                    {
                        // Nothing happens
                    }
                }

            }
        }


        async private void AddReservation_Loaded(object sender, RoutedEventArgs e)
        {
            var response = await clientImpl.Get(FixedUri + "/reservations");
            if (!response.IsSuccessStatusCode) throw new Exception(response.StatusCode.ToString());
            var content = response.Content.ReadAsStringAsync().Result;
            reservations = JsonConvert.DeserializeObject<List<Reservation>>(content);

            Label header = new Label
            {
                Content = "Reservation List: \n",
                FontFamily = new FontFamily("Arial Black"),
                FontSize = 15
            };

            resPanel.Children.Add(header);

            Label resList;
            foreach (var res in reservations)
            {
                resList = new Label
                {
                    Content = string.Join(Environment.NewLine,
                                "ReservationID: " + res.ReservationId.ToString() +
                                "\nCustomer: " + res.Customer.CustomerName +
                                "\nRoomNumber: " + res.Room.RoomNumber.ToString() +
                                "\nFrom: " + res.StartDate.ToShortDateString() +
                                "\nTo: " + res.EndDate.ToShortDateString()
                                 ),
                    FontFamily = new FontFamily("Arial Black"),
                    FontSize = 12,
                };

                resPanel.Children.Add(resList);
            }
        }

        private void RoomtypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
