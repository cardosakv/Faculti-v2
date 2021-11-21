using Faculti.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Faculti.UI.Controls
{
    /// <summary>
    /// Interaction logic for BaseTextbox.xaml
    /// </summary>
    public partial class BaseTextBox : UserControl
    {
        public BaseTextBox()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        public string ValidationType { get; set; }

        public string Text
        {
            get { return (String)GetValue(TextProperty); }
            set 
            {
                SetValue(TextProperty, value);
                if (ValidationType != null)
                    IsError = ValidationType == "Email" ?
                              !Syntax.IsValidEmail(value) :
                              !Syntax.IsValidMobileNumber(value);
            }
        }

        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register("Text", typeof(string),
            typeof(BaseTextBox), new PropertyMetadata(""));

        public string Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = 
            DependencyProperty.Register("Title", typeof(string), 
            typeof(BaseTextBox), new PropertyMetadata("Title"));

        public bool IsError
        {
            get { return (bool)GetValue(IsErrorProperty); }
            set
            {
                SetValue(IsErrorProperty, value);
                if (value == true)
                {
                    LabelTitle.Foreground = (Brush)Application.Current.Resources["ErrorRegular"];
                    PanelErrorMessage.Visibility = Visibility.Visible;
                }
                else
                {
                    LabelTitle.Foreground = (Brush)Application.Current.Resources["GrayDark"];
                    PanelErrorMessage.Visibility = Visibility.Hidden;
                }
            }
        }

        public static readonly DependencyProperty IsErrorProperty =
            DependencyProperty.Register("IsError", typeof(bool),
            typeof(BaseTextBox), new PropertyMetadata(false));

        public string ErrorText
        {
            get { return (String)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }

        public static readonly DependencyProperty ErrorTextProperty = 
            DependencyProperty.Register("ErrorText", typeof(string),
            typeof(BaseTextBox), new PropertyMetadata("Error text here."));


        #region UI
        private void TextBoxInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = TextBoxInput.Text;
        }
        #endregion
    }
}
