using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Messages
{
    /// <summary>
    /// klasa do serilizacji i deserializacji wiadomosci
    /// </summary>
    public static class MessageSerializer
    {
        /// <summary>
        /// metoda do deserializacji wiadomosci
        /// </summary>
        /// <param name="message">
        /// wiadomosc do deserializacji</param>
        /// <param name="type">
        /// typ deserializowanej wiadomosci</param>
        /// <returns>
        /// zdeserializwowana wiadomosc
        /// </returns>
        public static object Deserialize(string message,Type type)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));
            if (type==null)
                throw new ArgumentNullException(nameof(type));
            var serializer = new XmlSerializer(type);
            return serializer.Deserialize(new StringReader(message)); 
        }
        /// <summary>
        /// metoda do serializacji wiadomosci
        /// </summary>
        /// <param name="message">
        /// wiadomosc do serializacji
        /// </param>
        /// <param name="type">
        /// typ serializowanej wiadomosci
        /// </param>
        /// <returns>
        /// zserializowana wiadomosc
        /// </returns>
        public static string Serialize(object message, Type type)
        {
            var serializer = new XmlSerializer(type);
            var writer = new StringWriter();
            var xmlWriter = new XmlTextWriter(writer);
            serializer.Serialize(xmlWriter, message);
            return writer.ToString();
        }
    }
}
