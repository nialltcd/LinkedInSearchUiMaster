using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.MachineLearning
{
    public class SupportVectorMachineService : ISupportVectorMachineService
    {
        private IDataPointService _dataPointService;
        private SupportVectorMachine _supportVectorMachine;
        private bool[] trainingPredictions;
        private bool[] testPredictions;


        public SupportVectorMachineService(IDataPointService dataPointService)
        {
            _dataPointService = dataPointService;
        }


        public void Train(List<Person> trainingPeople)
        {
            double[][] inputs = _dataPointService.GenerateDataPointsFromPeople(trainingPeople);

            int[] expectedResults = _dataPointService.GenerateExpectedResultFromPeople(trainingPeople);

            // Now, we can create the sequential minimal optimization teacher
            var learn = new SequentialMinimalOptimization()
            {
                UseComplexityHeuristic = true,
                UseKernelEstimation = false
            };

            // And then we can obtain a trained SVM by calling its Learn method
            _supportVectorMachine = learn.Learn(inputs, expectedResults);

            // Finally, we can obtain the decisions predicted by the machine:
            trainingPredictions = _supportVectorMachine.Decide(inputs);

            File.WriteAllLines(
                @"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\XML\predictions.txt" // <<== Put the file name here
            , trainingPredictions.Select(d => d.ToString()).ToArray());


        }

        public void Test(List<Person> testingPeople)
        {
            double[][] inputs = _dataPointService.GenerateDataPointsFromPeople(testingPeople);
            testPredictions = _supportVectorMachine.Decide(inputs);
            File.WriteAllLines(
               @"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\XML\support_vector_machine_test_predictions.txt" // <<== Put the file name here
           , testPredictions.Select(d => d.ToString()).ToArray());
        }

        public MachineLearningStat ComputeMachineLearningTrainingStat()
        {
            double primaryJobCorrectCount = 0, otherJobCorrectCount = 0;

            for (int i = 0; i < trainingPredictions.Length; i++)
            {
                if (i < trainingPredictions.Length / 2)
                    if (trainingPredictions[i] == false)
                        primaryJobCorrectCount++;
                else
                    if (trainingPredictions[i] == true)
                        otherJobCorrectCount++;
            }

            return new MachineLearningStat()
            {
                Name = "Support Vector Machine Training",
                PrimaryJobAccurracy = (double)primaryJobCorrectCount / (double)(testPredictions.Length / 2),
                OtherJobAccurracy = (double)otherJobCorrectCount / (double)(testPredictions.Length / 2)
            };
        }

        public MachineLearningStat ComputeMachineLearningTestingStat()
        {
            double primaryJobCorrectCount = 0, otherJobCorrectCount = 0;

            for (int i = 0; i < testPredictions.Length; i++)
            {
                if (i < testPredictions.Length / 2)
                    if (testPredictions[i] == false)
                        primaryJobCorrectCount++;
                    else
                    if (testPredictions[i] == true)
                        otherJobCorrectCount++;
            }

            return new MachineLearningStat()
            {
                Name = "Support Vector Machine Testing",
                PrimaryJobAccurracy = (double)primaryJobCorrectCount / (double)(testPredictions.Length / 2),
                OtherJobAccurracy = (double)otherJobCorrectCount / (double)(testPredictions.Length / 2)
            };
        }
    }
}
