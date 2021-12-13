using Faculti.DataClasses;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Faculti.UI.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly User _loginUser = new();
        private bool _isKeepSignedIn;

        public LoginWindow()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Email))
            {
                _loginUser.Email = Properties.Settings.Default.Email;
                _loginUser.Password = Properties.Settings.Default.Password;

                OpenConnection();
                RoutedEventArgs newEventArgs = new(Button.ClickEvent);
                ButtonLogin.RaiseEvent(newEventArgs);
            }

            InitializeComponent();
            DataContext = _loginUser;
        }

        private void OpenConnection()
        {
            _loginUser.CreateConnectionAsync().Wait();
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Hidden;

            StartLoader();

            if (await _loginUser.HaveCredentialsMatchedAsync())
            {
                if (_isKeepSignedIn)
                {
                    SaveCredentials();
                }
                else
                {
                    RemoveCredentials();
                }

                await _loginUser.RetrieveGeneralInfoAsync();
                HomeWindow home;

                if (_loginUser.Type == UserType.Parent)
                {
                    home = new(_loginUser);
                }
                else
                {
                    home = new(_loginUser);
                }

                this.Close();
                home.Show();
            }
            else
            {
                ErrorMessage.Visibility = Visibility.Visible;
            }

            StopLoader();
        }

        private void SaveCredentials()
        {
            Properties.Settings.Default.Email = _loginUser.Email;
            Properties.Settings.Default.Password = _loginUser.Password;
        }

        private void RemoveCredentials()
        {
            Properties.Settings.Default.Email = string.Empty;
            Properties.Settings.Default.Password = string.Empty;
        }

        private void ButtonParent_Checked(object sender, RoutedEventArgs e)
        {
            _loginUser.Type = UserType.Parent;
        }

        private void ButtonTeacher_Checked(object sender, RoutedEventArgs e)
        {
            _loginUser.Type = UserType.Teacher;
        }

        private async void WindowLogin_Loaded(object sender, RoutedEventArgs e)
        {
            await _loginUser.CreateConnectionAsync();
        }

        #region UI code behind
        private void StartLoader()
        {
            Loader.Visibility = Visibility.Visible;
            ButtonLogin.Visibility = Visibility.Hidden;
        }

        private void StopLoader()
        {
            Loader.Visibility = Visibility.Hidden;
            ButtonLogin.Visibility = Visibility.Visible;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (_loginUser.DbConnection != null)
            {
                _loginUser.DbConnection.Close();
            }

            Environment.Exit(0);
        }

        private void WindowLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CheckboxKeepSignedIn_Click(object sender, RoutedEventArgs e)
        {
            _isKeepSignedIn = !_isKeepSignedIn;
            CheckboxKeepSignedIn.IsChecked = _isKeepSignedIn;
        }

        private void TextBlockCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new();
            if (_loginUser.DbConnection != null) _loginUser.DbConnection.Close();
            this.Close();
            register.Show();
        }

        private void TextBlockForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            ForgotWindow forgot = new();

            if (_loginUser.DbConnection != null)
            {
                _loginUser.DbConnection.Close();
            }

            this.Close();
            forgot.Show();
        }

        private void WindowLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FocusManager.SetFocusedElement(this, this);
                ButtonLogin.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }
        #endregion
    }
}
