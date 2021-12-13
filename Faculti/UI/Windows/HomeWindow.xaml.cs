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
    public delegate void AddingFinished();

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private Class _currClass;
        private Student _currStudent;
        private WelcomePage _welcomePage;
        private readonly User _user;
        private readonly DispatcherTimer _timer = new()
        {
            Interval = TimeSpan.FromMilliseconds(1000),
            IsEnabled = true
        };

        public HomeWindow(User user)
        {
            InitializeComponent();
            DisplayTimeAndDate();
            _timer.Tick += Timer_Tick;

            _user = user;
            InitializeGetStarted();
        }

        private void InitializeGetStarted()
        {
            RadioStudents.Visibility = Visibility.Hidden;

            if (_user.IsFirstTime)
            {
                _welcomePage = new(_user);
                _welcomePage.OnDialogShown += new DialogShown(DecreaseOpacity);
                _welcomePage.OnDialogHide += new DialogHidden(DefaultOpacity);
                _welcomePage.OnFinished += new AddingFinished(DisplayInfo);

                RadioWelcome.IsChecked = true;
                RadioWelcome.Visibility = Visibility.Visible;
                RadioFeed.Visibility = Visibility.Hidden;
                RadioChat.Visibility = Visibility.Hidden;
                RadioGrades.Visibility = Visibility.Hidden;
                Menu.Visibility = Visibility.Hidden;
                Frame.Content = _welcomePage;
            }
            else
            {
                DisplayInfo();
            }
        }

        private void DisplayInfo()
        {
            RadioWelcome.Visibility = Visibility.Collapsed;
            DisplayFeed();
            DisplayChat();
            DisplayMenu();
            DisplayGrades();

            if (_user.Type == UserType.Teacher)
            {
                DisplayStudents();
            }
        }

        private void DisplayMenu()
        {
            Menu.Visibility = Visibility.Visible;
        }

        private void DisplayFeed()
        {
            RadioFeed.Visibility = Visibility.Visible;
            RadioFeed.IsChecked = true;
        }

        private void DisplayChat()
        {
            RadioChat.Visibility = Visibility.Visible;
        }

        private void DisplayGrades()
        {
            RadioGrades.Visibility = Visibility.Visible;
        }

        private void DisplayStudents()
        {
            RadioStudents.Visibility = Visibility.Visible;
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

        private void FontAwesome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Frame.CanGoBack) Frame.GoBack();
        }

        private void FontAwesome_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (Frame.CanGoForward) Frame.GoForward();
        }
        #endregion
    }
}
