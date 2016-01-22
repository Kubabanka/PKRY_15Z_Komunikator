using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{   /// <summary>
    /// klasa definiujaca argumenty zdarzenia MessageReceivedEventArgs
    /// </summary>
    public class MessageReceivedEventArgs: EventArgs
    {   
        /// <summary>
        /// wiadomosc
        /// </summary>
        private string message;
        /// <summary>
        /// zwraca pole prywatne message
        /// </summary>
        public string Message
        {
            get { return message; }
        }
        /// <summary>
        /// konstruktoe obiektu MessageReceivedEventArgs
        /// </summary>
        /// <param name="data">wiadomosc</param>
        public MessageReceivedEventArgs(string data)
        {
            message = data;
        }
    }
}
