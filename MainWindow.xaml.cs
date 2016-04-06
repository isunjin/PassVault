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

namespace PassVault
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.login.PasswordRecieved += PasswordRecieved;
        }

        private void ShowLogin(bool show)
        {
            this.login.Visibility = show ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            this.credentialManager.Visibility = show ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
        }

        void PasswordRecieved(object sender, PasswordEventArgs e)
        {
            this.ShowLogin(false);
            this.credentialManager.Refresh(e.Password);
        }
    }
}
