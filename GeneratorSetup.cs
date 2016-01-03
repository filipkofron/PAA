using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack
{
  class GeneratorSetup
  {
    public int StartingId { get; set; } = 0;
    public int ItemCount { get; set; } = 20;

    public int InstanceCount { get; set; } = 10;
    public float RatioCapacityToWeightSum { get; set; } = 0.5f;
    public int MaxWeight { get; set; } = 1000;
    public int MaxCost { get; set; } = 1000;
    public float Exponent { get; set; } = 1.2f;
    public bool EqualThings { get; set; } = true;
    public bool LessThings { get; set; } = true;

    public GeneratorSetup()
    {
    }

    public GeneratorSetup(GeneratorSetup orig)
    {
      StartingId = orig.StartingId;
      ItemCount = orig.ItemCount;
      InstanceCount = orig.InstanceCount;
      RatioCapacityToWeightSum = orig.RatioCapacityToWeightSum;
      MaxWeight = orig.MaxWeight;
      MaxCost = orig.MaxCost;
      Exponent = orig.Exponent;
      EqualThings = orig.EqualThings;
      LessThings = orig.LessThings;
    }

    public string ToFolderName()
    {
      return $"{StartingId}_{ItemCount}_{InstanceCount}_{RatioCapacityToWeightSum}_{MaxWeight}_{MaxCost}_{Exponent}_{EqualThings}_{LessThings}";
    }
  }
}
