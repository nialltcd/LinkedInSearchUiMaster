using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public interface ISkillService
    {
        List<SkillStat> GenerateSkillStats(IEnumerable<Person> people);
        List<SkillStat> ParseSkillStatsFromXml();
        List<SkillStat> ParseSkillStatsWithCountAtLeastTenFromXml();
        List<SkillStat> ParseTopSkillStatsFromXml();
        void WriteSkillStatsToXmlFile(List<SkillStat> jobStats);
        void WriteSkillStatsTopStatisticsToXmlFile(List<SkillStat> jobStats);
        void WriteSkillStatsWithCountOfAtLeastTenToXmlFile(List<SkillStat> skillStats);
    }
}
