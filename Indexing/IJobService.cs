using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public interface IJobService
    {
        List<JobStat> GenerateJobStats(IEnumerable<Person> people);
        List<JobStat> ParseJobStatsFromXml();
        List<JobStat> ParseTopJobStatsFromXml();
        void WriteJobStatsToXmlFile(List<JobStat> jobStats);
        void WriteJobStatsTopStatisticsToXmlFile(List<JobStat> jobStats);
    }
}
