using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knapsack.Algorithms.Genetic
{
  class Generation
  {
    private class ConfigurationSortedItem : IComparable<ConfigurationSortedItem>
    {
      private int _idx;
      private float _value;

      public ConfigurationSortedItem(int idx, float value)
      {
        this._idx = idx;
        this._value = value;
      }

      public int CompareTo(ConfigurationSortedItem other)
      {
        float res = _value - other._value;

        if (res > 0)
        {
          return 1;
        }

        if (res < 0)
        {
          return -1;
        }

        return 0;
      }

      public int Idx
      {
        get { return _idx; }
      }
    }

    private List<Configuration> _configurations;
    private Knapsack _knapsack;

    public Generation(Mutator mutator, int size, Knapsack knapsack)
    {
      _knapsack = knapsack;
      _configurations = new List<Configuration>(size);
      for (int i = 0; i < size; i++)
      {
        Configuration configuration = new Configuration(knapsack);
        configuration.Randomize(mutator);
        _configurations.Add(configuration);
      }
    }

    public Generation(Mutator mutator, List<Configuration> bestConfigurations, int fullSize, Knapsack knapsack)
    {
      _knapsack = knapsack;
      _configurations = new List<Configuration>(fullSize);
      Debug.Assert(bestConfigurations.Count <= fullSize);
      int i = 0;
      for (i = 0; i < bestConfigurations.Count; i++)
      {
        _configurations.Add(new Configuration(bestConfigurations[i]));
      }
      for (; i < fullSize; i++)
      {
        Configuration configuration = new Configuration(knapsack);
        configuration.Randomize(mutator);
        _configurations.Add(configuration);
      }
    }

    public void MutateDuplicates(Mutator mutator, float percent)
    {
      int flipCount = (int)((_knapsack.ItemValues.Length * percent) * 0.5f);
      List<Configuration> newConfigs = new List<Configuration>();
      if (_configurations.Count > 0)
      {
        newConfigs.Add(_configurations[0]);
      }
      for (int i = 1; i < _configurations.Count; i++)
      {
        if (_configurations[i].Same(_configurations[i - 1]))
        {
          Configuration config = _configurations[i];
          config.Mutate(mutator, flipCount);
          newConfigs.Add(config);
        }
        else
        {
          newConfigs.Add(_configurations[i]);
        }
      }
      _configurations = newConfigs;
    }

    public void Mutate(Mutator mutator, int maxSize, float percent, float countPercent)
    {
      int flipCount = (int) ((_knapsack.ItemValues.Length * percent) * 0.5f);
      int count = (int)((maxSize * percent) * 0.5f);
      Mutator.Shuffle(_configurations);
      List<Configuration> newConfigs = new List<Configuration>(count);
      for (int i = 0; i < count && i < _configurations.Count; i++)
      {
        Configuration config = new Configuration(_configurations[i]);
        config.Mutate(mutator, flipCount);
        newConfigs.Add(config);
      }
      _configurations.AddRange(newConfigs);
    }

    public void Fix(Mutator mutator)
    {
      foreach (Configuration config in _configurations)
      {
        if (mutator.Rand(20) == 0)
        {
          config.Fix(mutator);
        }        
      }
    }

    public void Cross(Mutator mutator, int maxSize, Generation other, float percent, float countPercent)
    {
      int crossCount = (int)((_knapsack.ItemValues.Length * percent) * 0.5f);
      int count = (int)((maxSize * percent) * 0.5f);
      Configuration[] prevConfigs = new Configuration[other._configurations.Count];
      other._configurations.CopyTo(prevConfigs);
      Mutator.Shuffle(_configurations);
      List<Configuration> newConfigs = new List<Configuration>(count);
      for (int i = 0; i < count && i < other._configurations.Count; i++)
      {
        Configuration config = new Configuration(_configurations[i]);
        config.Cross(mutator, prevConfigs[i], crossCount);
        newConfigs.Add(config);
      }
      _configurations.AddRange(newConfigs);
    }

    public Generation SelectBest(Mutator mutator, int count, int maxSize)
    {
      SortedSet<ConfigurationSortedItem> set = new SortedSet<ConfigurationSortedItem>();
      for (int i = 0; i < _configurations.Count; i++)
      {
        set.Add(new ConfigurationSortedItem(i, _configurations[i].Evaluate()));
      }
      List<Configuration> configurations = new List<Configuration>(count);
      foreach (ConfigurationSortedItem configurationSortedItem in set.Reverse())
      {
        if (configurations.Count >= count)
          break;

        configurations.Add(_configurations[configurationSortedItem.Idx]);
      }
      return new Generation(mutator, configurations, maxSize, _knapsack);
    }

    public Configuration GetFirst()
    {
      return _configurations[0];
    }

    public override string ToString()
    {
      string str = "";
      foreach (Configuration config in _configurations)
      {
        str += config.Evaluate() + "/" + config.SumCost() + " ";
      }
      return str;
    }
  }
}
