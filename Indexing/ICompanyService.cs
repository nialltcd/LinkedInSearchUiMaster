using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public interface ICompanyService
    {
        List<CompanyStat> GenerateCompanyStats(IEnumerable<Person> people);
        List<CompanyStat> ParseCompanyStatsFromXml();
        List<CompanyStat> ParseTopCompanyStatsFromXml();
        void WriteCompanyStatsToXmlFile(List<CompanyStat> jobStats);
        void WriteCompanyStatsTopStatisticsToXmlFile(List<CompanyStat> jobStats);
    }
}
