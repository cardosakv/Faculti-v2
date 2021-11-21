using Oracle.ManagedDataAccess.Client;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Faculti.Helpers
{
    /// <summary>
    ///     Helper class for encrypting and decrypting strings.
    /// </summary>
    public class Security
    {
        #region Secret
        private const string _securityKey = "992fbd43528da0e45687230ff8acd9ed8ade0125";
        #endregion Secret

        /// <summary>
        ///    Converts the encrypted/unreadable string back to plain text.
        /// </summary>
        public static string Decrypt(string stringInCipherText)
        {
            byte[] toEncryptArray = Convert.FromBase64String(stringInCipherText);

            MD5CryptoServiceProvider objMD5CryptoService = new();
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(Encoding.UTF8.GetBytes(_securityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider
            {
                Key = securityKeyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();

            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        ///     Converts the string to encrypted/cipher text.
        /// </summary>
        public static string Encrypt(string stringdInPlainText)
        {
            byte[] toEncryptedArray = Encoding.UTF8.GetBytes(stringdInPlainText);

            MD5CryptoServiceProvider objMD5CryptoService = new();
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(Encoding.UTF8.GetBytes(_securityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider
            {
                Key = securityKeyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}