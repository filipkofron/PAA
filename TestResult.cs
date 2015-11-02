using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack
{
  class TestResult
  {
    public double AverageError { get; }
    public double MaxError { get; }
    public double AverageTime { get; }
    public double MaxTime { get; }
    public int N { get; }

    public TestResult(double averageError, double maxError, double averageTime, double maxTime, int n)
    {
      AverageError = averageError;
      MaxError = maxError;
      AverageTime = averageTime;
      MaxTime = maxTime;
      N = n;
    }
  }
}
