using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public interface ICustomXmlWriter<T>
    {
        void WriteToFile(List<T> list, string filePath);
        List<T> ReadFromFile(string filePath);
    }
}
