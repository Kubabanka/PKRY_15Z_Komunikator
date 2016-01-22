using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class StartConversationMessage: Message
    {

        private string destinationUser;
        public string DestinationUser
        {
            get { return destinationUser; }
            set { destinationUser = value; }
        }
            
        public StartConversationMessage()
        {
            Receiver = "server";
        }
         
    }
}
