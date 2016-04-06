using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassVault.Models
{
    /// <summary> Open a database</summary>
    public class CredentialDatabaseFactory
    {
        /// <summary> default database file name </summary>
        static string defualtFileName = "data.bin";

        /// <summary> Open a database</summary>
        public static ICredentialDatabase Open(string fileName)
        {
            return new CredentialDatabaseV1(fileName);
        }

        /// <summary/>
        public static ICredentialDatabase OpenDefault()
        {
            return Open(defualtFileName);
        }
    }
}
