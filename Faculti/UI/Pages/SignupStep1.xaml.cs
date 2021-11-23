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
    /// Interaction logic for SignupStep1.xaml
    /// </summary>
    public partial class SignupStep1 : Page
    {
        public UserType userType = UserType.Parent;

        public SignupStep1()
        {
            InitializeComponent();
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        /// <summary>
        /// Checks errors on the textboxes.
        /// </summary>
        public async Task<bool> CheckErrorsAsync(User signupUser)
        {
            var isError = false;

            if (string.IsNullOrEmpty(TextBoxFirstName.Text))
            {
                TextBoxFirstName.IsError = true;
                isError = true;
            }

            if (string.IsNullOrEmpty(TextBoxLastName.Text))
            {
                TextBoxLastName.IsError = true;
                isError = true;
            }

            if (string.IsNullOrEmpty(TextBoxEmail.Text) || !Syntax.IsValidEmail(TextBoxEmail.Text))
            {
                SetEmailError();
                isError = true;
            }
            else 
            {
                bool isEmailError = await signupUser.IsEmailOrNumberAvailableAsync();

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
                TextBoxEmail.ErrorText = "Email already registered.";
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

        private void ButtonParent_Checked(object sender, RoutedEventArgs e)
        {
            userType = UserType.Parent;
        }

        private void ButtonTeacher_Checked(object sender, RoutedEventArgs e)
        {
            userType = UserType.Teacher;
        }
    }
}
