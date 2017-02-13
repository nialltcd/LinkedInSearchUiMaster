using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.DataTypes
{
    public class Job
    {
        public string Name;
        public List<Person> Employees { get; set; }
    }
}
