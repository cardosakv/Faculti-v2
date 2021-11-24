using Faculti.DataClasses;
using Faculti.Helpers;
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
    public partial class ForgotStep1 : Page
    {
        public ForgotStep1()
        {
            InitializeComponent();
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        public async Task<bool> CheckErrorsAsync(User signupUser)
        {
            var isError = false;

            if (string.IsNullOrEmpty(TextBoxEmail.Text) || !Syntax.IsValidEmail(TextBoxEmail.Text))
            {
                SetEmailError();
                isError = true;
            }
            else
            {
                bool isEmailError = !await signupUser.IsEmailOrNumberAvailableAsync();

                if (isEmailError)
                {
                    SetEmailRegisteredError(true);
                    isError = true;
                }
            }

            return isError;
        }

        public void SetEmailRegisteredError(bool isError)
        {
            if (isError)
            {
                TextBoxEmail.IsError = true;
                TextBoxEmail.ErrorText = "Email is not associated to any account.";
            }
            else
            {
                TextBoxEmail.IsError = false;
                TextBoxEmail.ErrorText = "Invalid email format.";
            }
        }

        private void SetEmailError()
        {
            TextBoxEmail.IsError = true;
            TextBoxEmail.ErrorText = "Invalid email format.";
        }
    }
}
