using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Faculti.DataClasses;
using Faculti.Helpers;
using Faculti.UI.Pages;

namespace Faculti.UI.Windows
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly SignupStep1 _step1 = new();
        private readonly SignupStep2 _step2 = new();
        private readonly SignupStep3 _step3 = new();
        private User _signupUser = new();
        private string _tempPass = String.Empty;
        private bool _isSucess = false;

        public RegisterWindow()
        {
            InitializeComponent();
            DataContext = _signupUser;
            _step1.DataContext = _signupUser;
            _step2.DataContext = _signupUser;
            _step3.DataContext = _signupUser;
            Frame.Content = _step1;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await _signupUser.CreateConnectionAsync();
        }

        private async void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.Content == _step1)
            {
                StartLoader();
                var isError = await _step1.CheckErrorsAsync(_signupUser);

                if (!isError)
                {
                    _signupUser.Type = _step1.userType;
                    Frame.Navigate(_step2);
                }

                StopLoader();
            }
            else if (Frame.Content == _step2)
            {
                StartLoader();
                _tempPass = _signupUser.Password;

                if (!_step2.CheckErrors())
                {
                    await _step3.SendCodeToEmail(_signupUser.Email);
                    Frame.Navigate(_step3);
                }

                StopLoader();
            }
            else if (Frame.Content == _step3 && _isSucess)
            {
                LoginWindow login = new();
                this.Close();
                login.Show();
            }
            else if (Frame.Content == _step3 && _step3.IsCodeCorrect())
            {
                StartLoader();
                await _signupUser.AddToDatabaseAsync();
                await Task.Delay(1000);

                _isSucess = true;
                _step3.SetSuccessMessage();
                ShowSuccessUI();

                StopLoader();
            }

            _signupUser.Password = _tempPass;
        }


        #region UI
        private void StartLoader()
        {
            Loader.Visibility = Visibility.Visible;
            ButtonNext.Visibility = Visibility.Hidden;
        }

        private void StopLoader()
        {
            Loader.Visibility = Visibility.Hidden;
            ButtonNext.Visibility = Visibility.Visible;
        }

        private void ShowSuccessUI()
        {
            Welcome.Visibility = Visibility.Hidden;
            Caption.Visibility = Visibility.Hidden;
            ButtonPrevious.Visibility = Visibility.Hidden;
            ButtonLogin.Visibility = Visibility.Hidden;
            ButtonNext.ToolTip = "Log In";
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            this.Close();
            login.Show();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Frame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.Content != _step1) Frame.GoBack();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _step1.SetEmailRegisteredError(false);
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var ta = new ThicknessAnimation();
            ta.Duration = TimeSpan.FromSeconds(0.3);
            ta.DecelerationRatio = 0.7;
            ta.To = new Thickness(0, 0, 0, 0);
            if (e.NavigationMode == NavigationMode.New)
            {
                ta.From = new Thickness(500, 0, 0, 0);
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                ta.From = new Thickness(0, 0, 500, 0);
            }
                 (e.Content as Page).BeginAnimation(MarginProperty, ta);
        }
        #endregion
    }
}
