using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack
{
  class GeneratorSetup
  {
    public int StartingId { get; set; } = 0;
    public int ItemCount { get; set; } = 22;

    public int InstanceCount { get; set; } = 50;
    public float RatioCapacityToWeightSum { get; set; } = 0.5f;
    public int MaxWeight { get; set; } = 1000;
    public int MaxCost { get; set; } = 1000;
    public float Exponent { get; set; } = 1.0f;
    public bool EqualThings { get; set; } = false;
    public bool LessThings { get; set; } = false;
    public String GeneratorName { get; set; } = "None";

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
      GeneratorName = orig.GeneratorName;
    }

    public string ToFolderName()
    {
      return $"{GeneratorName}_{StartingId}_{ItemCount}_{InstanceCount}_{RatioCapacityToWeightSum}_{MaxWeight}_{MaxCost}_{Exponent}_{EqualThings}_{LessThings}";
    }
  }
}
