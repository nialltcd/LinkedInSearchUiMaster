using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public interface ICompanyJobPairService
    {
        List<CompanyJobPair> GenerateCompanyJobPairs(IEnumerable<Person> people);
        List<CompanyJobPair> ParseCompanyJobPairsFromXml();
        List<CompanyJobPair> ParseTopCompanyJobPairsFromXml();
        void WriteCompanyJobPairsToXmlFile(List<CompanyJobPair> companyJobPairs);
        void WriteCompanyJobPairsTopStatisticsToXmlFile(List<CompanyJobPair> companyJobPairs);
        void WriteCompanyJobPairsTopStatisticsToFileFormatted(List<CompanyJobPair> companyJobPairs);
    }
}
