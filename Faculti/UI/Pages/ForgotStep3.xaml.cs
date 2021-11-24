using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Faculti.UI.Pages
{
    /// <summary>
    /// Interaction logic for ForgotStep1.xaml
    /// </summary>
    public partial class ForgotStep3 : Page
    {
        public ForgotStep3()
        {
            InitializeComponent();
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        public bool CheckErrors()
        {
            var isError = false;

            if (string.IsNullOrEmpty(TextBoxPassword1.Password))
            {
                TextBoxPassword1.IsError = true;
                isError = true;
            }

            if (string.IsNullOrEmpty(TextBoxPassword2.Text))
            {
                TextBoxPassword2.IsError = true;
                isError = true;
            }

            if (TextBoxPassword1.IsError)
                isError = true;

            if (TextBoxPassword2.IsError)
                isError = true;

            return isError;
        }

        public void SetSuccessMessage()
        {
            TextBoxPassword1.Visibility = Visibility.Hidden;
            TextBoxPassword2.Visibility = Visibility.Hidden;
            Title.Text = "Password Change Successful";
            Caption.Text = "You can now log in to your account.";
            ImageSuccess.Visibility = Visibility.Visible;
            Title.Foreground = (Brush)Application.Current.Resources["BlueDark"];
        }
    }
}
