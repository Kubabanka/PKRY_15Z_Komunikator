using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    /// <summary>
    /// klasa definiujaca podstawowy typ przesylanej wiadomosci
    /// </summary>
    public class Message
    {
        /// <summary>
        /// nadawca wiadomosci
        /// </summary>
        private string sender;
        /// <summary>
        /// zwraca pole prywatne sender
        /// </summary>
        public string Sender
        {
            get { return sender; }
            set { sender = value;}
        }
        /// <summary>
        /// adresat wiadomosci
        /// </summary>
        private string receiver;
        /// <summary>
        /// zwraca pole prywatne receiver
        /// </summary>
        public string Receiver
        {
            get { return receiver; }
            set { receiver = value; }
        }

        /// <summary>
        /// tresc wiadomosci
        /// </summary>
        public string data;

        /// <summary>
        /// podpis cyfrowy
        /// </summary>
        private string signature;
        /// <summary>
        /// zwraca pole prywatne signature
        /// </summary>
        public string Signature
        {
            get { return signature; }
            set { signature = value; }
        }
        /// <summary>
        /// konstruktor podstawowy obiektu Message
        /// </summary>
        public Message()
        {
        
        }
        /// <summary>
        /// konstruktor obiektu Message
        /// </summary>
        /// <param name="sender">nadawca wiadomosci</param>
        /// <param name="receiver">adresat wiadomosci</param>
        /// <param name="data">tresc wiadomosci</param>
        /// <param name="sign">podpis cyfrowy</param>
        public Message(string sender, string receiver, string data, string sign)
        {
            this.sender = sender;
            this.receiver = receiver;
            this.data = data;
            this.signature = sign;
        }
    }
}
