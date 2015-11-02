using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Knapsack
{
  class ResultPrinter
  {
    private List<TestResult> _testResults;
    private string _algorithmName;

    public ResultPrinter(List<TestResult> testResults, string algorithmName)
    {
      _testResults = testResults;
      _algorithmName = algorithmName;
      if (File.Exists(GetFileName()))
      {
        File.Delete(GetFileName());
      }
    }

    private string GetFileName()
    {
      return "results_" + _algorithmName + ".txt";
    }

    private void Out(string text)
    {
      File.AppendAllText(GetFileName(), text);
      Console.Write(text);
    }

    public void Print()
    {
      Out("N,Avg. error,Max. error,Avg. time,Max. time\r\n");
      foreach (var result in _testResults)
      {
        Out(result.N + "," + $"{result.AverageError:F12}" + "," + $"{result.MaxError:F12}" + "," + (result.AverageTime / 1000000.0) + "," + (result.MaxTime / 1000000.0) + "\r\n");
      }
    }
  }
}
