using LinkedInSearchUi.DataTypes;
using LinkedInSearchUi.Indexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.MachineLearning
{
    public class DataPointService : IDataPointService
    {
        private readonly ISkillService _skillService;
        private List<SkillStat> _allSkills;
        public DataPointService(ISkillService skillService)
        {
            _skillService = skillService;
        }

        public int[] GenerateExpectedResultFromPeople(List<Person> people)
        {
            int count = people.Count;
            int[] result = new int[count];
            for(int i=0;i< count; i++)
            {
                if (i < count / 2)
                    result[i] = 0;
                else
                    result[i] = 1;
            }
            return result;
        }

        public double[][] GenerateDataPointsFromPeople(List<Person> people, int skillSetSize)
        {
            _allSkills = _skillService.GenerateSkillStats(people).OrderBy(t=>t.Count).Take(skillSetSize).ToList();
            var dataPoints = ConvertAllPeopleToDataPoints(people);
            return ConvertRawDataPointsToMachineLearningInputFormat(dataPoints);
        }

        private double[][] ConvertRawDataPointsToMachineLearningInputFormat(List<DataPoint> dataPoints)
        {
            double[][] result = new double[dataPoints.Count][];
            for (int i = 0; i < dataPoints.Count; i++)
            {
                result[i] = new double[dataPoints[i].Attributes.Count];
                for (int j = 0; j < dataPoints[i].Attributes.Count; j++)
                {
                    result[i][j] = dataPoints[i].Attributes[j].Value;
                }
            }
            return result;
        }

        private List<DataPoint> ConvertAllPeopleToDataPoints(List<Person> people)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (var person in people)
            {
                dataPoints.Add(ConvertPersonToDataPoint(person));
            }
            return dataPoints;
        }

        private DataPoint ConvertPersonToDataPoint(Person person)
        {
            DataPoint dataPoint = new DataPoint();
            dataPoint.Attributes.Add(new Attribute() { Value = person.NumberOfConnections });
            dataPoint.Attributes.Add(new Attribute() { Value = person.WorkExperienceInMonths });
            dataPoint.Attributes.Add(new Attribute() { Value = person.Experiences.Count });
            foreach (var skill in _allSkills)
            {
                int index = person.Skills.FindIndex(t => string.Equals(t.Name, skill.Name, StringComparison.OrdinalIgnoreCase));
                if (index != -1)
                    dataPoint.Attributes.Add(new Attribute() { Value = 1 });
                else
                    dataPoint.Attributes.Add(new Attribute() { Value = 0 });
            }

            return dataPoint;
        }
    }
}
