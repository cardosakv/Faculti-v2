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
        private User loginUser;
        private bool _isKeepSignedIn;

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void WindowLogin_Loaded(object sender, RoutedEventArgs e)
        {
            loginUser = new();
            DataContext = loginUser;
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

        private void TextBlockKepSignedIn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isKeepSignedIn = !_isKeepSignedIn;
            CheckboxKeepSignedIn.IsChecked = _isKeepSignedIn;
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
    }
}
