using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RSASocketServer
{
    class RSACryptor
    {
        string ContainerName;
        readonly RSACryptoServiceProvider rsa;
        public RSACryptor()
        {
            rsa = CreateRSA();
        }
        public RSACryptor(int bitsAmount)
        {
            rsa = CreateRSA(bitsAmount);
        }

        private RSACryptoServiceProvider CreateRSA(int bitAmount = 2048)
        {
            CspParameters cspParams = new CspParameters(1);
            cspParams.KeyContainerName = ContainerName;
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
            return new RSACryptoServiceProvider(bitAmount, cspParams) { PersistKeyInCsp = true };
        }

        public void DeleteKeyInCsp()
        {
            var cspParams = new CspParameters { KeyContainerName = ContainerName };
            var rsa = new RSACryptoServiceProvider(cspParams) { PersistKeyInCsp = false };

            rsa.Clear();
        }
        public string Decrypt(string encryptedData)
        {
            try
            {
                byte[] decryptedData;
                //Decrypt the passed byte array and specify OAEP padding. 
                decryptedData = rsa.Decrypt(Convert.FromBase64String(encryptedData), true);

                return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(encryptedData)));
            }
            catch
            {
                return null;
            }

        }

        public string Encrypt(string plainText)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(plainText)));
        }

        public byte[] Encrypt(byte[] rawData)
        {
            try
            {
                byte[] encryptedData;
                encryptedData = rsa.Encrypt(rawData, true);
                return encryptedData;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public byte[] Decrypt(byte[] encryptedData)
        {
            try
            {
                byte[] decryptedData;
                decryptedData = rsa.Decrypt(encryptedData, true);

                return decryptedData;
            }
            catch
            {
                return null;
            }
        }

        public byte[] GetPublicKey()
        {
            return rsa.ExportRSAPublicKey();
        }
        public RSAParameters GetParameters()
        {
            return rsa.ExportParameters(false);
        }

        public void SetPublicKey(byte[] key)
        {
            rsa.ImportRSAPublicKey(key, out _);
        }
    }
}
