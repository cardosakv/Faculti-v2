using System;
using System.Configuration;
using System.Collections.Specialized;
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
using Faculti.Helpers;
using Oracle.ManagedDataAccess.Client;

using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Data;
using Faculti.DataRepository.DatabaseManager;
using Faculti.DataClasses;

namespace Faculti.UI.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion

        private string _email = string.Empty;
        private string _password = string.Empty;
        private bool _isKeepSignedIn;

        public LoginWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
                TextBoxEmail.IsError = !Syntax.IsValidEmail(value);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }




        #region UI

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void WindowLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        private void Toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isKeepSignedIn)
            {
                CheckboxKeepSignedIn.IsChecked = false;
                _isKeepSignedIn = false;
            }
            else
            {
                CheckboxKeepSignedIn.IsChecked = true;
                _isKeepSignedIn = true;
            }

            CheckboxKeepSignedIn.Focus();
        }

        private void CheckboxKeepSignedIn_Click(object sender, RoutedEventArgs e)
        {
            if (_isKeepSignedIn)
            {
                CheckboxKeepSignedIn.IsChecked = false;
                _isKeepSignedIn = false;
            }
            else
            {
                CheckboxKeepSignedIn.IsChecked = true;
                _isKeepSignedIn = true;
            }

            CheckboxKeepSignedIn.Focus();
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow home = new HomeWindow();
            this.Close();
            home.Show();
        }

        private void TextBlockCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            this.Close();
            register.Show();
        }
    }
}
