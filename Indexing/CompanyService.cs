using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public class CompanyService
    {
        private List<Company> _companies;
        public CompanyService()
        {
            _companies = new List<Company>();
        }

        public List<Company> GenerateCompaniesWithCurrentEmployees(List<Person> people)
        {
            foreach (var person in people)
            {
                var company = person.Experiences.FirstOrDefault();
                if (company != null)
                {
                    int index = _companies.FindIndex(t => t.Name == company.Organisation);
                    if (index == -1)
                        _companies.Add(new Company() { Name = company.Organisation, Employees = new List<Person>() {person} });
                    else
                        _companies[index].Employees.Add(person);
                }
            }
            return _companies;
        }        
    }
}
