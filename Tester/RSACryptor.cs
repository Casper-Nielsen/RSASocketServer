using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Tester
{
    class RSACryptor
    {
        readonly RSACryptoServiceProvider rsa;
        public RSACryptor()
        {
            rsa = new RSACryptoServiceProvider();
        }
        public RSACryptor(int bitsAmount)
        {
            rsa = new RSACryptoServiceProvider(bitsAmount);
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

        public void SetPublicKey(byte[] key)
        {
            rsa.ImportRSAPublicKey(key, out _);
        }
    }
}
