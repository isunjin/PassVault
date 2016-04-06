using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassVault.Models
{
    /// <summary> interface to credentials file format </summary>
    public interface ICredentialDatabase
    {
        /// <summary> Load credentials </summary>
        List<CredentialEntity> GetCredentials(string password);

        /// <summary> validate password </summary>
        bool CheckPassword(string password);

        /// <summary> save the data </summary>
        void Save(string password, List<CredentialEntity> credentials);
    }
}
