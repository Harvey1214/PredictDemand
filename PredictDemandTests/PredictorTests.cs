using Microsoft.VisualStudio.TestTools.UnitTesting;
using PredictDemandLibrary;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PredictDemandTests
{
    [TestClass]
    public class PredictorTests
    {
        public static List<float> defaultInput = new List<float>()
        {
            2,
            4,
            8,
            16,
            32
        };

        [TestMethod]
        public void ValidAveragePrediction()
        {
            // Arrange
            List<float> inputData = defaultInput;
            double expected = 12.4;
            Predictor predictor = new Predictor();

            // Assign
            predictor.data = inputData.ToArray();

            // Act
            double actual = predictor.PredictUsingAverage();

            // Assert
            Assert.AreEqual(expected, actual, 0.0001, "The prediction using average was not calculated correctly");
        }

        [TestMethod]
        public void ValidMedianPrediction()
        {
            // Arrange
            List<float> inputData = defaultInput; 
            double expected = 8;
            Predictor predictor = new Predictor();

            // Assign
            predictor.data = inputData.ToArray();

            // Act
            double actual = predictor.PredictUsingMedian();

            // Assert
            Assert.AreEqual(expected, actual, 0.0001, "The prediction using median was not calculated correctly");
        }

        [TestMethod]
        public void ValidAbsoluteChangesPrediction()
        {
            // Arrange
            List<float> inputData = defaultInput; 
            double expected = 39.5;
            Predictor predictor = new Predictor();

            // Assign
            predictor.data = inputData.ToArray();

            // Act
            double actual = predictor.PredictUsingAbsoluteChanges();

            // Assert
            Assert.AreEqual(expected, actual, 0.0001, "The prediction using absolute changes was not calculated correctly");
        }

        [TestMethod]
        public void ValidTrendsPrediction()
        {
            // Arrange
            List<float> inputData = defaultInput; 
            double expected = 64;
            Predictor predictor = new Predictor();

            // Assign
            predictor.data = inputData.ToArray();

            // Act
            double actual = predictor.PredictUsingTrends();

            // Assert
            Assert.AreEqual(expected, actual, 0.0001, "The prediction using trends was not calculated correctly");
        }

        [TestMethod]
        public void ValidRegressionPrediction()
        {
            // Arrange
            List<float> inputData = defaultInput;
            List<int> inputDates = new List<int>()
            {
                0,
                1,
                2,
                3,
                4
            };
            double expected = 34;
            Predictor predictor = new Predictor();

            // Assign
            predictor.data = inputData.ToArray();
            predictor.dates = inputDates.ToArray();

            // Act
            double actual = predictor.PredictUsingRegression();

            // Assert
            Assert.AreEqual(expected, actual, 0.0001, "The prediction using simple linear regression was not calculated correctly");

        }
    }
}
