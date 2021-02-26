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
    

    public partial class SearchCustomer : Page
    {
        static readonly HttpClient client = new HttpClient();
        static readonly HttpClientImpl clientImpl = new HttpClientImpl(client);
      Customer customer { get; set; }
        public string FixedUri = "http://localhost:5000";
        public string nameInput { get; set; }
        public SearchCustomer()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        async private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var response = await clientImpl.Get(FixedUri + "/customers/search?customerName=" + $"{nameInput}");
            var content = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<Customer>(content);
            
            if (customer.CustomerName != null)
            {
                searchFrame.Content = new AddReservation(customer);
            }
        }
    }
}
