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
using Newtonsoft.Json;
using System.Net.Http;

namespace FrontDeskHotel
{
    using HttpClientImpl;
    using HotelLibrary;

    public partial class AddReservation : Page
    {

        static readonly HttpClient client = new HttpClient();
        static readonly HttpClientImpl clientImpl = new HttpClientImpl(client);

        public string FixedUri = "http://localhost:5000";

        public int inputSinglebed { get; set; }
        public int inputDoublebed { get; set; }

        public List<Reservation> reservations { get; set; }
        private Customer _customer { get; }

        
        public AddReservation(Customer customer)
        {
            
            _customer = customer;
            InitializeComponent();
            this.DataContext = this;
            

        }

        async private void addButton_Click(object sender, RoutedEventArgs e)
        {
            int singlebed = inputSinglebed;
            var doublebed = inputDoublebed;
            DateTime startDate = (DateTime)datepickerFrom.SelectedDate;
            DateTime endDate = (DateTime) datepickerTo.SelectedDate;

            if (singlebed > 2 || singlebed < 0 || doublebed < 0 || doublebed > 2 || startDate == null || endDate == null || startDate == endDate || endDate.CompareTo(startDate) < 0)
            {
                MessageBox.Show("Invalid input! Try again!");
            }
            else
            {

                MessageBoxResult msg = MessageBox.Show("Singlebed: " + singlebed +
                                                       "\nDoublebed: " + doublebed +
                                                       "\nFrom: " + startDate.ToShortDateString().ToString() + 
                                                       "\nTo: " + endDate.ToShortDateString().ToString() + 
                                                       "\nConfirm reservation?", "Confirm Reservation", System.Windows.MessageBoxButton.OKCancel); 
                {
                    if (msg == MessageBoxResult.OK)
                    {
                        ClearPanelChildren();
                        var reservation = new CreateReservationRequest
                        {
                            StartDate = startDate,
                            EndDate = endDate,
                            DoubleBeds = doublebed,
                            SingleBeds = singlebed
                        };

                        var response = await clientImpl.Post(FixedUri + "/reservations/" + $"{_customer.CustomerId}", JsonConvert.SerializeObject(reservation));
                        AddReservation_Loaded(this, e);
                    }
                }

            }
            
        }

        async private void AddReservation_Loaded(object sender, RoutedEventArgs e)
        {
            var response = await clientImpl.Get(FixedUri + "/reservations/" + $"{_customer.CustomerId}");
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
            Button delBtn;
            foreach (var res in reservations)
            {

                resList = new Label
                {
                    Content = string.Join(Environment.NewLine,
                                "ReservationID: " + res.ReservationId.ToString() +
                                "\nCustomer: " + res.Customer.CustomerName +
                                "\nRoom Number: " + res.Room.RoomNumber.ToString() +
                                "\nSingle Beds: " + res.Room.SingleBed.ToString() +
                                "\nDouble Beds: " + res.Room.DoubleBed.ToString() +
                                "\nFrom: " + res.StartDate.ToShortDateString() +
                                "\nTo: " + res.EndDate.ToShortDateString()
                                 ),
                    FontFamily = new FontFamily("Arial Black"),
                    FontSize = 12,
                    BorderThickness = new Thickness(1, 1, 1, 1),
                    BorderBrush = Brushes.Black,
                    Background = Brushes.LightGray,
                    MaxWidth = 200,
                    Width = 200,
                    HorizontalAlignment = HorizontalAlignment.Left,    
                };

                delBtn = new Button
                {
                    Content = "Delete ID " + res.ReservationId.ToString(),
                    FontFamily = new FontFamily("Arial Black"),
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    BorderThickness = new Thickness(2, 2, 2, 2),
                    BorderBrush = Brushes.Black,
                    Width = 200,
                    Height = 23,
                    Background = Brushes.White,
                    Cursor = Cursors.Hand
                };

                
                delBtn.Click += async (sender, e) =>
                {
                    
                    MessageBoxResult msg = MessageBox.Show("Delete reservation " + res.ReservationId + "?", "Delete Reservation", System.Windows.MessageBoxButton.OKCancel); 
                    if (msg == MessageBoxResult.OK)
                    {
                        ClearPanelChildren();
                        var responseDelete = await clientImpl.Delete(FixedUri + "/reservations/" + $"{res.ReservationId}");
                        AddReservation_Loaded(this, e);
                    }

                    
                };

                Label space = new Label();
                resPanel.Children.Add(resList);
                resPanel.Children.Add(delBtn);
                resPanel.Children.Add(space);
            }
        }

        private void ClearPanelChildren()
        {
            for (int size = resPanel.Children.Count - 1; size >= 0; size += -1)
            {
                UIElement elements = resPanel.Children[size];
                if (elements is Label || elements is Button)
                    resPanel.Children.Remove(elements);

                
            }

        }
    }
}
