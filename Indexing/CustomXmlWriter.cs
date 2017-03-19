using LinkedInSearchUi.DataTypes;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace LinkedInSearchUi.Indexing
{
    public class CustomXmlService<T> : ICustomXmlWriter<T>
    {
        private XmlSerializer _serializer;

        public CustomXmlService()
        {
            _serializer = new XmlSerializer(typeof(List<T>));
        }


         public void WriteToFile(List<T> list, string filePath)
        {
            TextWriter tw = new StreamWriter(filePath);
            _serializer.Serialize(tw, list);
        }

        public List<T> ReadFromFile(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            var list = (List<T>)_serializer.Deserialize(reader);
            reader.Close();
            return list;
        }

    }
}