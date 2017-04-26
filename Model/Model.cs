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
        private List<Person> _trainingPeople;
        private List<Person> _testingPeople;
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
        private int _randomForestSize = 5;
        private int _skillSetSize = 50;
        private string _jobName = "Software Developer";



        public Model(IHtmlParser htmlParser, ITrainingAndTestingService trainingAndTestingService,
            ICompanyService companyService, IJobService jobService,
            ICompanyJobPairService companyJobPairService, ISkillService skillService,
            IKmeansService kMeansService, ISupportVectorMachineService supportVectorMachineService, IRandomForestService randomForestService)
        {
            //Initialise properties
            _personCustomXmlService = new CustomXmlService<Person>();
            _parser = htmlParser;
            _trainingAndTestingService = trainingAndTestingService;
            _companyJobPairService = companyJobPairService;
            _jobService = jobService;
            _companyService = companyService;
            _kMeansService = kMeansService;
            _supportVectorMachineService = supportVectorMachineService;
            _randomForestService = randomForestService;
            _skillService = skillService;


            //Parse data from files
            _trainingPeople = ParseTrainingPeopleTopJobFromXml();
            //_personCustomXmlService.WriteToFile(_people, @"C:\Users\Niall\5th Year\Thesis\XML\AllPeopleUpdated.xml");
            //var x = _skillService.GenerateSkillStats(_people);            
            //var skills = _skillService.ParseSkillStatsWithCountAtLeastTenFromXml();
            //_skillService.WriteSkillStatsWithCountOfAtLeastTenToXmlFile(skills);
            //_kMeansService.Perform(2, _people, skills);
            _testingPeople = ParseTestingPeopleTopJobFromXml();
            _companyJobPairs = _companyJobPairService.ParseTopCompanyJobPairsFromXml();
            _topJobStats = _jobService.ParseTopJobStatsFromXml();
            _topCompanyStats = _companyService.ParseTopCompanyStatsFromXml();
            _topSkillStats = _skillService.ParseTopSkillStatsFromXml();

            UpdatePerformanceStats(_randomForestSize,_skillSetSize, _jobName);

            //_trainingAndTestingService.CreateTrainingAndTestingSetBasedOnSingleJob(_jobService.ParseJobStatsFromXml());


        }

        public void UpdatePerformanceStats(int randomForestSize, int skillSetSize, string jobName)
        {
            _randomForestSize = randomForestSize;
            _skillSetSize = skillSetSize;

            if (jobName != _jobName)
            {
                var trainingAndTestingSets = _trainingAndTestingService.CreateTrainingAndTestingSetBasedJobInput(jobName, _jobService.ParseJobStatsFromXml());
                _trainingPeople = trainingAndTestingSets[0];
                _testingPeople = trainingAndTestingSets[1];
                _jobName = jobName;
            }

            //Train Machine Learning Models with training data
            _randomForestService.Train(_trainingPeople, randomForestSize, skillSetSize);
            _supportVectorMachineService.Train(_trainingPeople, skillSetSize);
            _kMeansService.Train(_trainingPeople, skillSetSize);

            //Test Machine Learning Models with testing data
            _randomForestService.Test(_testingPeople, skillSetSize);
            _supportVectorMachineService.Test(_testingPeople, skillSetSize);
            _kMeansService.Test(_testingPeople, skillSetSize);
        }

        public List<Person> GetPeople()
        {
            return _trainingPeople;
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
                _randomForestService.ComputeMachineLearningTrainingStat(),
                _randomForestService.ComputeMachineLearningTestingStat(),
                _supportVectorMachineService.ComputeMachineLearningTrainingStat(),
                _supportVectorMachineService.ComputeMachineLearningTestingStat(),
                _kMeansService.ComputeMachineLearningTrainingStat(),
                _kMeansService.ComputeMachineLearningTestingStat()
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

        public MachineLearningStat GetLuceneStats()
        {
            var jobRole = _trainingPeople.FirstOrDefault().Experiences.FirstOrDefault().Role;
            var luceneSearchResults = _luceneService.SearchIndex(jobRole);

            double primaryJobCorrectCount = 0;
            foreach (var person in luceneSearchResults)
            {
                if (person.Experiences.FirstOrDefault().Role == jobRole)
                    primaryJobCorrectCount++;
            }

            return new MachineLearningStat()
            {
                Name = "Lucene Search Base Result",
                PrimaryJobAccurracy = (double)primaryJobCorrectCount / (double)(_trainingPeople.Count / 2),
                OtherJobAccurracy = 0.0
            };
        }



        public List<Person> LuceneSearch(string textSearch)
        {
            return _luceneService.SearchIndex(textSearch);
        }
        
        public int GetRandomForestSize()
        {
            return _randomForestSize;
        }

        public int GetSkillSetSize()
        {
            return _skillSetSize;
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