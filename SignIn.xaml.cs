using PassVault.Models;
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
    public class PasswordEventArgs : EventArgs
    {
        public string Password { get; set; }
    }

    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class SignIn : UserControl
    {
        public event EventHandler<PasswordEventArgs> PasswordRecieved;

        public SignIn()
        {
            InitializeComponent();
            this.Loaded += SignIn_Loaded;
        }

        void SignIn_Loaded(object sender, RoutedEventArgs e)
        {
            this.passwordBox.Focus();
        }

        private void Login()
        {
            var password = passwordBox.Password;
            Task.Run(() =>
            {
                var credentialDatabase = CredentialDatabaseFactory.OpenDefault();
                if (credentialDatabase.CheckPassword(password))
                {
                    if (PasswordRecieved != null)
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            PasswordRecieved(this, new PasswordEventArgs()
                            {
                                Password = password
                            });
                        }));
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect password", "Incorrect password", MessageBoxButton.OK);
                }
            });
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            this.Login();
        }

        private void signUp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Signup signUpWindow = new Signup();
            signUpWindow.PasswordSet += PasswordRecived;
            signUpWindow.ShowDialog();
        }

        void PasswordRecived(object sender, PasswordEventArgs e)
        {
            if (PasswordRecieved != null)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    PasswordRecieved(this, new PasswordEventArgs()
                    {
                        Password = e.Password
                    });
                }));
            }
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.Login();
            }
        }
    }
}
