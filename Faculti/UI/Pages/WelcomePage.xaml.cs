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
        public event AddingFinished OnFinished = delegate { };

        private readonly User _user;

        public WelcomePage(User user)
        {
            InitializeComponent();
            _user = user;
            
            if (_user.Type == UserType.Teacher)
            {
                Caption.Text = "Start creating your first class to continue.";
            }
            else
            {
                Caption.Text = "Start adding your student child to continue.";
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (_user.Type == UserType.Teacher)
            {
                AddClassWindow addClass = new(_user);
                OnDialogShown();
                addClass.ShowDialog();
                if (addClass.DialogResult == true)
                {
                    OnFinished();
                    OnDialogHide();
                }
                else
                {
                    OnDialogHide();
                }
            }
            else
            {
                AddChildWindow addChild = new(_user);
                OnDialogShown();
                addChild.ShowDialog();
                if (addChild.DialogResult == true)
                {
                    OnFinished();
                    OnDialogHide();
                }
                else
                {
                    OnDialogHide();
                }
            }
        }
    }
}
