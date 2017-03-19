using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public class CompanyJobPairService : ICompanyJobPairService
    {
        private CustomXmlService<CompanyJobPair> _companyJobPairCustomXmlService;
        private string _allCompanyJobPairsXmlFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\AllCompanyJobPairs.xml";
        private string _companyJobPairsTopStatisticsXmlFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\Top100CompanyJobPairs.xml";
        private string __companyJobPairsTopStatisticsTextFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\Top100CompanyJobPairs.txt";

        public CompanyJobPairService()
        {
            _companyJobPairCustomXmlService = new CustomXmlService<CompanyJobPair>();
        }

        public List<CompanyJobPair> GenerateCompanyJobPairs(IEnumerable<Person> people)
        {
            List<CompanyJobPair> companyJobPairs = new List<CompanyJobPair>();
            int count = 0;
            foreach (var person in people)
            {
                var company = person.Experiences.FirstOrDefault();
                if (company != null)
                {
                    int index = companyJobPairs.FindIndex(t => string.Equals(t.CompanyName, company.Organisation, StringComparison.OrdinalIgnoreCase) && string.Equals(t.JobName, company.Role, StringComparison.OrdinalIgnoreCase));

                    if (index >= 0)
                    {
                        companyJobPairs[index].Count++;
                    }
                    else
                    {
                        companyJobPairs.Add(new CompanyJobPair()
                        {
                            CompanyName = person.Experiences.FirstOrDefault().Organisation,
                            JobName = person.Experiences.FirstOrDefault().Role,
                            Count = 1
                        });
                    }
                }
                count++;
                if (count % 10000 == 0)
                    Console.WriteLine(count);
            }
            return companyJobPairs;
        }

        public List<CompanyJobPair> ParseCompanyJobPairsFromXml()
        {
            var companyAndJobPairs = _companyJobPairCustomXmlService.ReadFromFile(_allCompanyJobPairsXmlFilePath);
            return companyAndJobPairs;
        }

        public List<CompanyJobPair> ParseTopCompanyJobPairsFromXml()
        {
            var companyAndJobPairs = _companyJobPairCustomXmlService.ReadFromFile(_companyJobPairsTopStatisticsXmlFilePath);
            return companyAndJobPairs;
        }

        public void WriteCompanyJobPairsToXmlFile(List<CompanyJobPair> companyJobPairs)
        {
            var mostFrequentCompanyJobPairs = companyJobPairs.OrderByDescending(t => t.Count).ToList();
            _companyJobPairCustomXmlService.WriteToFile(mostFrequentCompanyJobPairs, _allCompanyJobPairsXmlFilePath);
        }

        public void WriteCompanyJobPairsTopStatisticsToXmlFile(List<CompanyJobPair> companyJobPairs)
        {
            var mostFrequentCompanyJobPairs = companyJobPairs.OrderByDescending(t => t.Count).Take(100).ToList();
            _companyJobPairCustomXmlService.WriteToFile(mostFrequentCompanyJobPairs, _companyJobPairsTopStatisticsXmlFilePath);
        }

        public void WriteCompanyJobPairsTopStatisticsToFileFormatted(List<CompanyJobPair> companyJobPairs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var companyJobPair in companyJobPairs)
            {
                sb.AppendLine(companyJobPair.CompanyName + "," + companyJobPair.JobName + "," + companyJobPair.Count);
            }
            File.WriteAllText(__companyJobPairsTopStatisticsTextFilePath, sb.ToString());
        }
    }
}
