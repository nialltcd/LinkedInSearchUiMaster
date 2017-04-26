using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.MachineLearning
{
    public interface IDataPointService
    {
        double[][] GenerateDataPointsFromPeople(List<Person> people, int skillSetSize);
        int[] GenerateExpectedResultFromPeople(List<Person> people);

    }
}
