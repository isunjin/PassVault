﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PassVault.Models
{
    class UIModel : ObservableCollection<CredentialEntity>
    {
        public string password;

        public UIModel(string password)
        {
            this.password = password;
            var database = CredentialDatabaseFactory.OpenDefault();
            var data = database.GetCredentials(password);

            foreach(var item in data)
            {
                this.Add(item);
            }
        }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            var database = CredentialDatabaseFactory.OpenDefault();
            database.Save(this.password, this.Where(c => !string.IsNullOrEmpty(c.Password) && !string.IsNullOrEmpty(c.UserName)).ToList());
        }
    }
}
