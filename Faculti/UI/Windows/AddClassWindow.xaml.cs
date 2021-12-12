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
    public partial class AddClassWindow : Window
    {
        private readonly User _user;
        private readonly Class _class = new();

        public AddClassWindow(User user)
        {
            InitializeComponent();
            Pic.ImageSource = Picture.ImageToBitmap(_class.Picture);
            DataContext = _class;
            _user = user;
        }

        private async void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_class.Name))
            {
                TextboxClassName.IsError = true;
                return;
            }

            Loader.Visibility = Visibility.Visible;
            ButtonCreate.Visibility = Visibility.Hidden;

            await _class.AddToDatabaseAsync(_user.DbConnection, _user.Id);
            ButtonCreate.Content = "✔️";

            ButtonCreate.Visibility = Visibility.Visible;
            Loader.Visibility = Visibility.Hidden;

            await Task.Delay(1000);
            DialogResult = true;
            Close();
        }

        private void PictureCon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Pic.ImageSource = Picture.ChooseFromFile((BitmapImage)Pic.ImageSource);
            _class.Picture = Picture.BitmapToImage(Pic.ImageSource as BitmapImage);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }
    }
}
