/*
 * Made by Mikuláš Hoblík in 2020
 * PROGameStudios s.r.o.
 * This work is subject to MIT License
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using PredictDemandLibrary;

namespace PredictDemand
{
    class Program
    {
        private static Predictor predictor = new Predictor();
        private static float correctValue;

        private static float[] data;
        private static int[] dates;

        private static string splitBy = ",";

        private static void GetData(string[] fileData)
        {
            data = new float[fileData.Length];
            dates = new int[fileData.Length];

            bool split = false;
            string startDate = "";

            string[] testRow = fileData[0].Split(splitBy);
            if (testRow.Length > 1)
            {
                split = true;
                startDate = testRow[1];
            }

            for (int i = 0; i < fileData.Length; i++)
            {
                if (split)
                {
                    string[] row = fileData[i].Split(splitBy);

                    data[i] = float.Parse(row[0]);
                    dates[i] = DayToNum(row[1], startDate);
                }
                else
                {
                    data[i] = float.Parse(fileData[i]);
                }
            }
        }

        private static void AssignData()
        {
            predictor.data = data;

            if (dates.Length > 0)
            {
                predictor.dates = dates;
            }
        }

        static void Main(string[] args)
        {
            // reading input
            string[] command = Console.ReadLine().Split(' ');

            // measuring the process time
            int startMilliseconds = DateTime.Now.Millisecond;

            // reading and formatting data
            GetData(File.ReadAllLines(command[0]));
            AssignData();

            // calculating results based of off user preferences
            string function = ProccessCommands(command);
            float result = CalculateResult(function);

            // displaying results
            Console.WriteLine(result.ToString());
            Console.WriteLine(Math.Abs(DateTime.Now.Millisecond - startMilliseconds).ToString() + " ms");

            Console.ReadKey();
        }

        private static float CalculateResult(string function)
        {
            float result = 0;

            if (function == "auto")
            {
                function = AutoChooseFunction(data.Length - 1);
            }

            switch (function)
            {
                case "average":
                    result = predictor.PredictUsingAverage();
                    break;
                case "median":
                    result = predictor.PredictUsingMedian();
                    break;
                case "trends":
                    result = predictor.PredictUsingTrends();
                    break;
                case "changes":
                    result = predictor.PredictUsingAbsoluteChanges();
                    break;
                case "regression":
                    result = (float)predictor.PredictUsingRegression();
                    break;
                default:
                    result = (float)predictor.PredictUsingRegression();
                    break;
            }

            return result;
        }

        private static string ProccessCommands(string[] command)
        {
            string function = "";

            for (int i = 1; i < command.Length; i++)
            {
                switch (command[i])
                {
                    case "-average":
                        function = "average";
                        break;
                    case "-median":
                        function = "median";
                        break;
                    case "-trends":
                        function = "trends";
                        break;
                    case "-changes":
                        function = "changes";
                        break;
                    case "-regression":
                        function = "regression";
                        break;
                    case "-auto":
                        function = "auto";
                        break;
                    case "-threshold":
                        if (i + 1 < command.Length)
                        {
                            predictor.threshold = float.Parse(command[i + 1]);
                        }
                        break;
                }
            }

            return function;
        }

        private static string AutoChooseFunction(int lengthToUseForPrediction)
        {
            Predictor autoPredictor = new Predictor();

            correctValue = predictor.data[lengthToUseForPrediction];
            string[] functions = { "average", "median", "trends", "changes", "regression" };
            List<float> errors = new List<float>();

            float[] data = new float[lengthToUseForPrediction];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = predictor.data[i];
            }

            autoPredictor.data = data;
            autoPredictor.dates = predictor.dates;

            errors.Add(GetAbsoluteError(autoPredictor.PredictUsingAverage()));
            errors.Add(GetAbsoluteError(autoPredictor.PredictUsingMedian()));
            errors.Add(GetAbsoluteError(autoPredictor.PredictUsingTrends()));
            errors.Add(GetAbsoluteError(autoPredictor.PredictUsingAbsoluteChanges()));
            errors.Add(GetAbsoluteError((float)autoPredictor.PredictUsingRegression()));

            float numToFind = 10000000000;
            foreach (float error in errors)
            {
                if (error < numToFind)
                {
                    numToFind = error;
                }
            }

            for (int i = 0; i < errors.Count; i++)
            {
                if (errors[i] == numToFind)
                {
                    return functions[i];
                }
            }

            return "trends";
        }

        private static float GetAbsoluteError(float value)
        {
            float error = Math.Abs(correctValue - value);

            return error;
        }

        static int DayToNum(string date, string startDate)
        {
            DateTime start = DateTime.Parse(startDate);
            DateTime dt = DateTime.Parse(date);
            TimeSpan t = dt - start;
            return (int)t.TotalDays;
        }
    }
}
