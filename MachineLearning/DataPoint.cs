using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.MachineLearning
{
    public class DataPoint
    {
        public DataPoint()
        {
            Attributes = new List<Attribute>();
        }
        public string Name { get; set; }
        public List<Attribute> Attributes { get; set; }
        public int Cluster { get; set; }
    }
}
