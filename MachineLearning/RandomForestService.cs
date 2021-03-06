﻿using Accord.MachineLearning.DecisionTrees;
using Accord.Math.Optimization.Losses;
using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.MachineLearning
{
    public class RandomForestService : IRandomForestService
    {
        private readonly IDataPointService _dataPointService;
        private RandomForest _randomForest;
        private int[] trainingPredictions;
        private int[] testPredictions;
        public RandomForestService(IDataPointService dataPointService)
        {
            _dataPointService = dataPointService;
        }
        public void Train(List<Person> people, int numberOfTrees, int skillSetSize)
        {
            double[][] inputs = _dataPointService.GenerateDataPointsFromPeople(people, skillSetSize);

            int[] expectedResults = _dataPointService.GenerateExpectedResultFromPeople(people);

            // Create the forest learning algorithm
            var teacher = new RandomForestLearning()
            {
                NumberOfTrees = numberOfTrees, // use 10 trees in the forest
            };

            // Finally, learn a random forest from data
            _randomForest = teacher.Learn(inputs, expectedResults);

            // We can estimate class labels using
            trainingPredictions = _randomForest.Decide(inputs);

            // And the classification error (0.0006) can be computed as 
            double error = new ZeroOneLoss(expectedResults).Loss(_randomForest.Decide(inputs));

            File.WriteAllLines(
                @"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\XML\random_forest_predictions.txt" // <<== Put the file name here
            , trainingPredictions.Select(d => d.ToString()).ToArray());
        }

        public void Test(List<Person> testingPeople, int skillSetSize)
        {
            double[][] inputs = _dataPointService.GenerateDataPointsFromPeople(testingPeople, skillSetSize);
            testPredictions = _randomForest.Decide(inputs);
            //calculate
            File.WriteAllLines(
               @"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\XML\random_forest_test_predictions.txt" // <<== Put the file name here
           , testPredictions.Select(d => d.ToString()).ToArray());
        }

        public MachineLearningStat ComputeMachineLearningTrainingStat()
        {
            double primaryJobCorrectCount = 0, otherJobCorrectCount = 0;

            for(int i=0;i< trainingPredictions.Length;i++)
            {
                if (i < trainingPredictions.Length / 2)
                {
                    if (trainingPredictions[i] == 0)
                        primaryJobCorrectCount++;
                }
                else
                    if (trainingPredictions[i] == 1)
                        otherJobCorrectCount++;
            }

            return new MachineLearningStat()
            {
                Name = "Random Forest Training",
                PrimaryJobAccurracy = primaryJobCorrectCount/ (trainingPredictions.Length / 2),
                OtherJobAccurracy = otherJobCorrectCount/ (trainingPredictions.Length / 2)
            };
        }

        public MachineLearningStat ComputeMachineLearningTestingStat()
        {
            double primaryJobCorrectCount = 0, otherJobCorrectCount = 0;

            for (int i = 0; i < testPredictions.Length; i++)
            {
                if (i < testPredictions.Length / 2)
                {
                    if (testPredictions[i] == 0)
                        primaryJobCorrectCount++;
                }
                else
                    if (testPredictions[i] == 1)
                    otherJobCorrectCount++;
            }

            return new MachineLearningStat()
            {
                Name = "Random Forest Testing",
                PrimaryJobAccurracy = primaryJobCorrectCount / (testPredictions.Length / 2),
                OtherJobAccurracy = otherJobCorrectCount / (testPredictions.Length / 2)
            };
        }
    }
}
