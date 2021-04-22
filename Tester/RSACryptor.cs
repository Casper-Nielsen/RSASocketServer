using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Tester
{
    class RSACryptor
    {
        readonly string ContainerName = "MyContainer";
        RSACryptoServiceProvider rsa;
        public RSACryptor()
        {
            rsa = new RSACryptoServiceProvider();
        }

        public RSACryptor(int bitsAmount)
        {
            rsa = new RSACryptoServiceProvider(bitsAmount);
        }

        public RSACryptor(int bitsAmount, bool csp)
        {
            if (csp)
            {
                rsa = CreateRSA(bitsAmount);
            }
            else
            {
                rsa = new RSACryptoServiceProvider(bitsAmount);
            }
        }

        /// <summary>
        /// Creates or gets the rsa with the csp
        /// </summary>
        /// <param name="bitAmount">The bit size</param>
        /// <returns></returns>
        private RSACryptoServiceProvider CreateRSA(int bitAmount = 2048)
        {
            CspParameters cspParams = new CspParameters(1);
            cspParams.KeyContainerName = ContainerName;
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";

            return new RSACryptoServiceProvider(bitAmount, cspParams);
        }

        /// <summary>
        /// Deletes the key in csp
        /// </summary>
        public void DeleteKeyInCsp()
        {
            var cspParams = new CspParameters { KeyContainerName = ContainerName };
            rsa = new RSACryptoServiceProvider(cspParams) { PersistKeyInCsp = false };

            rsa.Clear();
        }

        /// <summary>
        /// Decrypts the string using base64
        /// </summary>
        /// <param name="encryptedData">The message to decrypt</param>
        /// <returns>The decrypted message encoded in utf8</returns>
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

        /// <summary>
        /// Encryptes the string using utf8
        /// </summary>
        /// <param name="plainText">The message to encrypt</param>
        /// <returns>The encrypted message encoded in base64</returns>
        public string Encrypt(string plainText)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(plainText)));
        }

        /// <summary>
        /// Encryptes the message
        /// </summary>
        /// <param name="rawData">The messages that needs to be encrypted</param>
        /// <returns>The encrypted message</returns>
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

        /// <summary>
        /// Decrypts the message
        /// </summary>
        /// <param name="encryptedData">The message to decrypt</param>
        /// <returns>The decrypted message</returns>
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

        /// <summary>
        /// Gets the public key in bytes
        /// </summary>
        /// <returns>The public key in bytes</returns>
        public byte[] GetPublicKey()
        {
            return rsa.ExportRSAPublicKey();
        }

        /// <summary>
        /// Gets the rsa parameters
        /// </summary>
        /// <param name="privateKey">If it should be full key</param>
        /// <returns>The parameter</returns>
        public RSAParameters GetParameters(bool privateKey = false)
        {
            return rsa.ExportParameters(privateKey);
        }

        /// <summary>
        /// Sets the public key
        /// </summary>
        /// <param name="key">The public key</param>
        public void SetPublicKey(byte[] key)
        {
            rsa.ImportRSAPublicKey(key, out _);
        }
    }
}
