using System;
using System.Collections.Generic;
using System.IO;
namespace Knapsack
{
    class TestSuit
  {
    private List<Test> _tests = new List<Test>();

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
