using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public class JobService : IJobService
    {
        private CustomXmlService<JobStat> _jobStatsCustomXmlService;
        private string _allJobStatsXmlFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\AllJobStats.xml";
        private string _jobStatsTopStatisticsXmlFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\Top100JobStats.xml";
        private string _jobStatsTopStatisticsTextFilePath = @"C:\Users\Niall\5th Year\Thesis\XML\Top100JobStats.txt";


        public JobService()
        {
            _jobStatsCustomXmlService = new CustomXmlService<JobStat>();
        }

        public List<JobStat> GenerateJobStats(IEnumerable<Person> people)
        {
            List<JobStat> jobStats = new List<JobStat>();
            int count = 0;
            foreach (var person in people)
            {
                var experience = person.Experiences.FirstOrDefault();
                if (experience != null)
                {
                    int index = jobStats.FindIndex(t => string.Equals(t.JobName, experience.Role, StringComparison.OrdinalIgnoreCase));

                    if (index >= 0)
                    {
                        jobStats[index].Count++;
                    }
                    else
                    {
                        jobStats.Add(new JobStat()
                        {
                            JobName = person.Experiences.FirstOrDefault().Role,
                            Count = 1
                        });
                    }
                }
                count++;
                if (count % 10000 == 0)
                    Console.WriteLine(count);
            }
            return jobStats;
        }

        public List<JobStat> ParseJobStatsFromXml()
        {
            return _jobStatsCustomXmlService.ReadFromFile(_allJobStatsXmlFilePath);
        }

        public List<JobStat> ParseTopJobStatsFromXml()
        {
            return _jobStatsCustomXmlService.ReadFromFile(_jobStatsTopStatisticsXmlFilePath);
        }

        public void WriteJobStatsToXmlFile(List<JobStat> jobStats)
        {
            var mostFrequentJobs = jobStats.OrderByDescending(t => t.Count).ToList();
            _jobStatsCustomXmlService.WriteToFile(mostFrequentJobs, _allJobStatsXmlFilePath);
        }

        public void WriteJobStatsTopStatisticsToXmlFile(List<JobStat> jobStats)
        {
            var mostFrequentJobs = jobStats.OrderByDescending(t => t.Count).Take(100).ToList();
            _jobStatsCustomXmlService.WriteToFile(mostFrequentJobs, _jobStatsTopStatisticsXmlFilePath);
        }
    }
}
