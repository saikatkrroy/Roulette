using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace Roulette.Security.Helpers
{
    public class AesEncryptionHelper
    {
        /// <summary> 
        /// Encrypt the given string. The string can be decrypted using Decrypt(). 
        /// The sharedSecret and salt parameters must match. 
        /// </summary> 
        /// <param name="plainText">The text to encrypt.</param> 
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param> 
        /// <param name="salt">A salt string to be used, pass null or empty string to generate a random salt.  Salt MUST be Base-64.</param> 
        /// <returns>The encrypted text.</returns>
        public static string Encrypt(string plainText, string sharedSecret, ref string salt)
        {
            byte[] encryptedData;

            using (Aes aes = Aes.Create())
            {
                aes.Key = ClampToBitSize(aes.KeySize, Encoding.UTF8.GetBytes(sharedSecret));

                if (String.IsNullOrEmpty(salt))
                {
                    aes.GenerateIV();
                    salt = Convert.ToBase64String(aes.IV);
                }
                else
                    aes.IV = ClampToBitSize(aes.BlockSize, Convert.FromBase64String(salt));

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                MemoryStream memStream;
                using (memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                    }
                }
                encryptedData = memStream.ToArray();
            }
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary> 
        /// Decrypt the given string. Assumes the string was encrypted using Encrypt(), using an identical sharedSecret and salt. 
        /// </summary> 
        /// <param name="cipherText">The text to decrypt, must be Base 64.</param> 
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param> 
        /// <param name="salt">A salt string that was used to encrypt.  The salt MUST be Base 64.</param> 
        /// <returns>The decrypted text.</returns>
        public static string Decrypt(string cipherText, string sharedSecret, string salt)
        {
            byte[] cypherBytes = Convert.FromBase64String(cipherText);
            string resultText;

            using (Aes aes = Aes.Create())
            {
                aes.Key = ClampToBitSize(aes.KeySize, Encoding.UTF8.GetBytes(sharedSecret));
                aes.IV = ClampToBitSize(aes.BlockSize, Convert.FromBase64String(salt));

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memStream = new MemoryStream(cypherBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            resultText = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return resultText;
        }

        /// <summary>
        /// Takes an arbitrary string and remakes it as a valid base 64 string.  It will NOT
        /// even remotely resemble the input string.
        /// </summary>
        /// <param name="inText">The text to remake.</param>
        /// <returns>Base 64 version of the supplied string.</returns>
        public static string RemakeStringAsBase64(string inText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(inText));
        }

        /// <summary>
        /// Clamps the provided key to the specified number of bits.  Note that this method only
        /// works correctly for even multiples of 8 bits.
        /// </summary>
        /// <param name="keySize">The size of the target key in bits.</param>
        /// <param name="inKey">The current key to clamp or padd as appropriate.</param>
        /// <returns></returns>
        private static byte[] ClampToBitSize(int keySize, byte[] inKey)
        {
            int keyBytes = keySize / 8;

            // If the key is already the right size, just return it.
            if (inKey.Length == keyBytes)
            {
                return inKey;
            }

            // If it's too big or too small, copy it into a better sized array.
            // This assumes arrays are initialized to 0 and thus pads short arrays with zeroes.
            byte[] paddedKey = new byte[keyBytes];
            Array.Copy(inKey, paddedKey, Math.Min(inKey.Length, keyBytes));
            return paddedKey;
        }
    }
}