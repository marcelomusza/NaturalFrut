using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;

namespace NaturalFrut.Helpers
{
    public class Encryption
    {
        private static string _encryptionKey = "Q88fUHv9w73KwUqk7XwpDPBb0Xu8ABjKlZdgMRHIBaU=";
        private static string _encryptionIV = "RsKhmMvQiUZCR5LqS876jQ==";

        public static string EncryptPassword(string password)
        {
            try
            {
                string EncryptionKey = "MiViejaMulaYaNoEsLoQueEra";

                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    myRijndael.Key = pdb.GetBytes(32);
                    myRijndael.IV = pdb.GetBytes(16);
                    
                    // Encrypt the string to an array of bytes.
                    byte[] encrypted = EncryptStringToBytes(password, myRijndael.Key, myRijndael.IV);

                    string encryptedString = Convert.ToBase64String(encrypted);

                    return encryptedString;
                }

            }
            catch (Exception e)
            {
               return null;
            }
        }

        public static string DecryptPassword(string password)
        {
            try
            {
                string EncryptionKey = "MiViejaMulaYaNoEsLoQueEra";

                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    myRijndael.Key = pdb.GetBytes(32);
                    myRijndael.IV = pdb.GetBytes(16);                    

                    byte[] encryptedString = Convert.FromBase64String(password);

                    // Decrypt the bytes to a string.
                    string decriptedString = DecryptStringFromBytes(encryptedString, myRijndael.Key, myRijndael.IV);

                    return decriptedString;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

    }
}