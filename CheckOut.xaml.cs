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
using System.Net.Http;
using HotelLibrary;
using Newtonsoft.Json;

namespace FrontDeskHotel
{
    using HttpClientImpl;
   
    public partial class CheckOut : Page
    {

        static readonly HttpClient client = new HttpClient();
        static readonly HttpClientImpl clientImpl = new HttpClientImpl(client);

        public string FixedUri = "http://localhost:5000";

        public string InputName { get; set; }
        public CheckOut()
        {
            InitializeComponent();
            this.DataContext = this;
        }

       async private void CheckOutBtn_Click(object sender, RoutedEventArgs e)
        {
            var response = await clientImpl.Put(FixedUri + "/rooms/checkout?customerName=" + $"{InputName}");

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Thank you for visiting Hotel Hight!\n");
            }
            
        }
    }
}
