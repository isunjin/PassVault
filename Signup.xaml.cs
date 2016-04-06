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
using System.Windows.Shapes;

namespace PassVault
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Window
    {
        public event EventHandler<PasswordEventArgs> PasswordSet;

        public Signup()
        {
            InitializeComponent();
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            password1.Password = string.Empty;
            password2.Password = string.Empty;
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(password1.Password))
            {
                MessageBox.Show("Password cannot be null or empty", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password1.Password != password2.Password)
            {
                MessageBox.Show("Passwords does not match", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.Close();

            if(this.PasswordSet != null)
            {
                this.PasswordSet(this, new PasswordEventArgs() { Password = this.password1.Password });
            }
        }
    }
}
