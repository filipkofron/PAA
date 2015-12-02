using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack
{
  class GeneratorSetupGenerator
  {
    private static void GenerateRatioCapacityToWeightSum(ICollection<GeneratorSetup> list)
    {
      for (var f = 0.25f; f <= 2.0f; f += 0.25f)
      {
        GeneratorSetup gen = new GeneratorSetup();
        gen.RatioCapacityToWeightSum = f;
        list.Add(gen);
      }
    }

    private static void GenerateMaxWeight(ICollection<GeneratorSetup> list)
    {
      for (var i = 100; i <= 2100; i += 400)
      {
        GeneratorSetup gen = new GeneratorSetup();
        gen.MaxWeight = i;
        list.Add(gen);
      }
    }

    private static void GenerateMaxCost(ICollection<GeneratorSetup> list)
    {
      for (var i = 100; i <= 2100; i += 400)
      {
        GeneratorSetup gen = new GeneratorSetup();
        gen.MaxCost = i;
        list.Add(gen);
      }
    }

    private static void GenerateExponent(ICollection<GeneratorSetup> list)
    {
      for (var f = 0.25f; f <= 2.0f; f += 0.25f)
      {
        GeneratorSetup gen = new GeneratorSetup();
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
    }

    public List<GeneratorSetup> GetAllSetups()
    {
      var res = new List<GeneratorSetup>();
      GenerateRatioCapacityToWeightSum(res);
      GenerateMaxWeight(res);
      GenerateMaxCost(res);
      GenerateExponent(res);
      GenerateLessEqualGreater(res);

      return res;
    }
  }
}
