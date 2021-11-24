using Faculti.Helpers;
using Faculti.Services;
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
    /// Interaction logic for ForgotStep2.xaml
    /// </summary>
    public partial class ForgotStep2 : Page
    {
        private int _verCode = Randomizer.GenerateCode(10000, 99999);

        public ForgotStep2()
        {
            InitializeComponent();
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);
        }

        public bool IsCodeCorrect()
        {
            if (TextBoxCode.Text == _verCode.ToString())
                return true;
            else
                TextBoxCode.IsError = true;

            return false;
        }

        public void GetNewCode()
        {
            TextBoxCode.Text = String.Empty;
            TextBoxCode.IsError = false;
            _verCode = Randomizer.GenerateCode(10000, 99999);
        }

        public async Task SendCodeToEmail(string recepientEmail)
        {
            Clear();
            Focus();
            TextBoxCode.IsError = false;
            await Verification.SendCodeToEmailAsync(recepientEmail, _verCode);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxCode.IsError = false;
        }

        public void Clear()
        {
            TextBoxCode.Text = String.Empty;
        }
    }
}
