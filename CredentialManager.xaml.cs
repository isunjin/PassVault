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
    }
}
