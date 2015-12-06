using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Knapsack
{
  class GeneratorSetupGenerator
  {
    private static void GenerateRatioCapacityToWeightSum(ICollection<GeneratorSetup> list)
    {
      for (var f = 0.1f; f <= 1.0f; f += 0.05f)
      {
        GeneratorSetup gen = new GeneratorSetup();
        gen.GeneratorName = MethodBase.GetCurrentMethod().Name;
        gen.RatioCapacityToWeightSum = f;
        list.Add(gen);
      }
    }

    private static void GenerateMaxWeight(ICollection<GeneratorSetup> list)
    {
      for (var i = 100; i <= 5100; i += 500)
      {
        GeneratorSetup gen = new GeneratorSetup();
        gen.GeneratorName = MethodBase.GetCurrentMethod().Name;
        gen.MaxWeight = i;
        list.Add(gen);
      }
    }

    private static void GenerateMaxCost(ICollection<GeneratorSetup> list)
    {
      for (var i = 100; i <= 5100; i += 500)
      {
        GeneratorSetup gen = new GeneratorSetup();
        gen.GeneratorName = MethodBase.GetCurrentMethod().Name;
        gen.MaxCost = i;
        list.Add(gen);
      }
    }

    private static void GenerateExponent(ICollection<GeneratorSetup> list)
    {
      for (var f = 0.01f; f <= 3.0f; f += 0.05f)
      {
        GeneratorSetup gen = new GeneratorSetup();
        gen.GeneratorName = MethodBase.GetCurrentMethod().Name;
        gen.Exponent = f;
        list.Add(gen);
      }
    }

    private static void GenerateLessEqualGreater(ICollection<GeneratorSetup> list)
    {
      var genL = new GeneratorSetup();
      var genE = new GeneratorSetup();
      var genG = new GeneratorSetup();

      genL.LessThings = true;
      genL.EqualThings = false;
      
      genE.EqualThings = true;

      genG.LessThings = false;
      genG.EqualThings = false;

      list.Add(genL);
      list.Add(genE);
      list.Add(genG);

      genL.GeneratorName = MethodBase.GetCurrentMethod().Name;
      genE.GeneratorName = MethodBase.GetCurrentMethod().Name;
      genG.GeneratorName = MethodBase.GetCurrentMethod().Name;
    }

    public List<GeneratorSetup> GetAllSetups()
    {
      var res = new List<GeneratorSetup>();
      GenerateRatioCapacityToWeightSum(res);
      GenerateMaxWeight(res);
      GenerateMaxCost(res);
      GenerateExponent(res);
      GenerateLessEqualGreater(res);

      foreach (GeneratorSetup gs in res)
      {
        gs.GeneratorName = gs.GeneratorName.Replace("Generate", "");
      }

      return res;
    }
  }
}
