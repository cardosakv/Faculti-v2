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
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using Faculti.UI.Pages;

namespace Faculti.UI.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class ForgotWindow : Window
    {
        private readonly ForgotStep1 _step1 = new();
        private readonly ForgotStep2 _step2 = new();
        private readonly ForgotStep3 _step3 = new();
        private readonly User _forgotUser = new();
        private bool _isSucess = false;

        public ForgotWindow()
        {
            InitializeComponent();
            this.DataContext = _forgotUser;
            _step1.DataContext = _forgotUser;
            _step2.DataContext = _forgotUser;
            _step3.DataContext = _forgotUser;
            Frame.Content = _step1;
        }

        private async void WindowLogin_Loaded(object sender, RoutedEventArgs e)
        {
            await _forgotUser.CreateConnectionAsync();
        }

        private async void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.Content == _step1)
            {
                StartLoader();

                await Task.Delay(500);
                var isError = await _step1.CheckErrorsAsync(_forgotUser);
                if (!isError)
                {
                    await _step2.SendCodeToEmail(_forgotUser.Email);
                    Frame.Navigate(_step2);
                }

                StopLoader();
            }
            else if (Frame.Content == _step2)
            {
                StartLoader();

                await Task.Delay(500);
                var isCorrect = _step2.IsCodeCorrect();

                if (isCorrect) Frame.Navigate(_step3);

                StopLoader();
            }
            else if (Frame.Content == _step3 && _isSucess)
            {
                LoginWindow login = new();
                _forgotUser.DbConnection.Close();
                this.Close();
                login.Show();
            }
            else  if  (Frame.Content == _step3)
            {
                StartLoader();

                if (!_step3.CheckErrors())
                {
                    await Task.Delay(1000);
                    await _forgotUser.UpdatePassword();
                    Frame.Navigate(_step3);
                    _step3.SetSuccessMessage();
                    ShowSuccessUI();
                    _isSucess = true;
                }

                StopLoader();
            }
        }


        #region UI code behind
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
            ButtonLogin.Visibility = Visibility.Hidden;
            ButtonNext.ToolTip = "Log In";
        }

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

        private void TextBlockLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new();
            this.Close();
            login.Show();
        }

        private void Frame_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var fa = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            (e.Content as Page).BeginAnimation(OpacityProperty, fa);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FocusManager.SetFocusedElement(this, this);
                ButtonNext.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _step1.SetEmailRegisteredError(false);
        }        

        private void WindowLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FocusManager.SetFocusedElement(this, this);
                ButtonNext.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }
        #endregion


    }
}
