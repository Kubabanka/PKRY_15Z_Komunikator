using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Messages
    
{   /// <summary>
    ///klasa definiujaca typ wiadomosci rejestracyjnej 
    /// </summary>
    public class RegisterMessage : Message
    {
        /// <summary>
        /// nazwa uzytkownika
        /// </summary>
        private string username;
        /// <summary>
        /// zwraca pole prywatne username
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        /// <summary>
        /// wartosc mowiaca czy uzytkownik jest zarejestrowany
        /// </summary>
        private bool isRegistered;
        /// <summary>
        /// zwraca pole prywatne isRegistered
        /// </summary>
        public bool IsRegistred
        {
            get { return isRegistered; }
            set { isRegistered = value; }
        }
        /// <summary>
        /// haslo
        /// </summary>
        private string password;
        /// <summary>
        /// zwraca pole prywatne password
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        /// <summary>
        /// klucz publiczny RSA
        /// </summary>
        private RSAParameters publicKey;
        /// <summary>
        /// zwraca pole prywatne publicKey
        /// </summary>
        public RSAParameters PublicKey
        {
            get { return publicKey; }
            set { publicKey = value; }
        }
        /// <summary>
        /// konstruktor podstawowy obiektu RegisterMessage
        /// </summary>
        public RegisterMessage() { }
        /// <summary>
        /// konstruktor obiektu RegisterMessage
        /// </summary>
        /// <param name="name">nazwa uzytkownika</param>
        /// <param name="password">haslo</param>
        /// <param name="publicKey">klucz publiczny RSA</param>
        /// <param name="registered">czy zarejestrowany</param>
        public RegisterMessage(string name, string password, RSAParameters publicKey, bool registered)
        {
            username = name;
            this.password = password;
            this.publicKey = publicKey;
            this.isRegistered = registered;
        }

    }
}
