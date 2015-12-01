using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Knapsack.Algorithms;

namespace Knapsack
{
  class Program
  {
    private static void Preheat(List<Algorithm> algorithms)
    {
      TestSuit testSuit = new TestSuit();
      testSuit.LoadTestsFromDir("../../preheat");

      foreach (var algo in algorithms)
      {
        Console.WriteLine("Preheating algorithm: " + algo.GetType().Name + " [" + algo.GetConfig() + "]");
        testSuit.RunAllTests(algo);
      }
    }

    private static void RunGeneratedTests(List<Algorithm> algorithms)
    {
      TestSuit testSuit = new TestSuit();
      testSuit.GenerateTests();

      Console.WriteLine("========== Initiating tests ==========");

      foreach (var algo in algorithms)
      {
        Console.WriteLine("Running algorithm: " + algo.GetType().Name + " [" + algo.GetConfig() + "]");
        new ResultPrinter(testSuit.RunAllTests(algo), algo).Print();
      }
    }

    private static void RunTests(List<Algorithm> algorithms)
    {
      TestSuit testSuit = new TestSuit();
      testSuit.LoadTestsFromDir("../../data");

      Console.WriteLine("========== Initiating tests ==========");

      foreach (var algo in algorithms)
      {
        Console.WriteLine("Running algorithm: " + algo.GetType().Name + " [" + algo.GetConfig() + "]");
        new ResultPrinter(testSuit.RunAllTests(algo), algo).Print();
      }
    }

    static void Main(string[] args)
    {
      using (var p = Process.GetCurrentProcess())
        p.PriorityClass = ProcessPriorityClass.High;
      var algorithms = new List<Algorithm>
      {
        new BBRecursiveBruteforce(),
        new Heurestic(),
        new CostDecomposition(),
        new CostFPTAS(0.04),
        new CostFPTAS(0.1),
        new CostFPTAS(0.125),
        new CostFPTAS(0.175),
        new CostFPTAS(0.225),
        new CostFPTAS(0.275),
        new CostFPTAS(0.325),
        new CostFPTAS(0.375),
        new IterativeBruteforce(),
        new RecursiveBruteforce(),
      };

      Preheat(algorithms);
      //RunTests(algorithms);
      RunGeneratedTests(algorithms);

      Console.ReadKey();
    }
  }
}
