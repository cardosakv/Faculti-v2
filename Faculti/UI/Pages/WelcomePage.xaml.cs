using Faculti.DataClasses;
using Faculti.UI.Windows;
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
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public event DialogShown OnDialogShown = delegate { };
        public event DialogHidden OnDialogHide = delegate { };

        private User _user;
        private HomeWindow _homeWindow;

        public WelcomePage(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddClassWindow addClass = new(_user);
            OnDialogShown();
            addClass.ShowDialog();
            if (addClass.DialogResult == true || addClass.DialogResult == false)
            {
                OnDialogHide();
            }
        }
    }
}
