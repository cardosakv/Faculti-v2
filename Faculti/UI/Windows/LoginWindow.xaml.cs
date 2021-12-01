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
using Faculti.DataClasses;

namespace Faculti.UI.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private User _loginUser = new();
        private bool _isKeepSignedIn;

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            var isMatched = await _loginUser.HaveCredentialsMatchedAsync();
            if (isMatched)
            {
                HomeWindow homeWindow = new();
                this.Close();
                homeWindow.Show();
            }
            else
            {
                MessageBox.Show("Not found");
            }

            //kvcards26@gmail.com
            //12345678
        }

        private async void WindowLogin_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _loginUser;
            await _loginUser.CreateConnection();
        }


        #region UI code behind
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
            this.Close();
            register.Show();
        }
        #endregion

        private void ButtonParent_Checked(object sender, RoutedEventArgs e)
        {
            _loginUser.Type = UserType.Parent;
        }

        private void ButtonTeacher_Checked(object sender, RoutedEventArgs e)
        {
            _loginUser.Type = UserType.Teacher;
        }
    }
}
