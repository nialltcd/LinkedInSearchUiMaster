using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public class CompanyService : ICompanyService
    {
        private CustomXmlService<CompanyStat> _companyStatsCustomXmlService;
        private string _allCompanyStatsXmlFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\AllCompanyStats.xml";
        private string _companyStatsTopStatisticsXmlFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\Top100CompanyStats.xml";
        private string _companyStatsTopStatisticsTextFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\Top100CompanyStats.txt";

        public CompanyService()
        {
            _companyStatsCustomXmlService = new CustomXmlService<CompanyStat>();
        }

        public List<CompanyStat> GenerateCompanyStats(IEnumerable<Person> people)
        {
            List<CompanyStat> companyStats = new List<CompanyStat>();
            int count = 0;
            foreach (var person in people)
            {
                var experience = person.Experiences.FirstOrDefault();
                if (experience != null)
                {
                    int index = companyStats.FindIndex(t => string.Equals(t.CompanyName, experience.Organisation, StringComparison.OrdinalIgnoreCase));

                    if (index != -1)
                    {
                        companyStats[index].Count++;
                    }
                    else
                    {
                        companyStats.Add(new CompanyStat()
                        {
                            CompanyName = person.Experiences.FirstOrDefault().Organisation,
                            Count = 1
                        });
                    }
                }
                count++;
                if (count % 10000 == 0)
                    Console.WriteLine(count);
            }
            return companyStats;
        }

        public List<CompanyStat> ParseCompanyStatsFromXml()
        {
            return _companyStatsCustomXmlService.ReadFromFile(_allCompanyStatsXmlFilePath);
        }

        public List<CompanyStat> ParseTopCompanyStatsFromXml()
        {
            return _companyStatsCustomXmlService.ReadFromFile(_companyStatsTopStatisticsXmlFilePath);
        }

        public void WriteCompanyStatsToXmlFile(List<CompanyStat> jobStats)
        {
            var mostFrequentCompanies = jobStats.OrderByDescending(t => t.Count).ToList();
            _companyStatsCustomXmlService.WriteToFile(mostFrequentCompanies, _allCompanyStatsXmlFilePath);
        }

        public void WriteCompanyStatsTopStatisticsToXmlFile(List<CompanyStat> jobStats)
        {
            var mostFrequentCompanies = jobStats.OrderByDescending(t => t.Count).Take(100).ToList();
            _companyStatsCustomXmlService.WriteToFile(mostFrequentCompanies, _companyStatsTopStatisticsXmlFilePath);
        }
    }
}
