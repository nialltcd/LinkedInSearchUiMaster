using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.KMeansInfrastructure
{
    public interface IKmeansService
    {
        void Train(List<Person> trainingPeople, int skillSetSize);
        void Test(List<Person> trainingPeople, int skillSetSize);
        MachineLearningStat ComputeMachineLearningTrainingStat();
        MachineLearningStat ComputeMachineLearningTestingStat();

    }
}
