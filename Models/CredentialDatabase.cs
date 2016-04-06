using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PassVault.Models
{
    /// <summary/>
    [Serializable]
    public class CredentialDatabaseEntity
    {
        /// <summary/>
        public int Version { get; set; }

        /// <summary/>
        public string Password { get; set; }

        /// <summary/>
        public List<CredentialEntity> Credentials { get; set; }
    }

    /// <summary/>
    public class CredentialDatabaseV1 : ICredentialDatabase
    {
        /// <summary/>
        private string fileName;

        /// <summary/>
        private const int Version = 1;

        /// <summary/>
        public CredentialDatabaseV1(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName cannot be null or empty");
            }
            this.fileName = fileName;
        }

        /// <summary> Deserialize data </summary>
        private bool TryDeserialize<T>(byte[] data, out T obj)
        {
            obj = default(T);
            try
            {
                var str = UTF8Encoding.UTF8.GetString(data);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (var memoryStream = new MemoryStream(data))
                {
                    obj = (T)serializer.Deserialize(memoryStream);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary/>
        private CredentialDatabaseEntity CreateEmptyEntity(string password)
        {
            return new CredentialDatabaseEntity()
            {
                Password = password,
                Credentials = new List<CredentialEntity>(),
                Version = Version
            };
        }

        /// <summary> try load the database, could fail if password is incorrect </summary>
        private bool TryLoad(string password, out CredentialDatabaseEntity entity)
        {
            if(!File.Exists(this.fileName))
            {
                entity = this.CreateEmptyEntity(password);
                return true;
            }

            entity = null;

            try
            {
                var data = File.ReadAllBytes(this.fileName);
                AESCryptographer crypt = new AESCryptographer();

                //TEMP
                var salt = UTF8Encoding.UTF8.GetBytes(password.PadRight(16, 'x'));
                var decryptedData = crypt.Decrypt(password, data, salt);     
                return this.TryDeserialize<CredentialDatabaseEntity>(decryptedData, out entity);
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary/>
        public List<CredentialEntity> GetCredentials(string password)
        {
            CredentialDatabaseEntity entity;
            List<CredentialEntity> credentials = null;

            if (this.TryLoad(password, out entity))
            {
                credentials = entity.Credentials;
            }

            return credentials;
        }

        /// <summary/>
        public bool CheckPassword(string password)
        {
            CredentialDatabaseEntity entity;
            if (this.TryLoad(password, out entity))
            {
                return password == entity.Password;
            }
            return false;
        }

        /// <summary/>
        public void Save(string password, List<CredentialEntity> credentials)
        {
            CredentialDatabaseEntity database = new CredentialDatabaseEntity();
            database.Password = password;
            database.Version = Version;
            database.Credentials = credentials;

            XmlSerializer serializer = new XmlSerializer(typeof(CredentialDatabaseEntity));
            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, database);
                AESCryptographer crypt = new AESCryptographer();

                //TEMP
                var salt = UTF8Encoding.UTF8.GetBytes(password.PadRight(16, 'x'));
                var encryptedData = crypt.Encrypt(password, memoryStream.ToArray(), salt);

                File.WriteAllBytes(this.fileName, encryptedData);
            }
        }
    }
}
