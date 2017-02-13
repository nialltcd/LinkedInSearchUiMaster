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
        public List<Company> GenerateCompaniesWithCurrentEmployees(List<Person> people)
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

        public List<Job> GenerateJobsWithCurrentEmployees(List<Person> people)
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
