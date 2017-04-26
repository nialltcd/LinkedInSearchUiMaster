using Accord.MachineLearning;
using LinkedInSearchUi.DataTypes;
using LinkedInSearchUi.Indexing;
using LinkedInSearchUi.MachineLearning;
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
        private IDataPointService _dataPointService;
        KMeansClusterCollection _clustersCollection;
        private int[] testPredictions;
        private int[] trainingPredictions;

        public KMeansService(IDataPointService dataPointService)
        {
            _dataPointService = dataPointService;
        }
        
        public void Train(List<Person> trainingPeople, int skillSetSize)
        {
            double[][] inputs = _dataPointService.GenerateDataPointsFromPeople(trainingPeople, skillSetSize);
            KMeans kMeans = new KMeans(2);

            _clustersCollection = kMeans.Learn(inputs);

            trainingPredictions = _clustersCollection.Decide(inputs);
        }

        public void Test(List<Person> testingPeople, int skillSetSize)
        {
            var inputs = _dataPointService.GenerateDataPointsFromPeople(testingPeople, skillSetSize);

            testPredictions = _clustersCollection.Decide(inputs);
        }

        public MachineLearningStat ComputeMachineLearningTrainingStat()
        {
            double primaryJobCorrectCount = 0, otherJobCorrectCount = 0;

            for (int i = 0; i < trainingPredictions.Length; i++)
            {
                if (i < trainingPredictions.Length / 2)
                    if (trainingPredictions[i] == 0)
                        primaryJobCorrectCount++;
                    else
                    if (trainingPredictions[i] == 1)
                        otherJobCorrectCount++;
            }

            return new MachineLearningStat()
            {
                Name = "K-Means Training",
                PrimaryJobAccurracy = (double)primaryJobCorrectCount / (double)(trainingPredictions.Length / 2),
                OtherJobAccurracy = (double)otherJobCorrectCount / (double)(trainingPredictions.Length / 2)
            };
        }

        public MachineLearningStat ComputeMachineLearningTestingStat()
        {
            double primaryJobCorrectCount = 0, otherJobCorrectCount = 0;

            for (int i = 0; i < testPredictions.Length; i++)
            {
                if (i < testPredictions.Length / 2)
                    if (testPredictions[i] == 0)
                        primaryJobCorrectCount++;
                    else
                    if (testPredictions[i] == 1)
                        otherJobCorrectCount++;
            }

            return new MachineLearningStat()
            {
                Name = "K-Means Testing",
                PrimaryJobAccurracy = (double)primaryJobCorrectCount / (double)(testPredictions.Length / 2),
                OtherJobAccurracy = (double)otherJobCorrectCount / (double)(testPredictions.Length / 2)
            };
        }
    }
}
