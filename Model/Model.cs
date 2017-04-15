using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using HtmlAgilityPack;
using LinkedInSearchUi.DataTypes;
using LinkedInSearchUi.Indexing;
using System;
using System.Linq;
using System.Text;
using LinkedInSearchUi.KMeansInfrastructure;
using LinkedInSearchUi.MachineLearning;

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
        private readonly List<JobStat> _topJobStats;
        private readonly List<CompanyStat> _topCompanyStats;
        private readonly List<SkillStat> _topSkillStats;
        private readonly IJobService _jobService;
        private readonly ICompanyService _companyService;
        private readonly ISkillService _skillService;
        private readonly IKmeansService _kMeansService;
        private readonly ISupportVectorMachineService _supportVectorMachineService;
        private readonly IRandomForestService _randomForestService;



        public Model(IHtmlParser htmlParser, ITrainingAndTestingService trainingAndTestingService,
            ICompanyService companyService, IJobService jobService,
            ICompanyJobPairService companyJobPairService, ISkillService skillService,
            IKmeansService kMeansService, ISupportVectorMachineService supportVectorMachineService, IRandomForestService randomForestService)
        {
            _personCustomXmlService = new CustomXmlService<Person>();
            _parser = htmlParser;
            _trainingAndTestingService = trainingAndTestingService;
            _companyJobPairService = companyJobPairService;
            _jobService = jobService;
            _companyService = companyService;
            _kMeansService = kMeansService;
            _supportVectorMachineService = supportVectorMachineService;
            _randomForestService = randomForestService;
            _people = ParseTrainingPeopleTopJobFromXml();
            //_personCustomXmlService.WriteToFile(_people, @"C:\Users\Niall\5th Year\Thesis\XML\AllPeopleUpdated.xml");
            _skillService = skillService;
            //var x = _skillService.GenerateSkillStats(_people);
            var skills = _skillService.ParseSkillStatsWithCountAtLeastTenFromXml();
            //_skillService.WriteSkillStatsWithCountOfAtLeastTenToXmlFile(skills);
            //_kMeansService.Perform(2, _people, skills);
            var testingPeople = ParseTestingPeopleTopJobFromXml();
            randomForestService.Train(_people);
            randomForestService.Test(testingPeople);
            _supportVectorMachineService.Train(_people);
            _supportVectorMachineService.Test(_people);

            _companyJobPairs = _companyJobPairService.ParseTopCompanyJobPairsFromXml();
            
            _topJobStats = _jobService.ParseTopJobStatsFromXml();
            //_trainingAndTestingService.CreateTrainingAndTestingSetBasedOnSingleJob(_jobService.ParseJobStatsFromXml());

            _topCompanyStats = _companyService.ParseTopCompanyStatsFromXml();
            _topSkillStats = _skillService.ParseTopSkillStatsFromXml();

        }

        public List<Person> GetPeople()
        {
            return _people;
        }

        public List<CompanyJobPair> GetCompanyJobPairs()
        {
            return _companyJobPairs;
        }

        public List<JobStat> GetTopJobStats()
        {
            return _topJobStats;
        }

        public List<CompanyStat> GetTopCompanyStats()
        {
            return _topCompanyStats;
        }

        public List<SkillStat> GetTopSkillStats()
        {
            return _topSkillStats;
        }

        public List<MachineLearningStat> GetMachineLearningStats()
        {
            return new List<MachineLearningStat>()
            {
                _randomForestService.ComputeMachineLearningStat(),
                _supportVectorMachineService.ComputeMachineLearningStat()
            };
        }

        private List<Person> ParseRawHtmlFilesFromDirectory()
        {
            List<Person> people = new List<Person>();
            var document = new HtmlDocument();
            int idCount = 0;
            foreach (var file in Directory.EnumerateFiles(@"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\SmallLinkedInDataSet", "*.html"))
            {
                try
                {
                    document.Load(file);
                    var person = _parser.GeneratePersonFromHtmlDocument(document);
                    person.Id = idCount++;
                    people.Add(person);
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
            var people = _personCustomXmlService.ReadFromFile(@"C:\Users\Niall\5th Year\Thesis\XML\AllPeople.xml");
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

        private List<Person> ParseTestingPeopleTopJobFromXml()
        {
            var people = _personCustomXmlService.ReadFromFile(@"C:\Users\Niall\5th Year\Thesis\XML\TestingSetMostPopularJob.xml");
            _luceneService = new LuceneService(people);
            return people;
        }

        private List<Person> ParseTrainingPeopleTopJobFromXml()
        {
            var people = _personCustomXmlService.ReadFromFile(@"C:\Users\Niall\5th Year\Thesis\XML\TrainingSetMostPopularJob.xml");
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

        #region Job Statistics
        public List<JobStat> ParseJobStatsFromXml()
        {
            return _jobService.ParseJobStatsFromXml();
        }

        public List<JobStat> ParseTopJobStatsFromXml()
        {
            return _jobService.ParseTopJobStatsFromXml();
        }
        #endregion

        #region Company Statistics
        public List<CompanyStat> ParseCompanyStatsFromXml()
        {
            return _companyService.ParseCompanyStatsFromXml();
        }

        public List<CompanyStat> ParseTopCompanyStatsFromXml()
        {
            return _companyService.ParseTopCompanyStatsFromXml();
        }
        #endregion
    }
}