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
        void Perform(int numberOfClusters, List<Person> people, List<SkillStat> skills);
    }
}
