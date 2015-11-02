using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Knapsack
{
  class ResultPrinter
  {
    private List<TestResult> _testResults;
    private Algorithm _algorithm;

    public ResultPrinter(List<TestResult> testResults, Algorithm algorithm)
    {
      _testResults = testResults;
      _algorithm = algorithm;
      if (File.Exists(GetFileName()))
      {
        File.Delete(GetFileName());
      }
    }

    private string GetFileName()
    {
      string config = _algorithm.GetConfig();
      if (String.IsNullOrEmpty(config))
      {
        return "results_" + _algorithm.GetType().Name + ".txt";
      }
      config = config.Replace(".", "_").Replace(":", "-").Replace(" ", "-");
      return "results_" + _algorithm.GetType().Name + "_" + config + ".txt";
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
