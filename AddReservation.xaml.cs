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

        public List<Reservation> reservations { get; set; }

        public AddReservation()
        {
            InitializeComponent();

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            ClearLabel();
            DateTime? time = dp.SelectedDate;
            DateTime? time2 = dp2.SelectedDate;

            Label from = new Label();
            Label to = new Label();
            from.Content = time.Value.ToShortDateString() + " - ";
            Canvas.SetLeft(from, 0);
            Canvas.SetLeft(to, 70);
            to.Content = time2.Value.ToShortDateString();


            mycanvas.Children.Add(from);
            mycanvas.Children.Add(to);

            MessageBox.Show("Reservation was added");
        }

        private void ClearLabel()
        {
            for (int size = mycanvas.Children.Count - 1; size >= 0; size += -1)
            {
                UIElement elements = mycanvas.Children[size];
                if (elements is Label)
                    mycanvas.Children.Remove(elements);
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
    }
}
