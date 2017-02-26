using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public class TrainingAndTestingService
    {
        private CustomXmlService<Person> _personCustomXmlService;

        public TrainingAndTestingService()
        {
            _personCustomXmlService = new CustomXmlService<Person>();
        }

        public void CreateTrainingAndTestSetsBasedOnJob()
        {
            List<Person> trainingSet = new List<Person>();
            List<Person> testingSet = new List<Person>();
            var jobs = GenerateJobsWithCurrentEmployees(_personCustomXmlService.ReadFromFile(@"C:\Users\nihughes\Downloads\new_data.xml"));
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
            _personCustomXmlService.WriteToFile(trainingSet, @"U:\5th Year\Thesis\LinkedIn\XML\training_set_jobs.xml");
            _personCustomXmlService.WriteToFile(testingSet, @"U:\5th Year\Thesis\LinkedIn\XML\testing_set_jobs.xml");

        }

        public void CreateTrainingAndTestSetsBasedOnCompany()
        {
            List<Person> trainingSet = new List<Person>();
            List<Person> testingSet = new List<Person>();
            var companies = GenerateCompaniesWithCurrentEmployees(_personCustomXmlService.ReadFromFile(@"C:\Users\nihughes\Downloads\new_data.xml"));
            foreach (var company in companies)
            {
                if (company.Employees.Count > 1)
                {
                    for (int i = 0; i < company.Employees.Count; i++)
                    {
                        if (i % 2 == 0)
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
            _personCustomXmlService.WriteToFile(trainingSet, @"U:\5th Year\Thesis\LinkedIn\XML\training_set.xml");
            _personCustomXmlService.WriteToFile(testingSet, @"U:\5th Year\Thesis\LinkedIn\XML\testing_set.xml");

        }


        private List<Company> GenerateCompaniesWithCurrentEmployees(List<Person> people)
        {
            List<Company> companies = new List<Company>();
            foreach (var person in people)
            {
                var experience = person.Experiences.FirstOrDefault();
                if (experience != null)
                {
                    int index = companies.FindIndex(t => t.Name == experience.Organisation);
                    if (index == -1)
                        companies.Add(new Company() { Name = experience.Organisation, Employees = new List<Person>() {person} });
                    else
                        companies[index].Employees.Add(person);
                }
            }
            return companies;
        }

        private List<Job> GenerateJobsWithCurrentEmployees(List<Person> people)
        {
            List<Job> jobs = new List<Job>();
            foreach (var person in people)
            {
                var experience = person.Experiences.FirstOrDefault();
                if (experience != null)
                {
                    int index = jobs.FindIndex(t => t.Name == experience.Role);
                    if (index == -1)
                        jobs.Add(new Job() { Name = experience.Role, Employees = new List<Person>() { person } });
                    else
                        jobs[index].Employees.Add(person);
                }
            }
            return jobs;
        }
    }
}
