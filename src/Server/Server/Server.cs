using Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Server
{
    /// <summary>
    /// klasa definiujaca serwer TCP obslugujacy zdarzenia
    /// </summary>
    public class Server
    {
        /// <summary>
        /// slownik przechowujacy gniazda TCP zalogowanych klientow
        /// </summary>
        private Dictionary<string,TcpClient> onlineClients;
        /// <summary>
        /// slownik przechowujacy wszystkich uzytkownikow
        /// </summary>
        private Dictionary<string, User> usersInformation;
        /// <summary>
        /// algorytm hashujacy
        /// </summary>
        private SHA256Managed hashAlgorithm;
        /// <summary>
        /// zdarzenie wywolujace sie po odebraniu wiadomosci
        /// </summary>
        private event EventHandler<RequestEventArgs> requestReceived;
        /// <summary>
        /// obiekt sluzacy do zblokowania watkow
        /// </summary>
        private static readonly object syncRoot = new object();
        /// <summary>
        /// generator kluczy AES
        /// </summary>
        private AesManaged aesKeyGenerator;
        /// <summary>
        /// generator kluczy RSA
        /// </summary>
        private RSACryptoServiceProvider rsaKeyGenerator;
        /// <summary>
        /// konstruktor obiektu Server 
        /// </summary>
        public Server()
        {
            requestReceived += Server_requestReceived;
            aesKeyGenerator = new AesManaged();
            hashAlgorithm = new SHA256Managed();
            rsaKeyGenerator = new RSACryptoServiceProvider();      
        }


        /// <summary>
        /// metoda inicjalizujaca server
        /// </summary>
        public void Initialize()
        {
            var listener = new TcpListener(IPAddress.Any,5000);
            listener.Start();
            onlineClients = new Dictionary<string, TcpClient>();
            usersInformation = GetUsers();
            while (true)
            {
                var client = listener.AcceptTcpClient();
                var clientThread = new Thread(new ParameterizedThreadStart(HandleClientRequest));
                clientThread.IsBackground = true;
                clientThread.Start(client);
            }            
        }
        /// <summary>
        /// metoda sluzaca do pobrania uzytkownikow z pliku XML
        /// </summary>
        /// <returns>slownik zawierajacy uzytkownikow </returns>
        private Dictionary<string,User> GetUsers()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("usersnames.xml");
            var list = (List<User>)MessageSerializer.Deserialize(xmlDoc.InnerXml, typeof(List<User>));
            var dictonary = new Dictionary<string, User>();
            foreach (var user in list)
            {
                dictonary.Add(user.Username, user);
            }
            return dictonary;
        }
        /// <summary>
        /// metoda efiniujaca przyjscie zadan klienta
        /// </summary>
        /// <param name="obj">zdarzenie przyjscia zadania klienta</param>
        private void HandleClientRequest(object obj)
        {
            var converter = new UnicodeEncoding();
            var client = (TcpClient)obj;
            var clientStream = client.GetStream();
            var keyWriter = new BinaryWriter(clientStream);
            keyWriter.Write(rsaKeyGenerator.ToXmlString(false));
            var reader = new BinaryReader(clientStream);
            try
            {
                string input = reader.ReadString();
                var encryptedRegisterMessage = (RegisterMessage) MessageSerializer.Deserialize(input, typeof(RegisterMessage));
                var decryptedRegisterMessagestr = Security.DecryptMessage(encryptedRegisterMessage, rsaKeyGenerator.ExportParameters(true));
                var decryptedRegisterMessage = (RegisterMessage)MessageSerializer.Deserialize(decryptedRegisterMessagestr, typeof(RegisterMessage));
                var clientName = decryptedRegisterMessage.Username;
                if (!String.IsNullOrEmpty(clientName))
                {
                    if (CheckDictionary(clientName))
                    {
                        if (!decryptedRegisterMessage.IsRegistred)
                        {
                            var writer = new BinaryWriter(clientStream);
                            string signature = Convert.ToBase64String(rsaKeyGenerator.SignData(converter.GetBytes("Chosen username is not available.Choose different one."), hashAlgorithm));
                            var mes = new Message("Server", clientName, "Chosen username is not available.Choose different one.",signature);
                            var str = MessageSerializer.Serialize(mes, typeof(Message));
                            writer.Write(str);
                            return;  
                        }
                        else
                        {
                            var writer = new BinaryWriter(clientStream);
                            Message mes1;
                            if (CheckPassword(decryptedRegisterMessage.Username, decryptedRegisterMessage.Password))
                            {
                                string signature = Convert.ToBase64String(rsaKeyGenerator.SignData(converter.GetBytes("Login succeeded."), hashAlgorithm));
                                mes1 = new Message("Server", clientName, "Login succeeded.",signature);
                            }
                            else
                            {
                                string signature = Convert.ToBase64String(rsaKeyGenerator.SignData(converter.GetBytes("Login failed."), hashAlgorithm));
                                mes1 = new Message("Server", clientName, "Login failed.",signature);
                                var stringMes1 = MessageSerializer.Serialize(mes1, typeof(Message));
                                writer.Write(stringMes1);
                                return;
                            }
                           
                            var str = MessageSerializer.Serialize(mes1, typeof(Message));
                            writer.Write(str);
                            UpdateList();
                        }
                    }
                    else
                    {
                        AddToDictonary(clientName, client);
                        var hashedPass = Convert.ToBase64String(hashAlgorithm.ComputeHash(converter.GetBytes(decryptedRegisterMessage.Password)));
                        AddToDictonary(clientName, new User(clientName, hashedPass, decryptedRegisterMessage.PublicKey));
                        var writer = new BinaryWriter(clientStream);
                        string signature = Convert.ToBase64String(rsaKeyGenerator.SignData(converter.GetBytes("Chosen username was free. Account added."), hashAlgorithm));
                        var mes = new Message("Server", clientName, "Chosen username was free. Account added.",signature);
                        var str = MessageSerializer.Serialize(mes, typeof(Message));
                        writer.Write(str);
                        UpdateList();
                        SaveUsers();
                    }
                }
                while (true)
                {
                    var request = reader.ReadString();
                    if (requestReceived != null)
                        requestReceived(this, new RequestEventArgs(clientName, request));
                }
                
            }
            catch (Exception ex)
            {  }
        }
        /// <summary>
        /// metoda do weryfikacji hasla uzytkownika
        /// </summary>
        /// <param name="name">nazwa uzytkownika</param>
        /// <param name="pass">haslo</param>
        /// <returns>wartos true, gdy haslo jest poprawne i false, gdy haslo jest niepoprawne</returns>
        private bool CheckPassword(string name,string pass)
        {
            var converter = new UnicodeEncoding();
            var hashedPass = Convert.ToBase64String(hashAlgorithm.ComputeHash(converter.GetBytes(pass)));
            var user = GetFromUsersDictonary(name);
            return hashedPass.Equals(user.Password);
        }
        /// <summary>
        /// metoda sluzaca do zapisywania informacji o uzytkownikach do pliku XML
        /// </summary>
        private void SaveUsers()
        {
            var users = usersInformation.Values.ToList();
            var clientList = MessageSerializer.Serialize(users, users.GetType());
            var xmlDoc = new XmlDocument();
            xmlDoc.InnerXml = clientList;
            xmlDoc.Save("usersnames.xml");
        }
        /// <summary>
        /// metoda sluzaca do rozeslania listy dostepnych uzytkownikow do zalogowanych uzytkownikow
        /// </summary>
        private void UpdateList()
        {
            var converter = new UnicodeEncoding();
            var users = new List<User>();
            foreach (var key in onlineClients.Keys)
            {
                users.Add(GetFromUsersDictonary(key));
            }
            var clientList = MessageSerializer.Serialize(users, users.GetType());
            foreach (var client in onlineClients.Values)
            {
                var clientStream = client.GetStream();
                var writer = new BinaryWriter(clientStream);
                string signature = Convert.ToBase64String(rsaKeyGenerator.SignData(converter.GetBytes(clientList), hashAlgorithm));
                var mes = new Message("Server", "Contact list", clientList,signature);
                var str = MessageSerializer.Serialize(mes, typeof(Message));
                writer.Write(str);                
            }
            
        }
        /// <summary>
        /// metoda do obslugi przyjscia zadania od klienta
        /// </summary>
        /// <param name="sender">nadawca zdarzenia</param>
        /// <param name="e">zdarzenie przyjscia wiadomosci od klienta</param>
        private void Server_requestReceived(object sender, RequestEventArgs e)
        {
            string clientName = e.Username;
            string messageString = e.Message;
            var encrptedMessage = (Message)MessageSerializer.Deserialize(messageString, typeof(Message));
            var decryptedMessage = Security.DecryptMessage(encrptedMessage, rsaKeyGenerator.ExportParameters(true));
            var message = (Message)MessageSerializer.Deserialize(decryptedMessage, typeof(Message));
            if (string.IsNullOrEmpty(message.Receiver) )
            {
                var client = GetFromClientDictonary(clientName);
                client.Close();
                RemoveFromDictonary(clientName);
                UpdateList();

            }
            else if (message.Receiver.Equals("server"))
            {
                aesKeyGenerator.GenerateKey();
                aesKeyGenerator.GenerateIV();
                var reply = new StartConversationReplyMessage(clientName, aesKeyGenerator.Key, aesKeyGenerator.IV);
                var client = GetFromClientDictonary(clientName);
                var writer = new BinaryWriter(client.GetStream());
                writer.Write(MessageSerializer.Serialize(new StartConversationReplyMessage(clientName, aesKeyGenerator.Key, aesKeyGenerator.IV,message.data), typeof(StartConversationReplyMessage)));
                client = GetFromClientDictonary(message.data);
                writer = new BinaryWriter(client.GetStream());
                writer.Write(MessageSerializer.Serialize(new StartConversationReplyMessage(message.data, aesKeyGenerator.Key, aesKeyGenerator.IV,clientName), typeof(StartConversationReplyMessage)));
            }
            else
            {
                var aReceiver = GetFromUsersDictonary(message.Receiver);
                var receiver = GetFromClientDictonary (message.Receiver);
                //#region do niezaprzeczalności
                //var signature = Convert.FromBase64String(message.Signature);
                //if (signature[3] == 12)
                //    signature[3] = 72;
                //else
                //    signature[3] = 12;
                //var changed = Convert.ToBase64String(signature);
                //message.Signature = changed;
                //#endregion
                var encrypted = Security.EncryptMessage(message, aReceiver.RSAKeys);
                var writer = new BinaryWriter(receiver.GetStream());
                writer.Write(encrypted);
            }
        }
        /// <summary>
        /// metoda do przeszukiwnania slownika
        /// </summary>
        /// <param name="key">klucz przeszukiwania slownika</param>
        /// <returns>wartosc true, gdy uzytkownik znajduje sie w obu slownikach lub wartosc false gdy nie znajduje sie w ktoryms z nich</returns>
        private bool CheckDictionary(string key)
        {
            lock (syncRoot)
            {
                return usersInformation.ContainsKey(key);
            }
        }
        /// <summary>
        /// metoda sluzaca do dodawania uzytkownika do slownika klientow online
        /// </summary>
        /// <param name="key">klucz dodawania do slownika</param>
        /// <param name="value">klient TCP, ktorego odajemy do slownika klientow online</param>
        private void AddToDictonary(string key, TcpClient value)
        {
            lock (syncRoot)
            {
                onlineClients.Add(key, value);
            }
        }
        /// <summary>
        /// metoda sluzaca do dodawania uzytkownika do slownika klientow
        /// </summary>
        /// <param name="key">klucz dodawania do slownika</param>
        /// <param name="value">uzytkownik, ktorego dodajemy do slownika uzytkownikow</param>
        private void AddToDictonary(string key, User value)
        {
            lock (syncRoot)
            {
                usersInformation.Add(key, value);
            }
        }
        /// <summary>
        /// metoda do usuwania uzytkownikow ze slownika uzytkownikow online
        /// </summary>
        /// <param name="key">klucz usuwania ze slownika uzytkownikow online</param>
        private void RemoveFromDictonary(string key)
        {
            lock (syncRoot)
            {
                if (onlineClients.ContainsKey(key))
                onlineClients.Remove(key);
            }
        }
        /// <summary>
        /// metoda do pobierania danego uzytkownika online ze slownika uzytkownikow online
        /// </summary>
        /// <param name="key">parametr przeszukiwania slownika</param>
        /// <returns>socket danego uzytkownika online</returns>
        private TcpClient GetFromClientDictonary(string key)
        {
            lock (syncRoot)
            {
                if (onlineClients.ContainsKey(key))
                    return onlineClients[key];
            }
            return null;
        }
        /// <summary>
        /// metoda do pobierania danego uzytkownika ze slownika uzytkownikow
        /// </summary>
        /// <param name="key">parametr przeszukiwania slownika</param>
        /// <returns>danego uzytkownika</returns>
        private User GetFromUsersDictonary(string key)
        {
            lock (syncRoot)
            {
                //if (onlineClients.ContainsKey(key))
                    return usersInformation[key];
            }
            return null;
        }
    }
}
