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
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;

namespace PassVault
{
    /// <summary>
    /// Interaction logic for PassVault.xaml
    /// </summary>
    public partial class CredentialManager : UserControl
    {
        private UIModel model;

        public CredentialManager()
        {
            InitializeComponent();
        }

        public void Refresh(string password)
        {
            this.model = new UIModel(password);
            this.valt.ItemsSource = this.model;
        }

        private DataGridRow FindDataGridRow(MouseButtonEventArgs e)
        {
            DataGridRow row = null;

            DependencyObject dep = (DependencyObject)e.OriginalSource;

            // iteratively traverse the visual tree
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
            {
                return null;
            }

            if (dep is DataGridCell)
            {
                DataGridCell cell = dep as DataGridCell;

                while ((dep != null) && !(dep is DataGridRow))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                row = dep as DataGridRow;
            }

            return row;
        }

        private void Delete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var row = FindDataGridRow(e);
            if(row!=null && row.Item!=null)
            {
                var entity = row.Item as CredentialEntity;
                if (entity != null)
                {
                    var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive); 
                    if(MessageBox.Show(window, "Are you sure to delete this item?", "Remove item", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                    {
                        this.model.Remove(entity);
                    }
                }
            }
        }

        private void Copy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var row = FindDataGridRow(e);
            if (row != null && row.Item != null)
            {
                var entity = row.Item as CredentialEntity;
                if (entity != null)
                {
                    Clipboard.SetText(entity.Password);
                }
            }
        }

        private void addNew_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.userName.Text) && !string.IsNullOrEmpty(this.passWord.Text))
            {
                this.model.Add(new CredentialEntity()
                {
                    UserName = this.userName.Text,
                    Password = this.passWord.Text
                });

                this.passWord.Text = string.Empty;
            }
        }

        private void HandleFile(Action<String> action)
        {
            String myFile = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    if ((myFile = openFileDialog1.FileName) != null)
                    {
                        action(myFile);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void encrypt_Click(object sender, RoutedEventArgs e)
        {
            this.HandleFile((s) =>
            {
                var data = File.ReadAllBytes(s);
                AESCryptographer crypt = new AESCryptographer();
                var salt = UTF8Encoding.UTF8.GetBytes(this.model.password.PadRight(16, 'x'));
                var encryptedData = crypt.Encrypt(this.model.password, data, salt);

                File.WriteAllBytes(s+".eee", encryptedData);

                MessageBox.Show("done");
            });
        }

        private void decrypt_Click(object sender, RoutedEventArgs e)
        {
            this.HandleFile((s) =>
            {
                if (s.EndsWith(".eee"))
                {
                    var data = File.ReadAllBytes(s);
                    AESCryptographer crypt = new AESCryptographer();
                    var salt = UTF8Encoding.UTF8.GetBytes(this.model.password.PadRight(16, 'x'));
                    var decryptedData = crypt.Decrypt(this.model.password, data, salt);

                    File.WriteAllBytes(s.Substring(0, s.Length - 4) + ".1", decryptedData);
                    MessageBox.Show("done");
                }
            });
        }
    }
}
