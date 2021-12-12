using Faculti.DataClasses;
using Faculti.UI;
using Faculti.UI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Faculti
{
    public delegate void DialogShown();
    public delegate void DialogHidden();

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private readonly User _user;
        private readonly WelcomePage welcome;
        private readonly DispatcherTimer timer = new()
        {
            Interval = TimeSpan.FromMilliseconds(1000),
            IsEnabled = true
        };

        public HomeWindow(User user)
        {
            InitializeComponent();
            DisplayTimeAndDate();
            timer.Tick += Timer_Tick;

            _user = user;
            _user.RetrieveGeneralInfoAsync();

            welcome = new(user);
            welcome.OnDialogShown += new DialogShown(DecreaseOpacity);
            welcome.OnDialogHide += new DialogHidden(DefaultOpacity);

            RadioWelcome.IsChecked = true;
            RadioFeed.Visibility = Visibility.Hidden;
            RadioChat.Visibility = Visibility.Hidden;
            RadioCalendar.Visibility = Visibility.Hidden;
            RadioGrades.Visibility = Visibility.Hidden;
            Frame.Content = welcome;
        }


        #region UI
        private void DecreaseOpacity()
        {
            this.Opacity = 0.5;
        }

        private void DefaultOpacity()
        {
            this.Opacity = 1;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DisplayTimeAndDate();
        }

        private void DisplayTimeAndDate()
        {
            DateTime now = DateTime.Now;
            string day = now.ToString("ddd");
            string month = now.ToString("MMM");
            string date = now.ToString("dd");
            string time = now.ToString("hh:mm tt");

            MonthLabel.Text = $"{month} {date}";
            DayLabel.Text = $"{day}";
            TimeLabel.Text = $"{time}";
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void Grid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            _user.DbConnection.Close();
            Environment.Exit(0);
        }

        private void Grid_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Grid_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Grid_MouseDown_4(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        #endregion
    }
}
