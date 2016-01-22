using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{   
    /// <summary>
    /// klasa definiujaca argumenty zdarzenia przyjscia wiadomosci
    /// </summary>
    public class RequestEventArgs : EventArgs
    {
        /// <summary>
        /// nazwa uzytkownika
        /// </summary>
        private string username;
        /// <summary>
        /// zwraca prywatne pole username
        /// </summary>
        public string Username
        {
            get { return username; }
        }
        /// <summary>
        /// wiadomosc
        /// </summary>
        private string message;
        /// <summary>
        /// zwraca prywatne pole wiadomosc
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// konstruktor obiektu RequestEventArgs
        /// </summary>
        /// <param name="name">nazwa uzytkownika</param>
        /// <param name="data">wiadomosc</param>
        public RequestEventArgs(string name, string data)
        {
            username = name;
            message = data;
        }
    }
}
