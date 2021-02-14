using System;
using System.Collections.Generic;
using System.Text;

namespace PredictDemandLibrary
{
    public class Predictor
    {
        public float[] data { get; set; }
        public int[] dates { get; set; }

        public float threshold = 0.2f;

        #region regression
        public double PredictUsingRegression()
        {
            //CheckDates();

            double result = 0;

            result = CalculateIntercept() + (CalculateSlope() * (dates[dates.Length - 1] + 1));

            return result;
        }

        private void CheckDates()
        {
            bool noDate = true;

            if (dates != null)
            {
                if (dates.Length == data.Length)
                {
                    noDate = false;
                }
            }

            if (noDate)
            {
                return;
            }

            dates = new int[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                dates[i] = i;
            }

        }

        private double CalculateIntercept()
        {
            double leftSideUp = GetSumOfY() * GetSumOfXSquared();
            double rightSideUp = GetSumOfX() * GetSumOfXY();
            double up = leftSideUp - rightSideUp;

            double leftSideDown = GetN() * GetSumOfXSquared();
            double rightSideDown = GetSumOfX() * GetSumOfX();
            double down = leftSideDown - rightSideDown;

            return up / down;
        }

        private double CalculateSlope()
        {
            double leftSideUp = GetN() * GetSumOfXY();
            double rightSideUp = GetSumOfX() * GetSumOfY();
            double up = leftSideUp - rightSideUp;

            double leftSideDown = GetN() * GetSumOfXSquared();
            double rightSideDown = GetSumOfX() * GetSumOfX();
            double down = leftSideDown - rightSideDown;

            return up / down;
        }

        private double GetN()
        {
            return data.Length;
        }

        private double GetSumOfY()
        {
            double sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }

            return sum;
        }

        private double GetSumOfX()
        {
            double sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += dates[i];
            }

            return sum;
        }

        private double GetSumOfXSquared()
        {
            double sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += dates[i] * dates[i];
            }

            return sum;
        }

        private double GetSumOfXY()
        {
            double sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i] * dates[i];
            }

            return sum;
        }
        #endregion regression

        public float PredictUsingMedian()
        {
            List<float> data = new List<float>();
            foreach (float dataPoint in this.data)
            {
                data.Add(dataPoint);
            }
            data.Sort();

            int medianIndex = 0;
            if (data.Count % 2 == 0)
            {
                medianIndex = data.Count / 2;
            }
            else
            {
                medianIndex = (data.Count - 1) / 2;
            }
            return data[medianIndex];
        }

        public float PredictUsingAverage()
        {
            float sum = 0;
            foreach (float num in data)
            {
                sum += num;
            }

            return sum / data.Length;
        }

        public float PredictUsingTrends()
        {
            List<float> trends = new List<float>();

            for (int i = 0; i < data.Length; i++)
            {
                if (i + 1 < data.Length)
                {
                    float trend = data[i + 1] / data[i];

                    trends.Add(trend);
                }
            }

            float trendAverage = trends[0];
            for (int j = 1; j < trends.Count; j++)
            {
                trendAverage += trends[j];
            }
            trendAverage /= trends.Count;

            float result = data[data.Length - 1] * trendAverage;

            trends.Sort();
            int median = Convert.ToInt32(trends.Count / 2);
            float resultWithMedianThreshold = (trends[median] + threshold) * data[data.Length - 1];
            if (result > resultWithMedianThreshold)
            {
                return resultWithMedianThreshold;
            }    

            return result;
        }

        public float PredictUsingAbsoluteChanges()
        {
            List<float> changes = new List<float>();

            for (int i = 0; i < data.Length; i++)
            {
                if (i + 1 < data.Length)
                {
                    float change = data[i + 1] - data[i];

                    changes.Add(change);
                }
            }

            float changeAverage = changes[0];
            for (int j = 1; j < changes.Count; j++)
            {
                changeAverage += changes[j];
            }
            changeAverage /= changes.Count;

            float result = data[data.Length - 1] + changeAverage;

            changes.Sort();
            int median = Convert.ToInt32(changes.Count / 2);
            float resultWithMedianThreshold = (changes[median] * (1 + threshold)) + data[data.Length - 1];
            if (result > resultWithMedianThreshold)
            {
                return resultWithMedianThreshold;
            }

            return result;
        }
    }
}
