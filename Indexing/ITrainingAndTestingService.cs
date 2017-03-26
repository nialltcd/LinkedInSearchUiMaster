using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public interface ITrainingAndTestingService
    {
        void CreateTrainingAndTestSetsBasedOnJob();
        void CreateTrainingAndTestSetsBasedOnCompany();
        void CreateTrainingAndTestingSetNewRequirements(List<Person> people);
        void CreateTrainingAndTestingSetBasedOnSingleJob(List<Person> people);
    }
}
