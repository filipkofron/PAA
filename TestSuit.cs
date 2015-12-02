using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Knapsack
{
  class TestSuit
  {
    private List<Test> _tests = new List<Test>();

    private Stream GenerateStreamFromString(string s)
    {
      MemoryStream stream = new MemoryStream();
      StreamWriter writer = new StreamWriter(stream);
      writer.Write(s);
      writer.Flush();
      stream.Position = 0;
      return stream;
    }

    private StreamReader GenerateTest(GeneratorSetup generatorSetup)
    {
      StringBuilder outputBuilder = new StringBuilder();
      string output = "";
      using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
      using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
      {
        using (Process process = new Process())
        {
          process.StartInfo.FileName = "../../knapgen.exe";
          process.StartInfo.UseShellExecute = false;
          process.EnableRaisingEvents = true;
          process.StartInfo.RedirectStandardOutput = true;
          process.StartInfo.RedirectStandardError = true;
          process.StartInfo.Arguments =
            $"-I {generatorSetup.StartingId}" +
            $" -n {generatorSetup.ItemCount}" +
            $" -N {generatorSetup.InstanceCount}" +
            $" -m {generatorSetup.RatioCapacityToWeightSum}" +
            $" -W {generatorSetup.MaxWeight}" +
            $" -C {generatorSetup.MaxCost}" +
            $" -k {generatorSetup.Exponent}" +
            $" -d {(generatorSetup.EqualThings ? 0 : (generatorSetup.LessThings ? -1 : 1))}";

          try
          {
            process.OutputDataReceived += (sender, e) =>
            {
              if (e.Data != null)
              {
                outputBuilder.AppendLine(e.Data);
              }
            };
            process.ErrorDataReceived += (sender, e) =>
            {
              if (e.Data != null)
              {
                // outputBuilder.AppendLine(e.Data);
              }
            };

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            output = outputBuilder.ToString();
          }
          finally
          {
            outputWaitHandle.WaitOne(1);
            errorWaitHandle.WaitOne(1);
          }
        }
      }
      
      return new StreamReader(GenerateStreamFromString(output));

    }

    public void GenerateTests(List<GeneratorSetup> generatorSetups)
    {
      foreach (var generatorSetup in generatorSetups)
      {
        StreamReader streamReader = GenerateTest(generatorSetup);
        _tests.Add(new Test(streamReader, generatorSetup));
        streamReader.Close();
      }
    }

    public void LoadTestsFromDir(string path)
    {
      var fileNames = Directory.GetFiles(path);
      foreach (var file in fileNames)
      {
        _tests.Add(new Test(file));
      }
      _tests.Sort();
    }

    public List<TestResult> RunAllTests(Algorithm algorithm)
    {
      List<TestResult> testResults = new List<TestResult>();
      foreach (Test test in _tests)
      {
        testResults.Add(test.RunTest(algorithm));
        Console.WriteLine("[" + testResults[testResults.Count - 1].N + "] done.");
      }
      return testResults;
    }
  }
}
