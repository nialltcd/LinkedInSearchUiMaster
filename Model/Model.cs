using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using HtmlAgilityPack;
using LinkedInSearchUi.DataTypes;
using LinkedInSearchUi.Indexing;
using System;
using System.Linq;
using System.Text;

namespace LinkedInSearchUi.Model
{
    public class Model
    {
        private HtmlParser _parser;
        private LuceneService _luceneService;
        private TrainingAndTestingService _trainingAndTestingService;
        private CompanyJobPairService _companyJobPairService;
        private CustomXmlService<Person> _personCustomXmlService;

        public Model()
        {
            _personCustomXmlService = new CustomXmlService<Person>();
            _parser = new HtmlParser();
            _trainingAndTestingService = new TrainingAndTestingService();
            _companyJobPairService = new CompanyJobPairService();
        }

        public List<Person> ParseRawHtmlFilesFromDirectory()
        {
            List<Person> people = new List<Person>();
            var document = new HtmlDocument();
            foreach (var file in Directory.EnumerateFiles(@"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\SmallLinkedInDataSet", "*.html"))
            {
                try
                {
                    document.Load(file);
                    people.Add(_parser.GeneratePersonFromHtmlDocument(document));
                    if (people.Count % 1000 == 0)
                        Console.WriteLine(people.Count);
                }
                catch (Exception e) { }
            }
            _personCustomXmlService.WriteToFile(people, @"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\XML\data.xml");
            _luceneService = new LuceneService(people);
            return people;
        }

        public List<Person> ParsePeopleFromXml()
        {
            var people = _personCustomXmlService.ReadFromFile(@"C:\Users\Niall\5th Year\Thesis\XML\all_people.xml");
            _luceneService = new LuceneService(people);
            return people;
        }



        public List<Company> GenerateCompanies(List<Person> people)
        {
            List<Company> companies = new List<Company>();
            foreach(var person in people)
            {
                var company = person.Experiences.FirstOrDefault();
                if (company != null)
                {
                    int index = companies.FindIndex(t => t.Name == company.Organisation);
                    if (index >= 0)
                    {
                        companies[index].Employees.Add(person);
                    }
                    else
                    {
                        companies.Add(new Company() { Name = company.Organisation, Employees = new List<Person>() } );
                    }                        
                }
            }
            return companies;
        }    
        

        public List<Person> LuceneSearch(string textSearch)
        {
            return _luceneService.SearchIndex(textSearch);
        }

        #region Training and Testing Sets
        public void CreateTrainingAndTestSetsBasedOnCompany()
        {
            _trainingAndTestingService.CreateTrainingAndTestSetsBasedOnCompany();
        }

        public void CreateTrainingAndTestSetsBasedOnJob()
        {
            _trainingAndTestingService.CreateTrainingAndTestSetsBasedOnJob();
        }
        #endregion

        #region CompanyJobPairService

        public List<CompanyJobPair> GenerateCompanyJobPairs(IEnumerable<Person> people)
        {
            return _companyJobPairService.GenerateCompanyJobPairs(people);
        }

        public List<CompanyJobPair> ParseCompanyJobPairsFromXml()
        {
            return _companyJobPairService.ParseCompanyJobPairsFromXml();
        }

        public void WriteCompanyJobPairsToXmlFile(List<CompanyJobPair> companyJobPairs)
        {
            _companyJobPairService.WriteCompanyJobPairsToXmlFile(companyJobPairs);
        }

        public void WriteCompanyJobPairsTopStatisticsToXmlFile(List<CompanyJobPair> companyJobPairs)
        {
            _companyJobPairService.WriteCompanyJobPairsTopStatisticsToXmlFile(companyJobPairs);
        }

        public void WriteCompanyJobPairsTopStatisticsToFileFormatted(List<CompanyJobPair> companyJobPairs)
        {
            _companyJobPairService.WriteCompanyJobPairsTopStatisticsToFileFormatted(companyJobPairs);
        }
        #endregion

    }
}