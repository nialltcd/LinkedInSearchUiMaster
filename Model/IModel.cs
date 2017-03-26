using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Model
{
    public interface IModel
    {
        List<Person> GetPeople();
        List<CompanyJobPair> GetCompanyJobPairs();
        List<JobStat> GetTopJobStats();
        List<CompanyStat> GetTopCompanyStats();
        List<Company> GenerateCompanies(List<Person> people);
        List<Person> LuceneSearch(string textSearch);
        void CreateTrainingAndTestSetsBasedOnCompany();
        void CreateTrainingAndTestSetsBasedOnJob();
        void CreateTrainingAndTestSetsNewRequirements(List<Person> people);
        List<CompanyJobPair> GenerateCompanyJobPairs(IEnumerable<Person> people);
        List<CompanyJobPair> ParseCompanyJobPairsFromXml();
        List<CompanyJobPair> ParseTopCompanyJobPairsFromXml();
        void WriteCompanyJobPairsToXmlFile(List<CompanyJobPair> companyJobPairs);
        void WriteCompanyJobPairsTopStatisticsToXmlFile(List<CompanyJobPair> companyJobPairs);
        void WriteCompanyJobPairsTopStatisticsToFileFormatted(List<CompanyJobPair> companyJobPairs);
        List<JobStat> ParseJobStatsFromXml();
        List<JobStat> ParseTopJobStatsFromXml();
        List<CompanyStat> ParseCompanyStatsFromXml();
        List<CompanyStat> ParseTopCompanyStatsFromXml();
    }
}
