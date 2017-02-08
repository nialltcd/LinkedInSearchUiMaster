using LinkedInSearchUi.DataTypes;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace LinkedInSearchUi.Indexing
{
    public class CustomXmlWriter
    {
         public void WriteToFile(List<Person> people)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Person>));
            TextWriter tw = new StreamWriter(@"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\XML\data.xml");
            xs.Serialize(tw, people);
        }
    }
}