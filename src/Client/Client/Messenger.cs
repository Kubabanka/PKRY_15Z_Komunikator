using Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Client
{   /// <summary>
    /// klasa efiniujaca glowne okno aplikacji Messenger 
    /// </summary>
    public partial class Messenger : Form
    {
        /// <summary>
        /// gniazdo TCP do komunikacji z serwerem 
        /// </summary>
        private TcpClient client;
        /// <summary>
        /// pole przechowujace klucz publiczny serwera
        /// </summary>
        private RSACryptoServiceProvider serverPK;
        /// <summary>
        /// algorytm hashujacy
        /// </summary>
        private SHA256Managed hashAlgorithm = new SHA256Managed();
        /// <summary>
        /// obiekt user typu User
        /// </summary>
        private User user;
        /// <summary>
        /// czy zalogowany
        /// </summary>
        private bool loggedIn = false;
        /// <summary>
        /// slownik przechowujacy wszystkie rozmowy
        /// </summary>
        private Dictionary<string, string> chats;
        /// <summary>
        /// slownik przechowujacy klucze AES
        /// </summary>
        private Dictionary<string, AesManaged> aesDictionary;
        /// <summary>
        /// lista przechowujaca zalogowanych uzytkownikow
        /// </summary>
        private List<User> userList = new List<User>();
        /// <summary>
        /// zdarzenie wywolujace sie po odebraniu wiadomosci
        /// </summary>
        private event EventHandler<MessageReceivedEventArgs> messageReceived;
        /// <summary>
        /// obiekt sluzacy do zblokowania watkow
        /// </summary>
        private object syncRoot = new object();
        /// <summary>
        /// konstruktor podstawowy obietu Messenger
        /// </summary>
        public Messenger()
        {
            InitializeComponent();
            serverPK = new RSACryptoServiceProvider();
            user = new User();
            messageReceived += Form1_messageReceived;
            chats = new Dictionary<string, string>();
            aesDictionary = new Dictionary<string, AesManaged>();
        }
        /// <summary>
        /// metoda definiujaca przyjscie wiadomosci
        /// </summary>
        /// <param name="sender">nadawca</param>
        /// <param name="e">zdarzenie przyjscia wiadomosci</param>
        private void Form1_messageReceived(object sender, MessageReceivedEventArgs e)
        {
            Messages.Message message;
            if (e.Message.Contains("StartConversationReply"))
                message = (Messages.Message)MessageSerializer.Deserialize(e.Message, typeof(StartConversationReplyMessage));
            else
            {
                message = (Messages.Message)MessageSerializer.Deserialize(e.Message, typeof(Messages.Message));
            }

            if (message.Sender.Equals("server"))
            {
                var reply = (StartConversationReplyMessage)message;
                AesManaged aes = new AesManaged();
                aes.IV = reply.IV;
                aes.Key = reply.Key;
                AddOrUpdateAesDictionary(message.data, aes);
                MessageBox.Show("You can write to "+message.data+" now!", "Succes!",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);

            }
            else if (message.Receiver.Equals("Contact list"))
            {
                userList = (List<User>)MessageSerializer.Deserialize(message.data, typeof(List<User>));
                foreach (var item in userList)
                {
                    if (item.Username.Equals(user.Username))
                    {
                        userList.Remove(item);
                        break;
                    }
                }
                onlineUsersListBox.DataSource = userList;
                onlineUsersListBox.DisplayMember = "Username";
            }
            else
            {
                var partiallyDecryptedstr = Security.DecryptMessage(message, user.RSAKeys);
                var partially = (Messages.Message)MessageSerializer.Deserialize(partiallyDecryptedstr, typeof(Messages.Message));
                var aes = GetAes(partially.Sender);
                var decryptedString = Security.DecryptMessage(message, user.RSAKeys, aes.Key, aes.IV);
                var decryptedMessage = (Messages.Message)MessageSerializer.Deserialize(decryptedString, typeof(Messages.Message));
                var rsa = new RSACryptoServiceProvider();
                var aSender = GetSender(decryptedMessage.Sender);
                rsa.ImportParameters(aSender.RSAKeys);
                var converter = new UnicodeEncoding();
                bool verified = rsa.VerifyData(converter.GetBytes(decryptedMessage.data), hashAlgorithm, Convert.FromBase64String(decryptedMessage.Signature));
                if (verified) 
                {

                    var text = string.Format("{0} {1} wrote: {2}", DateTime.Now.ToString(), decryptedMessage.Sender, decryptedMessage.data);
                    AddOrUpdateDictionary(message.Sender, text);

                    messengerRichTextBox.ChangeText(text, false); 
                }
            }
        }
        /// <summary>
        /// metoda aktualizujaca slownik przechowujacy wszystkie rozmowy
        /// </summary>
        /// <param name="key">klucz przeszukiwania slownika</param>
        /// <param name="value">wartosc do dodania</param>
        private void AddOrUpdateDictionary(string key, string value)
        {
            lock (syncRoot)
            {
                if (!chats.ContainsKey(key))
                    chats.Add(key, value);
                else
                    chats[key] += value;
            }
        }
        /// <summary>
        /// metoda zwracajaca nadawce wiaomosci z listy uzytkownikow
        /// </summary>
        /// <param name="name">nazwa uzytkownika</param>
        /// <returns>obiekt uzytkownika o nazwie podanej w argumencie</returns>
        private User GetSender(string name)
        {
            foreach (var user in userList)
            {
                if (user.Username.Equals(name))
                {
                    return user;
                }
            }
            return null;
        }
        /// <summary>
        /// metoda aktualizujaca slownik przechowujacy wszystkie klucze AES
        /// </summary>
        /// <param name="key">klucz przeszukiwania slownika</param>
        /// <param name="value">wartosc do dodania</param>
        private void AddOrUpdateAesDictionary(string key, AesManaged value)
        {
            lock (syncRoot)
            {
                if (!aesDictionary.ContainsKey(key))
                    aesDictionary.Add(key, value);
                else
                    aesDictionary[key] = value;
            }
        }
        /// <summary>
        /// metoda do pobierania wartosci ze slownika przechowujacego wszystkie rozmowy
        /// </summary>
        /// <param name="key">klucz przeszukiwania slownika</param>
        /// <returns>zwraca element slownika zawierajacy klucz</returns>
        private string GetFromDictionary(string key)
        {
            lock (syncRoot)
            {
                return chats[key];
            }
        }
        /// <summary>
        /// metoda sluzaca do sprawdzania czy dany element jest w slowniku przechowujacym wszystkie rozmowy
        /// </summary>
        /// <param name="key">klucz przeszukiwania slownika</param>
        /// <returns>warotsc true jesli element jest w slowniku lub false, gdy do nie ma</returns>
        private bool CheckDictionary(string key)
        {
            lock (syncRoot)
            {
                return chats.ContainsKey(key);
            }
        }
        /// <summary>
        /// metoda sluzaca do zalogowania uzytkownika
        /// </summary>
        /// <param name="obj">obiekt sluzacy do uruchomienia metody</param>
        private void Login(object obj)
        {
            client = new TcpClient("127.0.0.1", 5000);
            var reader = new BinaryReader(client.GetStream());
            var serverPKString = reader.ReadString();
            serverPK.FromXmlString(serverPKString);
            user.Username = loginTextBox.Text;
            user.Password = passwordTextBox.Text;
            bool registered=false;

            RSACryptoServiceProvider pk = new RSACryptoServiceProvider();
            user.RSAKeys = pk.ExportParameters(true);

            if (registerRadioButton.Checked)
            {
                registered = false;
            }
            else if (loginRadioButton.Checked)
            {
                registered = true;
            }
            var message = new RegisterMessage(user.Username, user.Password, pk.ExportParameters(false),registered);
            var messageEncrypted = Security.EncryptMessage(message, serverPK.ExportParameters(false));

            var writer = new BinaryWriter(client.GetStream());
            writer.Write(messageEncrypted);

            var t = reader.ReadString();
            var loginReply = (Messages.Message)MessageSerializer.Deserialize(t, typeof(Messages.Message));
            MessageBox.Show(loginReply.data);


            if (loginReply.data.Equals("Chosen username was free. Account added.") || loginReply.data.Equals("Login succeeded."))
            {
                loginButton.EnableButton(false);
                loggedIn = true; 
            }
            while (loggedIn)
            {
                try
                {
                    var buffer = reader.ReadString();
                    if (messageReceived != null)
                    {
                        messageReceived(this, new MessageReceivedEventArgs(buffer));
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }


        }

        /// <summary>
        /// metoda definiujaca zdarzenie wcisniecia przycisku logowania
        /// </summary>
        /// <param name="sender">nadawca zdarzenia</param>
        /// <param name="e">zdarzenie wcisnieca przycisku logowania</param>
        private void loginButton_Click(object sender, EventArgs e)
        {
            var networkThread = new Thread(new ParameterizedThreadStart(Login));
            networkThread.IsBackground = true;
            networkThread.Start();

        }

        
        /// <summary>
        /// metoda sluzaca do wygenerowania podpisu cyfrowego z obietu typu string
        /// </summary>
        /// <param name="message">wiadomosc do podpisania</param>
        /// <returns>podpisana podpisem cyfrowym wiadomosc</returns>
        private string SignString(string message)
        {
            var converter = new UnicodeEncoding();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(user.RSAKeys);
            var signature = rsa.SignData(converter.GetBytes(message), hashAlgorithm);
            return Convert.ToBase64String(signature);
        }
        /// <summary>
        /// metoda sluzaca do sprawdzania czy dany element jest w slowniku przechowujacym klucze AES 
        /// </summary>
        /// <param name="key">klucz przeszukiwania slownika</param>
        /// <returns>warotsc true jesli element jest w slowniku lub false, gdy do nie ma</returns>
        private bool CheckAesDictionary(string key)
        {
            {
                lock (syncRoot)
                {
                    return aesDictionary.ContainsKey(key);
                }
            }
        }
        /// <summary>
        /// metoda sluzaca do wyslania wiadomosci do drugiego uzytkownika
        /// </summary>
        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(MessageInputTextBox.Text))
            {
                var selected = (User)onlineUsersListBox.SelectedItem;
                var writer = new BinaryWriter(client.GetStream());
                var aes = GetAes(selected.Username);
                var message = new Messages.Message(user.Username, selected.Username, MessageInputTextBox.Text, SignString(MessageInputTextBox.Text));
                var encryptedMessage = Security.EncryptMessage(message, serverPK.ExportParameters(false), aes.Key, aes.IV);
                var text = string.Format("{0} You wrote: {1} ", DateTime.Now.ToString(), MessageInputTextBox.Text);
                AddOrUpdateDictionary(selected.Username, text);
                messengerRichTextBox.ChangeText(text, true);
                MessageInputTextBox.Text = string.Empty;
                writer.Write(encryptedMessage); 
            }
        }
        /// <summary>
        /// metoda definiujaca zdarzenie wcisniecia klawisza enter znajdujac sie w polu sluzacym do wpisywania wiadomosci
        /// </summary>
        /// <param name="sender">nadawca zdarzenia</param>
        /// <param name="e">zdarzenie nacisniecia klawisza enter</param>
        private void MessageInputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            { 
                SendMessage();
            }
        }
        /// <summary>
        /// metoda sluzaca do pobrania klucza AES dla danego uzytkownika
        /// </summary>
        /// <param name="key">klucz przeszukiwania slownika</param>
        /// <returns>zwraca lucz AES danego uzytkownika</returns>
        private AesManaged GetAes(string key)
        {
            lock(syncRoot)
            {
                if (aesDictionary.ContainsKey(key))
                    return aesDictionary[key];
                else
                    return null;
            }
        }
        /// <summary>
        /// metoda definiujaca zdarzenie podniesienia klawisza enter znajdujac sie w polu sluzacym do wpisywania wiadomosci
        /// </summary>
        /// <param name="sender">nadawca zdarzenia</param>
        /// <param name="e">zdarzenie podniesienia klawisza enter</param>
        private void MessageInputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
                MessageInputTextBox.Text = string.Empty;

        }
        /// <summary>
        /// metoda definiujaca zdarzenie zamkniecia obiektu typu Messenger
        /// </summary>
        /// <param name="sender">nadawca zdarzenia</param>
        /// <param name="e">zdarzenie zamkniecia obiektu typu Messenger</param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Send(MessageSerializer.Serialize(new Messages.Message(user.Username, string.Empty, string.Empty,string.Empty), typeof(Messages.Message)));
            if (client!=null)
                 client.Close();
            File.WriteAllText(@"./Logs/"+user.Username+".xml", messengerRichTextBox.Text);

        }
        /// <summary>
        /// metoda sluzaca do wyslania wiadomosci do danego uzytkownika
        /// </summary>
        /// <param name="text">tekst wiadomosci</param>
        public void Send(string text)
        {
            if (client == null)
                return;
            var writer = new BinaryWriter(client.GetStream());
            writer.Write(text);
        }
        /// <summary>
        /// metoda sluzaca do wyslania prosby o klucz danego uzytkownika do serwera
        /// </summary>
        private void GetAesKey()
        {
            var selected = (User)onlineUsersListBox.SelectedItem;
            var writer = new BinaryWriter(client.GetStream());
            var start = new Messages.Message(user.Username, "server", selected.Username, SignString(selected.Username));
            var startEncrypted = Security.EncryptMessage(start, serverPK.ExportParameters(false));
            writer.Write(startEncrypted);
        }


        /// <summary>
        /// metoda definiujaca zdarzenie wybrania danego uzytkownika z listy uzytkownikow
        /// </summary>
        /// <param name="sender">nadawca zdarzenia</param>
        /// <param name="e">zdarzenie wybrania danego uzytkownika z listy uzytkownikow</param>
        private void onlineUsersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = (User)onlineUsersListBox.SelectedItem;
            StartConversationButton.Enabled = !CheckAesDictionary(selected.Username);
            MessageInputTextBox.Enabled = CheckAesDictionary(selected.Username);
        }
        /// <summary>
        /// metoda definiujaca zdarzenie nacisniecia przycisku startu konwersacji 
        /// </summary>
        /// <param name="sender">nadawca zdarzenia</param>
        /// <param name="e">zdarzenie nacisniecia przycisku startu konwersacji</param>
        private void StartConversationButton_Click(object sender, EventArgs e)
        {
            GetAesKey();
            MessageInputTextBox.Enabled = true;
            StartConversationButton.Enabled = false;
        }
        /// <summary>
        /// metoda definiujaca zdarzenie nacisniecia przycisku wyslania wiadomosci
        /// </summary>
        /// <param name="sender">nadawca zdarzenia</param>
        /// <param name="e">zdarzenie nacisniecia przycisku startu konwersacji</param>
        private void SendMessageButton_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        
    }
}
