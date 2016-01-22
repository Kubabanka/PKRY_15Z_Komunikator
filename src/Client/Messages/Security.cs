using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    /// <summary>
    /// klasa definiujaca metody do szyfrowania i deszyfrowania wiadomosci
    /// </summary>
    public static class Security
    {
        /// <summary>
        /// metoda do deszyfrowania danych algorytmen RSA
        /// </summary>
        /// <param name="DataToDecrypt">dane do zaszyfrowania</param>
        /// <param name="RSAKeyInfo">klucze RSA</param>
        /// <param name="DoOAEPPadding">czy stosowac wypelnienie typu OAEP</param>
        /// <returns>odszyfrowana tablica bajtow</returns>
        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);

                    decryptedData = RSA.Decrypt(DataToDecrypt, false);
                }
                return decryptedData;
            }

            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }

        }
        /// <summary>
        /// metoda do deszyfrowania danych algorytmem RSA
        /// </summary>
        /// <param name="DataToEncrypt">dane do zaszyfrowania</param>
        /// <param name="RSAKeyInfo">klucze RSA</param>
        /// <param name="DoOAEPPadding">czy stosowac wypelnienie typu OAEP</param>
        /// <returns>zaszyfrowana tablica bajtow</returns>
        public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);

                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }

            catch (CryptographicException e)
            {

                return null;
            }

        }
        /// <summary>
        /// metoda do szyfrowania danych algorytmen AES
        /// </summary>
        /// <param name="text">dane do zaszyfrowania</param>
        /// <param name="key">klucz tajny AES</param>
        /// <param name="iv">wektor inicjalizacji</param>
        /// <returns>zaszyfrowana tablica bajtow</returns>
        public static byte[] AESEncrypt(string text, byte[] key, byte[] iv)
        {
            byte[] encrypted;
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }
        /// <summary>
        /// metoda do deszyfrowania danych algorytmen AES
        /// </summary>
        /// <param name="cipher">dane do odszyfrowania</param>
        /// <param name="key">klucz tajny AES</param>
        /// <param name="iv">>wektor inicjalizacji</param>
        /// <returns>odeszyfrowana tablica bajtow></returns>
        public static string AESDecrypt(byte[] cipher, byte[] key, byte[] iv)
        {
            string text = null;
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipher))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            text = srDecrypt.ReadToEnd();
                        }

                    }
                }
            }

            return text;
        }
        /// <summary>
        /// metoda do szyfrowania wiadomosci algorytmen RSA
        /// </summary>
        /// <param name="message">wiadomosc do zaszyfrowania</param>
        /// <param name="keyInfo">klucze RSA</param>
        /// <returns>zserializowana i zaszyfrowana wiadomosc</returns>
        public static string EncryptMessage(Message message, RSAParameters keyInfo)
        {
            var converter = new UnicodeEncoding();
            var sender = converter.GetBytes(message.Sender);
            var receiver = converter.GetBytes(message.Receiver);
            string encryptedSender = Convert.ToBase64String(RSAEncrypt(sender, keyInfo, false));
            string encryptedReceiver = Convert.ToBase64String(RSAEncrypt(receiver, keyInfo, false));

            return MessageSerializer.Serialize(new Message(encryptedSender, encryptedReceiver, message.data, message.Signature), typeof(Message));

        }
        /// <summary>
        /// metoda do deszyfrowania wiadomosci algorytmen RSA
        /// </summary>
        /// <param name="message">wiadomosc do odszyfrowania</param>
        /// <param name="keyInfo">klucze RSA</param>
        /// <returns>zserializowana i odszyfrowana wiadomosc</returns>
        public static string DecryptMessage(Message message, RSAParameters keyInfo)
        {
            var converter = new UnicodeEncoding();
            var encryptedSender = Convert.FromBase64String(message.Sender);
            var encryptedReceiver = Convert.FromBase64String(message.Receiver);
            string decryptedSender = converter.GetString(RSADecrypt(encryptedSender, keyInfo, false));
            string decryptedReceiver = converter.GetString(RSADecrypt(encryptedReceiver, keyInfo, false));
            return MessageSerializer.Serialize(new Message(decryptedSender, decryptedReceiver, message.data, message.Signature), typeof(Message));
        }
        /// <summary>
        /// metoda do szyfrowania wiadomosci algorytmen AES
        /// </summary>
        /// <param name="message">wiadomosc do zaszyfrowania</param>
        /// <param name="rsaKeyInfo">klucze RSA</param>
        /// <param name="aesKey">klucz tajny AES</param>
        /// <param name="aesIV">wektor inicjalizacji</param>
        /// <returns>zserializowana i zaszyfrowana wiadomosc</returns>
        public static string EncryptMessage(Message message, RSAParameters rsaKeyInfo, byte[] aesKey, byte[] aesIV)
        {
            var converter = new UnicodeEncoding();
            var sender = converter.GetBytes(message.Sender);
            var receiver = converter.GetBytes(message.Receiver);
            string encryptedSender = Convert.ToBase64String(RSAEncrypt(sender, rsaKeyInfo, false));
            string encryptedReceiver = Convert.ToBase64String(RSAEncrypt(receiver, rsaKeyInfo, false));
            string encryptedData = Convert.ToBase64String(AESEncrypt(message.data, aesKey, aesIV));
            return MessageSerializer.Serialize(new Message(encryptedSender, encryptedReceiver, encryptedData, message.Signature), typeof(Message));
        }
        /// <summary>
        /// metoda do deszyfrowania wiadomosci algorytmen AES
        /// </summary>
        /// <param name="message">wiadomosc do odszyfrowania</param>
        /// <param name="rsaKeyInfo">klucze RSA</param>
        /// <param name="aesKey">klucz tajny AES</param>
        /// <param name="aesIV">wektor inicjalizacji</param>
        /// <returns>zserializowana i odszyfrowana wiadomosc</returns>
        public static string DecryptMessage(Message message, RSAParameters rsaKeyInfo, byte[] aesKey, byte[] aesIV)
        {
            var converter = new UnicodeEncoding();
            var encryptedSender = Convert.FromBase64String(message.Sender);
            var encryptedReceiver = Convert.FromBase64String(message.Receiver);
            string decryptedSender = converter.GetString(RSADecrypt(encryptedSender, rsaKeyInfo, false));
            string decryptedReceiver = converter.GetString(RSADecrypt(encryptedReceiver, rsaKeyInfo, false));
            string decryptedData = AESDecrypt(Convert.FromBase64String(message.data), aesKey, aesIV);
            return MessageSerializer.Serialize(new Message(decryptedSender, decryptedReceiver, decryptedData, message.Signature), typeof(Message));
        }
        /// <summary>
        /// metoda do szyfrowania wiadomosci rejestracyjnej algorytmen RSa
        /// </summary>
        /// <param name="message">wiadomosc do zaszyfrowania</param>
        /// <param name="keyInfo">klucze RSA</param>
        /// <returns>zserializowana i zaszyfrowana wiadomosc</returns>
        public static string EncryptMessage(RegisterMessage message, RSAParameters keyInfo)
        {
            var converter = new UnicodeEncoding();
            var username = converter.GetBytes(message.Username);
            var password = converter.GetBytes(message.Password);
            string encryptedUsername = Convert.ToBase64String(RSAEncrypt(username, keyInfo, false));
            string encryptedPassword = Convert.ToBase64String(RSAEncrypt(password, keyInfo, false));
            var encryptedMessage = new RegisterMessage(encryptedUsername, encryptedPassword, message.PublicKey, message.IsRegistred);
            return MessageSerializer.Serialize(encryptedMessage, encryptedMessage.GetType());
        }
        /// <summary>
        /// metoda do szyfrowania wiadomosci rejestracyjnej algorytmen RSa
        /// </summary>
        /// <param name="message">wiadomosc do odszyfrowania</param>
        /// <param name="keyInfo">klucze RSA</param>
        /// <returns>zserializowana i odszyfrowana wiadomosc</returns>
        public static string DecryptMessage(RegisterMessage message, RSAParameters keyInfo)
        {
            var converter = new UnicodeEncoding();
            var username = Convert.FromBase64String(message.Username);
            var password = Convert.FromBase64String(message.Password);
            string decryptedUsername = converter.GetString(RSADecrypt(username, keyInfo, false));
            string decryptedPassword = converter.GetString(RSADecrypt(password, keyInfo, false));
            var decryptedMessage = new RegisterMessage(decryptedUsername, decryptedPassword, message.PublicKey, message.IsRegistred);
            return MessageSerializer.Serialize(decryptedMessage, decryptedMessage.GetType());
        }
    }
}
