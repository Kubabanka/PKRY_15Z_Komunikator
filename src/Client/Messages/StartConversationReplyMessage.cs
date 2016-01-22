using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Messages
{   /// <summary>
    /// klasa definiujaca wiadomosc, za pomoca ktorej rozsylane sa tajne klucze RSA
    /// </summary>
    public class StartConversationReplyMessage : Message
    {
        /// <summary>
        /// wektor inicjalizacji
        /// </summary>
        private byte[] iv;
        /// <summary>
        /// zwraca pole prywatne iv
        /// </summary>
        public byte[] IV
        {
            get { return iv; }
            set { iv = value; }
        }
        /// <summary>
        /// klucz tajny AES
        /// </summary>
        private byte[] key;
        /// <summary>
        /// zwraca pole prywatne key
        /// </summary>
        public byte[] Key { get {return key; }  set { key = value; } }
        /// <summary>
        /// konstruktor podstawowy obiektu StartConversationReplyMessage
        /// </summary>
        public StartConversationReplyMessage ()
        {
            this.Sender = "server";
        }
        /// <summary>
        /// konstruktor obiektu StartConversationReplyMessage
        /// </summary>
        /// <param name="receiver">adresat</param>
        /// <param name="Key">klucz tajny AES</param>
        /// <param name="IV">wektor inicjalizacji</param>
        public StartConversationReplyMessage(string receiver, byte[] Key, byte[]IV) : this()
        {
            this.Receiver = Receiver;
            
            this.IV = IV;
            this.Key = Key;
        }
        /// <summary>
        /// konstruktor obiektu StartConversationReplyMessage
        /// </summary>
        /// <param name="receiver">adresat</param>
        /// <param name="Key">klucz tajny AES</param>
        /// <param name="IV">wektor inicjalizacji</param>
        /// <param name="data">dane</param>
        public StartConversationReplyMessage(string receiver, byte[] Key, byte[] IV,string data) : this(receiver,Key,IV)
        {
            this.data = data;
        }
    }
}
