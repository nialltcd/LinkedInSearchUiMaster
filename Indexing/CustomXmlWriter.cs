using LinkedInSearchUi.DataTypes;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace LinkedInSearchUi.Indexing
{
    public class CustomXmlService
    {
        private XmlSerializer _serializer;
        public CustomXmlService()
        {
            _serializer = new XmlSerializer(typeof(List<Person>));
        }


         public void WriteToFile(List<Person> people, string filePath)
        {
            TextWriter tw = new StreamWriter(filePath);
            _serializer.Serialize(tw, people);
        }

        public List<Person> ReadFromFile(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            List<Person> people = (List<Person>)_serializer.Deserialize(reader);
            reader.Close();
            return people;
        }

    }
}