using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.MachineLearning
{
    public interface IRandomForestService
    {
        void Train(List<Person> people, int numberOfTrees, int skillSetSize);
        void Test(List<Person> people, int skillSetSize);
        MachineLearningStat ComputeMachineLearningTrainingStat();
        MachineLearningStat ComputeMachineLearningTestingStat();
    }
}
