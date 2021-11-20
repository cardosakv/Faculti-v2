using Faculti.UI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Faculti.UI.Windows
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        HomePage home;

        public HomeWindow()
        {
            InitializeComponent();
            home = new HomePage("HOME");
            frame.Content = home;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            home = new HomePage("HOME");
            frame.Content = home;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            home = new HomePage("FEED");
            frame.Content = home;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            home = new HomePage("CHAT");
            frame.Content = home;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            home = new HomePage("GRADES");
            frame.Content = home;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            home = new HomePage("CALENDAR");
            frame.Content = home;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            home = new HomePage("TASKS");
            frame.Content = home;
        }
    }
}
