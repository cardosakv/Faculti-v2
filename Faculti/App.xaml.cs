using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Faculti
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly SplashScreen _splash = new("/Resources/Images/splashscreen-img.png");

        protected override void OnStartup(StartupEventArgs e)
        {
            _splash.Show(false, false);
            _splash.Close(TimeSpan.FromMilliseconds(500));
        }
    }
}
