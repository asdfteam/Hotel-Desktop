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

namespace FrontDeskHotel
{

    public partial class CheckInOut : Page
    {
        public CheckInOut()
        {
            InitializeComponent();
        }

        private void checkOutBtn_Click(object sender, RoutedEventArgs e)
        {
            checkFrame.Content = new CheckOut(); 
        }

        private void checkInBtn_Click(object sender, RoutedEventArgs e)
        {
            checkFrame.Content = new CheckIn();
        }
    }
}
