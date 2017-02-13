using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using HtmlAgilityPack;
using LinkedInSearchUi.DataTypes;
using LinkedInSearchUi.Indexing;
using System;
using System.Linq;

namespace LinkedInSearchUi.Model
{
    public class Model
    {
        private HtmlParser _parser;
        private LuceneService _luceneService;
        private CustomXmlService _customXmlService;
        private TrainingAndTestingService _trainingAndTestingService;

        public Model()
        {
            _parser = new HtmlParser();
            _customXmlService = new CustomXmlService();
            _trainingAndTestingService = new TrainingAndTestingService();
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
            _customXmlService.WriteToFile(people, @"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\XML\data.xml");
            _luceneService = new LuceneService(people);
            return people;
        }

        public List<Person> ParsePeopleFromXml()
        {
            var people = _customXmlService.ReadFromFile(@"U:\5th Year\Thesis\LinkedIn\XML\training_set.xml");
            _luceneService = new LuceneService(people);
            return people;
        }

        public void CreateTrainingAndTestSetsBasedOnCompany()
        {
            List<Person> trainingSet = new List<Person>();
            List<Person> testingSet = new List<Person>();
            var companies = _trainingAndTestingService.GenerateCompaniesWithCurrentEmployees(_customXmlService.ReadFromFile(@"C:\Users\nihughes\Downloads\new_data.xml"));
            foreach(var company in companies)
            {
                if(company.Employees.Count >1)
                {
                    for(int i=0;i<company.Employees.Count;i++)
                    {
                        if(i%2==0)
                        {
                            testingSet.Add(company.Employees[i]);
                        }
                        else
                        {
                            trainingSet.Add(company.Employees[i]);
                        }
                    }
                }
                else { trainingSet.Add(company.Employees[0]); }
            }
            _customXmlService.WriteToFile(trainingSet, @"U:\5th Year\Thesis\LinkedIn\XML\training_set.xml");
            _customXmlService.WriteToFile(testingSet, @"U:\5th Year\Thesis\LinkedIn\XML\testing_set.xml");

        }

        public void CreateTrainingAndTestSetsBasedOnJob()
        {
            List<Person> trainingSet = new List<Person>();
            List<Person> testingSet = new List<Person>();
            var jobs = _trainingAndTestingService.GenerateJobsWithCurrentEmployees(_customXmlService.ReadFromFile(@"C:\Users\nihughes\Downloads\new_data.xml"));
            foreach (var job in jobs)
            {
                if (job.Employees.Count > 1)
                {
                    for (int i = 0; i < job.Employees.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            testingSet.Add(job.Employees[i]);
                        }
                        else
                        {
                            trainingSet.Add(job.Employees[i]);
                        }
                    }
                }
                else { trainingSet.Add(job.Employees[0]); }
            }
            _customXmlService.WriteToFile(trainingSet, @"U:\5th Year\Thesis\LinkedIn\XML\training_set_jobs.xml");
            _customXmlService.WriteToFile(testingSet, @"U:\5th Year\Thesis\LinkedIn\XML\testing_set_jobs.xml");

        }

        public void GenerateCompanies(List<Person> people)
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
        }

        public List<Person> LuceneSearch(string textSearch)
        {
            return _luceneService.SearchIndex(textSearch);
        }
    }
}