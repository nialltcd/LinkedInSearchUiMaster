using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public class SkillService : ISkillService
    {
        private CustomXmlService<SkillStat> _skillStatsCustomXmlService;
        private string _allSkillStatsXmlFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\AllTrainingSkillStats.xml";
        private string _skillStatsTopStatisticsXmlFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\Top100SkillStats.xml";
        private string _skillStatsTopStatisticsTextFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\Top100SkillStats.txt";
        private string _skillStatsWithCountOfAtLeastTenXmlFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\SkillStatsWithCountOfAtLeastTen.txt";
        public SkillService()
        {
            _skillStatsCustomXmlService = new CustomXmlService<SkillStat>();
        }

        public List<SkillStat> GenerateSkillStats(IEnumerable<Person> people)
        {
            List<SkillStat> allSkills = new List<SkillStat>();
            int count = 0;
            foreach (var person in people)
            {
                foreach (var skill in person.Skills)
                {
                    if (skill != null)
                    {
                        int index = allSkills.FindIndex(t => string.Equals(t.Name, skill.Name, StringComparison.OrdinalIgnoreCase));
                        if (index != -1)
                        {
                            allSkills[index].Count++;
                        }
                        if (index == -1)
                        {
                            allSkills.Add(new SkillStat() { Name = skill.Name });
                        }
                    }
                }
                count++;
                if (count % 10000 == 0)
                    Console.WriteLine(count);
            }
            return allSkills;
        }

        public List<SkillStat> ParseSkillStatsFromXml()
        {
            return _skillStatsCustomXmlService.ReadFromFile(_allSkillStatsXmlFilePath);
        }

        public List<SkillStat> ParseSkillStatsWithCountAtLeastTenFromXml()
        {
            return _skillStatsCustomXmlService.ReadFromFile(_skillStatsWithCountOfAtLeastTenXmlFilePath);
        }

        public List<SkillStat> ParseTopSkillStatsFromXml()
        {
            return _skillStatsCustomXmlService.ReadFromFile(_skillStatsTopStatisticsXmlFilePath);
        }

        public void WriteSkillStatsToXmlFile(List<SkillStat> jobStats)
        {
            var allSkills = jobStats.OrderByDescending(t => t.Count).ToList();
            _skillStatsCustomXmlService.WriteToFile(allSkills, _allSkillStatsXmlFilePath);
        }

        public void WriteSkillStatsTopStatisticsToXmlFile(List<SkillStat> skillStats)
        {
            var mostFrequentSkills = skillStats.OrderByDescending(t => t.Count).Take(100).ToList();
            _skillStatsCustomXmlService.WriteToFile(mostFrequentSkills, _skillStatsTopStatisticsXmlFilePath);
        }

        public void WriteSkillStatsWithCountOfAtLeastTenToXmlFile(List<SkillStat> skillStats)
        {
            var mostFrequentSkills = skillStats.Where(t => t.Count >= 10).ToList();
            _skillStatsCustomXmlService.WriteToFile(mostFrequentSkills, _skillStatsWithCountOfAtLeastTenXmlFilePath);
        }
    }
}
