using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{   /// <summary>
    /// klasa definiajaca uzytkownika komunikatora
    /// </summary>
    public class User
        {  /// <summary>
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
        } /// <summary>
          /// klucze RSA
          /// </summary>
        private RSAParameters rsaKeys;
        /// <summary>
        /// zwraca pole prywatne publicKey
        /// </summary>
        public RSAParameters RSAKeys
        {
            get { return rsaKeys; }
            set { rsaKeys = value; }
        }
        /// <summary>
        /// konstruktor podstawowy obiektu User
        /// </summary>
        public User()
        {
        }
        /// <summary>
        /// konstruktor obiektu User
        /// </summary>
        /// <param name="name">nazwa uzytkownika</param>
        /// <param name="pass">haslo</param>
        /// <param name="keys">klucze RSA</param>
        public User(string name, string pass, RSAParameters keys ) : this()
        {
            username = name;
            password = pass;
            rsaKeys = keys;
        }
    }
}
