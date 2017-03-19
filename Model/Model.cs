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
    public class Model : IModel
    {
        private IHtmlParser _parser;
        private ILuceneService _luceneService;
        private ITrainingAndTestingService _trainingAndTestingService;
        private ICompanyJobPairService _companyJobPairService;
        private CustomXmlService<Person> _personCustomXmlService;
        private readonly List<Person> _people;
        private readonly List<CompanyJobPair> _companyJobPairs;

        public Model(IHtmlParser htmlParser, ITrainingAndTestingService trainingAndTestingService, ICompanyJobPairService companyJobPairService)
        {
            _personCustomXmlService = new CustomXmlService<Person>();
            _parser = htmlParser;
            _trainingAndTestingService = trainingAndTestingService;
            _companyJobPairService = companyJobPairService;
            _people = ParsePeopleFromXml();
            _companyJobPairs = _companyJobPairService.ParseTopCompanyJobPairsFromXml();
        }

        public List<Person> GetPeople()
        {
            return _people;
        }

        public List<CompanyJobPair> GetCompanyJobPairs()
        {
            return _companyJobPairs;
        }

        private List<Person> ParseRawHtmlFilesFromDirectory()
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

        private List<Person> ParsePeopleFromXml()
        {
            var people = _personCustomXmlService.ReadFromFile(@"C:\Users\Niall\5th Year\Thesis\XML\all_people.xml");
            _luceneService = new LuceneService(people);
            return people;
        }

        private List<Person> ParseTestingPeopleFromXml()
        {
            var people = _personCustomXmlService.ReadFromFile(@"C:\Users\Niall\5th Year\Thesis\XML\testing_set_new.xml");
            _luceneService = new LuceneService(people);
            return people;
        }

        private List<Person> ParseTrainingPeopleFromXml()
        {
            var people = _personCustomXmlService.ReadFromFile(@"C:\Users\Niall\5th Year\Thesis\XML\training_set_new.xml");
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

        public void CreateTrainingAndTestSetsNewRequirements(List<Person> people)
        {
            _trainingAndTestingService.CreateTrainingAndTestingSetNewRequirements(people);
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

        public List<CompanyJobPair> ParseTopCompanyJobPairsFromXml()
        {
            return _companyJobPairService.ParseTopCompanyJobPairsFromXml();
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