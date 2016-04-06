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
        private string password;

        public CredentialManager()
        {
            InitializeComponent();
        }

        public void Refresh(string password)
        {
            this.valt.ItemsSource = new UIModel(password);
        }

        private void valt_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if(e.EditAction ==  DataGridEditAction.Commit)
            {
                var item = e.Row.Item as CredentialEntity;
                if (item != null && item.Password != null && item.Password != null)
                {
                    e.Cancel = false;
                }
            }
            e.Cancel = true;
        }
    }
}
