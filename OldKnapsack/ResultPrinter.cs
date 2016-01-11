using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Text;

namespace Knapsack
{
  class ResultPrinter
  {
    private List<TestResult> _testResults;
    private Algorithm _algorithm;
    private List<KeyValuePair<Algorithm, List<TestResult>>> _genTestResults;

    public ResultPrinter(List<TestResult> testResults, Algorithm algorithm)
    {
      _testResults = testResults;
      _algorithm = algorithm;

      GeneratorSetup generatorSetup = null;
      if (_testResults.Count > 0)
        generatorSetup = _testResults[0].GeneratorSetup;

      if (File.Exists(GetFileName(null)))
      {
        File.Delete(GetFileName(null));
      }
    }

    public ResultPrinter(List<KeyValuePair<Algorithm, List<TestResult>>> genTestResults)
    {
      this._genTestResults = genTestResults;
    }

    private string GetFileName(Algorithm algorithm)
    {
      string config = "";
      if (algorithm == null)
      {
        config = _algorithm.GetConfig();
      }

      string dirWithSlash = "";

      if (algorithm != null)
        dirWithSlash = algorithm.GetType().Name + "/";

      if (!string.IsNullOrEmpty(dirWithSlash))
      {
        Directory.CreateDirectory(dirWithSlash.Substring(0, dirWithSlash.Length - 1));
      }

      string fileId;

      if (algorithm != null)
      {
        fileId = "gen";
      }
      else
      {
        if (string.IsNullOrEmpty(config))
        {
          fileId = _algorithm.GetType().Name;
        }
        else
        {
          config = config.Replace(".", "_").Replace(":", "-").Replace(" ", "-");
          fileId = _algorithm.GetType().Name + "_" + config;
        }
      }

      return dirWithSlash + "results_" + fileId + ".txt" ;
    }

    private void Out(string text, Algorithm algorithm)
    {
      File.AppendAllText(GetFileName(algorithm), text);
      Console.Write(text);
    }

    private void PrintSimple()
    {
      Out("N,Avg. error,Max. error,Avg. time,Max. time\r\n", null);
      foreach (var result in _testResults)
      {
        Out(result.N + "," + $"{result.AverageError:F12}" + "," + $"{result.MaxError:F12}" + "," + (result.AverageTime / 1000000.0) + "," + (result.MaxTime / 1000000.0) + "\r\n", null);
      }
    }
    private void PrintSetupConfigs(List<TestResult> configs)
    {
      string filename = "setup_configs.txt";
      if (File.Exists(filename))
      {
        File.Delete(filename);
      }
      StringBuilder sb = new StringBuilder();

      foreach (TestResult config in configs)
      {
        sb.AppendLine(config.GeneratorSetup.ToFolderName());
      }

      string text = sb.ToString();
      File.AppendAllText(filename, text);
      Console.Write(text);
    }

    private void PrintGenSetups()
    {
      if (_genTestResults.Count > 0)
        PrintSetupConfigs(_genTestResults[0].Value); 

      foreach (KeyValuePair<Algorithm, List<TestResult>> pair in _genTestResults)
      {
        Algorithm algo = pair.Key;
        List<TestResult> testResults = pair.Value;

        if (File.Exists(GetFileName(algo)))
        {
          File.Delete(GetFileName(algo));
        }
        Out("N,Avg. error,Max. error,Avg. time,Max. time\r\n", algo);
        foreach (TestResult result in testResults)
        {
          Out(result.N + "," + $"{result.AverageError:F12}" + "," + $"{result.MaxError:F12}" + "," + (result.AverageTime / 1000000.0) + "," + (result.MaxTime / 1000000.0) + "\r\n", algo);
        }
      }
    }

    public void Print()
    {
      if (_genTestResults != null)
      {
        PrintGenSetups();
      }
      else
      {
        PrintSimple();
      }
    }
  }
}
