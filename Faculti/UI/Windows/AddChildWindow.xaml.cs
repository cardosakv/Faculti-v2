using Faculti.DataClasses;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Faculti.Helpers;

namespace Faculti.UI.Windows
{
    /// <summary>
    /// Interaction logic for AddClassWindow.xaml
    /// </summary>
    public partial class AddChildWindow : Window
    {
        private readonly User _user;
        private readonly Student _student = new();

        public AddChildWindow(User user)
        {
            InitializeComponent();
            DataContext = _student;
            _user = user;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        private async void ButtonAccess_Click(object sender, RoutedEventArgs e)
        {
            Loader.Visibility = Visibility.Visible;
            ButtonAccess.Visibility = Visibility.Hidden;

            await _student.UpdateParent(_user.Id, _user.DbConnection);
            await Task.Delay(500);
            ButtonAccess.Content = "✔️";

            ButtonAccess.Visibility = Visibility.Visible;
            Loader.Visibility = Visibility.Hidden;

            _user.RemoveFirstTime();
            await Task.Delay(1000);
            DialogResult = true;
            Close();
        }

        private async Task ButtoSearch_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_student.Code))
            {
                TextboxStudentCode.IsError = true;
                return;
            }

            if (!await _student.CheckCode(_user.DbConnection))
            {
                return;
            }

            await _student.RetrieveInfoAsync(_user.Id, _user.DbConnection);
            StudentName.Content = _student.FullName;
            StudentId.Content = _student.Id;
            StudentClass.Content = _student.Class.Name;
        }

        private void ButtoSearch_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
