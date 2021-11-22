using Faculti.DataClasses;
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
    /// Interaction logic for SignupStep1.xaml
    /// </summary>
    public partial class SignupStep2 : Page
    {
        public SignupStep2()
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
    }
}
