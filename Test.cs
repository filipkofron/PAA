using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Knapsack
{
  class Test : IComparable<Test>
  {
    private List<Knapsack> _knapsacks = new List<Knapsack>();
    private GeneratorSetup _generatorSetup;

    public Test(string filePath)
    {
      string solutionFileName = PathToSolutionPath(filePath);
      var knapsackReader = new KnapsackReader(filePath, solutionFileName);
      
      Knapsack knapsack;
      do
      {
        knapsack = knapsackReader.ReadKnapsack();

        if (knapsack != null)
          _knapsacks.Add(knapsack);
      } while (knapsack != null);
    }

    public Test(StreamReader streamReader, GeneratorSetup generatorSetup)
    {
      KnapsackReader knapsackReader = new KnapsackReader(streamReader);
      _generatorSetup = generatorSetup;

      Knapsack knapsack;
      do
      {
        knapsack = knapsackReader.ReadKnapsack();

        if (knapsack != null)
          _knapsacks.Add(knapsack);
      } while (knapsack != null);
    }

    private int N()
    {
      if (_knapsacks.Count == 0) return 0;
      return _knapsacks[0].ItemValues.Length/2;
    }

    private static string[] SplitPath(string path)
    {
      var split1 = path.Split('\\');
      List<string> splitted = new List<string>();
      foreach (string spl in split1)
      {
        var split2 = spl.Split('/');
        splitted.AddRange(split2);
      }
      return splitted.ToArray();
    }

    private static string FileNameToSolutionFileName(string filename)
    {
      return filename.Replace("inst", "sol");
    }

    private string PathToSolutionPath(string filename)
    {
      var splitted = SplitPath(filename);
      if (splitted.Length < 1) throw new FormatException("Invalid filename: " + filename);

      string solutionFileName = FileNameToSolutionFileName(splitted[splitted.Length - 1]);

      var reconStructPath = "";
      for (int i = 0; i < splitted.Length - 1; i++)
      {
        reconStructPath += splitted[i] + "/";
      }

      return reconStructPath + "sols/" + solutionFileName;
    }
    
    public TestResult RunTest(Algorithm algorithm)
    {
      if (_knapsacks.Count == 0) return null;
      Stopwatch avgSW = new Stopwatch();
      avgSW.Start();
      long maxTime = 0;
      double avgError = 0;
      double maxError = 0;
      int iterations = 0;
      foreach (var knapsack in _knapsacks)
      {
        Stopwatch currSW = new Stopwatch();
        currSW.Start();
        int solution = knapsack.Solve(algorithm);
        long currTime = (long) ((1000000000.0 * (double)currSW.ElapsedTicks) / Stopwatch.Frequency);
        if (currTime > maxTime)
        {
          maxTime = currTime;
        }
        double currError = Math.Abs((knapsack.Solution - solution) / (double) knapsack.Solution);
        avgError += currError;
        if (currError > maxError)
        {
          maxError = currError;
        }
        iterations++;

        if (currTime > 2000000000 && iterations == 10)
          break;

        if (currTime > 10000000000 && iterations == 5)
          break;

        if (currTime > 100000000000 && iterations == 1)
          break;
      }
      long avgTime = (long) ((1000000000.0 * (double)avgSW.ElapsedTicks) / Stopwatch.Frequency) / iterations;
      avgError /= iterations;

      return new TestResult(avgError, maxError, avgTime, maxTime, N(), _generatorSetup);
    }

    public int CompareTo(Test other)
    {
      return N() - other.N();
    }
  }
}
