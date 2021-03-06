﻿using System;
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
using Newtonsoft.Json;


namespace FrontDeskHotel
{
    using HttpClientImpl;
    public partial class Reservations : Page
    {

        static readonly HttpClient client = new HttpClient();
        static readonly HttpClientImpl clientImpl = new HttpClientImpl(client);

        public string FixedUri = "http://localhost:5000";

        public List<Reservation> reservations { get; set; } 


        public Reservations()
        {
            InitializeComponent();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            resPanel.Children.Clear();
            scrollviewer.Visibility = Visibility.Collapsed;
            resFrame.Content = null;
            resFrame.Content = new SearchCustomer();
        }

       

        async private void Reservation_OnLoaded(object sender, RoutedEventArgs e)
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
                                "\nCustomer: "  + res.Customer.CustomerName + 
                                "\nRoom Number: " + res.Room.RoomNumber.ToString() + 
                                "\nSingle Beds: " + res.Room.SingleBed.ToString() +
                                "\nDouble Beds: " + res.Room.DoubleBed.ToString() +
                                "\nFrom: " + res.StartDate.ToShortDateString() + 
                                "\nTo: " + res.EndDate.ToShortDateString()
                                 ),
                    FontFamily = new FontFamily("Arial Black"),
                    FontSize = 12,
                    BorderThickness = new Thickness(1,1,1,1),
                    BorderBrush = Brushes.Black,
                    Background = Brushes.LightGray,
                    MaxWidth = 300,
                    Width = 300,
                    HorizontalAlignment = HorizontalAlignment.Left

                };
                Label space = new Label();

                resPanel.Children.Add(space);
                resPanel.Children.Add(resList);
            }
            

        }

        
    }

}
