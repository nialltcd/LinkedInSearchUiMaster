using Accord.MachineLearning;
using LinkedInSearchUi.DataTypes;
using LinkedInSearchUi.Indexing;
using LinkedInSearchUi.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LinkedInSearchUi.KMeansInfrastructure
{
    public class KMeansService : IKmeansService
    {
        private List<DataPoint> _rawDataToCluster;
        private List<DataPoint> _normalizedDataToCluster;
        private List<DataPoint> _clusters;
        private List<Person> _allPeople;
        private int _numberOfClusters;
        private readonly IModel _model;
        private KMeans _kMeans;
        private List<SkillStat> _allSkills;

        public KMeansService()
        {
            _kMeans = new KMeans(2);
        }
        
        public void Perform(int numberOfClusters, List<Person> people, List<SkillStat> skills)
        {
            _allSkills = skills;
            _kMeans = new KMeans(numberOfClusters);
            _rawDataToCluster = ConvertAllPeopleToDataPoints(people);
            var kMeansData = ConvertRawDataPointsToKMeansFormat(_rawDataToCluster);
            double[] weights = new double[_rawDataToCluster.Count];

            List<ClusterStat> BestClusterStats = new List<ClusterStat>()
            {
                new ClusterStat() { ClusterId = 1, PrimaryJobCount=0,OtherJobCount=0 },
                new ClusterStat() { ClusterId = 2, PrimaryJobCount=0,OtherJobCount=0 }
            };

            for (int i = 0; i < 10000; i++)
            {
                var newWeights = RandomiseWeights(weights);
                KMeansClusterCollection clusters = _kMeans.Learn(kMeansData, newWeights);
                int[] labels = clusters.Decide(kMeansData);
                var clusterStats = GenerateClusterStats(labels);
                
                if((Math.Abs(clusterStats[0].PrimaryJobCount - clusterStats[0].OtherJobCount))
                    > (Math.Abs(BestClusterStats[0].PrimaryJobCount - BestClusterStats[0].OtherJobCount)
                    ))
                {
                    WriteClusterStatsToFile(clusterStats);
                    BestClusterStats = clusterStats;
                }
                //WriteWeightsToFile(weights);
            }

            //File.WriteAllText(
            //                @"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\XML\elapsed.txt" // <<== Put the file name here
            //            , sw.Elapsed.ToString()
            //            );
            //File.WriteAllLines(
            //    @"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\XML\labels.txt" // <<== Put the file name here
            //, labels.Select(d => d.ToString()).ToArray()
            //);
        }

        private double[] RandomiseWeights(double[] weights)
        {
            Random random = new Random();
            for (int i=0;i<weights.Length;i++)
            {
                weights[i] = (random.NextDouble());
                
            }
            return weights;
        }

        
        private void WriteWeightsToFile(double[] weights)
        {
            File.AppendAllText(@"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\XML\WeightData.txt",
                string.Join(",", weights) + Environment.NewLine);
        }

        private void WriteClusterStatsToFile(List<ClusterStat> clusterStats)
        {
            string s = clusterStats[0].ClusterId + "," 
                        + clusterStats[0].PrimaryJobCount + "," 
                        + clusterStats[0].OtherJobCount + ","
                        + clusterStats[1].ClusterId + ","
                        + clusterStats[1].PrimaryJobCount + ","
                        + clusterStats[1].OtherJobCount;

            File.AppendAllText(@"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\XML\NewSupervisedClusterData.txt", 
                s + Environment.NewLine);
        }

        private void Train()
        {

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

        public DataPoint ConvertPersonToDataPoint(Person person)
        {
            DataPoint dataPoint = new DataPoint();
            dataPoint.Attributes.Add(new Attribute() { Value = person.NumberOfConnections });
            dataPoint.Attributes.Add(new Attribute() { Value = person.WorkExperienceInMonths });
            dataPoint.Attributes.Add(new Attribute() { Value = person.Experiences.Count });
            foreach (var skill in _allSkills.Take(5000))
            {
                int index = person.Skills.FindIndex(t => string.Equals(t.Name, skill.Name, StringComparison.OrdinalIgnoreCase));
                if (index != -1)
                    dataPoint.Attributes.Add(new Attribute() { Value = 1 });
                else
                    dataPoint.Attributes.Add(new Attribute() { Value = 0 });
            }

            return dataPoint;
        }

        private double [][] ConvertRawDataPointsToKMeansFormat(List<DataPoint> dataPoints)
        {
            double[][] result = new double [dataPoints.Count][];
            for(int i=0; i< dataPoints.Count;i++)
            {
                result[i] = new double[dataPoints[i].Attributes.Count];
                for(int j=0;j<dataPoints[i].Attributes.Count;j++)
                {
                    result[i][j] = dataPoints[i].Attributes[j].Value;
                }
            }
            return result;
        }

        public class ClusterStat
        {
            public int ClusterId { get; set; }
            public int PrimaryJobCount { get; set; }
            public int OtherJobCount { get; set; }

        }

        private List<ClusterStat> GenerateClusterStats(int[] labels)
        {
            List<ClusterStat> result = new List<ClusterStat>()
            {
                new ClusterStat() { ClusterId = 1, PrimaryJobCount=0,OtherJobCount=0 },
                new ClusterStat() { ClusterId = 2, PrimaryJobCount=0,OtherJobCount=0 }
            };

            for(int i=0;i<labels.Length;i++)
            {
                if(i<labels.Length/2)
                        result[labels[i]].PrimaryJobCount++;
                else
                    result[labels[i]].OtherJobCount++;
            }
            return result;
        }

    }
}
