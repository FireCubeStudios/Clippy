using Clippy.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Security.Credentials;

namespace Clippy.Services
{
    public class KeyService : IKeyService
    {
        private const string Name = "Key";
        private const string Resource = "R";
        private PasswordVault Vault = new PasswordVault();

        public string GetKey()
        {
            try
            {
                return Vault.Retrieve(Resource, Name).Password;
            }
            catch
            {
                return "";
            }
        }

        public void SetKey(string key) => Vault.Add(new PasswordCredential(Resource, Name, key));
    }
}
