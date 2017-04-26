using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public class TrainingAndTestingService : ITrainingAndTestingService
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

        public void CreateTrainingAndTestingSetNewRequirements(List<Person> people)
        {
            _personCustomXmlService.WriteToFile(people, @"C:\Users\Niall\5th Year\Thesis\XML\training_set_new.xml");
            people.Select(t => { t.Experiences = null; return t; }).ToList();
            _personCustomXmlService.WriteToFile(people, @"C:\Users\Niall\5th Year\Thesis\XML\testing_set_new.xml");


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

        public void CreateTrainingAndTestingSetBasedOnSingleJob(List<JobStat> jobStats)
        {
            //Get Most popular job and all employees who have that job
            var jobs = jobStats.OrderByDescending(t => t.Employees.Count);
            var mostPopularJob = jobs.FirstOrDefault();

            //Split these employees into a training and testing set
            var employeesWithMostPopularJobTraining = mostPopularJob.Employees.Take(mostPopularJob.Employees.Count / 2);
            var employeesWithMostPopularJobTesting = mostPopularJob.Employees.Skip(mostPopularJob.Employees.Count / 2);

            //Remove this most popular job from the list of jobs
            jobs.ToList().RemoveAt(0);

            //Randomise the list
            var rnd = new Random();
            var randomJobs = jobs.OrderBy(item => rnd.Next()).ToList();

            //Select an equal number of employees that do not have the most popular job for the training set
            var randomTrainingJobs = randomJobs.Take(mostPopularJob.Employees.Count / 2);
            List<Person> randomTrainingEmployees = new List<Person>();
            foreach (var job in randomTrainingJobs)
            {
                randomTrainingEmployees.Add(job.Employees.FirstOrDefault());
            }

            //Select an equal number of employees that do not have the most popular job for the testing set
            var randomTestingJobs = randomJobs.Skip(mostPopularJob.Employees.Count / 2).Take(mostPopularJob.Employees.Count / 2);
            List<Person> randomTestingEmployees = new List<Person>();
            foreach (var job in randomTestingJobs)
            {
                randomTestingEmployees.Add(job.Employees.FirstOrDefault());
            }

            //Write new training set to XML file
            var trainingSetFinal = employeesWithMostPopularJobTraining.Concat(randomTrainingEmployees).ToList();
            _personCustomXmlService.WriteToFile(trainingSetFinal, @"C:\Users\Niall\5th Year\Thesis\XML\TrainingSetMostPopularJob.xml");

            //Write new testing set to XML file
            var testingSetFinal = employeesWithMostPopularJobTesting.Concat(randomTestingEmployees).ToList();
            testingSetFinal.Select(t => t.Experiences = new List<Experience>());
            _personCustomXmlService.WriteToFile(testingSetFinal, @"C:\Users\Niall\5th Year\Thesis\XML\TestingSetMostPopularJob.xml");

        }

        public List<List<Person>> CreateTrainingAndTestingSetBasedJobInput(string input, List<JobStat> jobStats)
        {
            //Get Most popular job and all employees who have that job
            var jobs = jobStats.OrderByDescending(t => t.Employees.Count);
            var selectedJob = jobs.Where(t=>t.JobName==input).FirstOrDefault();

            //Split these employees into a training and testing set
            var employeesWithMostPopularJobTraining = selectedJob.Employees.Take(selectedJob.Employees.Count / 2);
            var employeesWithMostPopularJobTesting = selectedJob.Employees.Skip(selectedJob.Employees.Count / 2);

            //Remove this most popular job from the list of jobs
            jobs.ToList().RemoveAt(0);

            //Randomise the list
            var rnd = new Random();
            var randomJobs = jobs.OrderBy(item => rnd.Next()).ToList();

            //Select an equal number of employees that do not have the most popular job for the training set
            var randomTrainingJobs = randomJobs.Take(selectedJob.Employees.Count / 2);
            List<Person> randomTrainingEmployees = new List<Person>();
            foreach (var job in randomTrainingJobs)
            {
                randomTrainingEmployees.Add(job.Employees.FirstOrDefault());
            }

            //Select an equal number of employees that do not have the most popular job for the testing set
            var randomTestingJobs = randomJobs.Skip(selectedJob.Employees.Count / 2).Take(selectedJob.Employees.Count / 2);
            List<Person> randomTestingEmployees = new List<Person>();
            foreach (var job in randomTestingJobs)
            {
                randomTestingEmployees.Add(job.Employees.FirstOrDefault());
            }

            //Write new training set to XML file
            var trainingSetFinal = employeesWithMostPopularJobTraining.Concat(randomTrainingEmployees).ToList();
            //_personCustomXmlService.WriteToFile(trainingSetFinal, @"C:\Users\Niall\5th Year\Thesis\XML\TrainingSetMostPopularJob.xml");

            //Write new testing set to XML file
            var testingSetFinal = employeesWithMostPopularJobTesting.Concat(randomTestingEmployees).ToList();
            testingSetFinal.Select(t => t.Experiences = new List<Experience>());
            //_personCustomXmlService.WriteToFile(testingSetFinal, @"C:\Users\Niall\5th Year\Thesis\XML\TestingSetMostPopularJob.xml");

            return new List<List<Person>>()
            {
                trainingSetFinal,
                testingSetFinal
            };
        }

    }
}
