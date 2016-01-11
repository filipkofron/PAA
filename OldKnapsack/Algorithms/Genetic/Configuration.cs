using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knapsack.Algorithms.Genetic
{
  class Configuration
  {
    private Knapsack _knapsack;
    private bool[] _presence;

    public Configuration(Knapsack knapsack)
    {
      _knapsack = knapsack;
      _presence = new bool[knapsack.ItemValues.Length / 2];
    }

    public Configuration(Configuration configuration)
    {
      _knapsack = configuration._knapsack;
      _presence = new bool[configuration._presence.Length];
      configuration._presence.CopyTo(_presence, 0);
    }

    public void Randomize(Mutator mutator)
    {
      Mutate(mutator, _presence.Length);
    }

    public void Fix(Mutator mutator)
    {
      int weight = SumWeight();
      while (weight > _knapsack.Capacity)
      {
        int idx = mutator.Rand(_presence.Length);
        int dir = mutator.Rand(2);
        int found = -1;
        if (dir == 0)
        {
          for (int i = idx; i < _presence.Length; i++)
          {
            if (_presence[idx])
            {
              found = idx;
              break;
            }
          }
        }
        else
        {
          for (int i = idx; i >= 0; i--)
          {
            if (_presence[idx])
            {
              found = idx;
              break;
            }
          }
        }
        if (found >= 0)
        {
          _presence[found] = false;
          weight -= _knapsack.ItemValues[found * 2];
        }
      }
    }

    public void Mutate(Mutator mutator, int count)
    {
      Debug.Assert(count <= _presence.Length);
      mutator.Mutate(_presence, count);
    }

    public Configuration Cross(Mutator mutator, Configuration other, int count)
    {
      Configuration configuration = new Configuration(_knapsack);
      mutator.Mutate(configuration._presence, count);

      for (int i = 0; i < _presence.Length; i++)
      {
        configuration._presence[i] = configuration._presence[i] ? other._presence[i] : _presence[i];
      }

      return configuration;
    }

    public int SumCost()
    {
      int sum = 0;
      for (int i = 0; i < _presence.Length; i++)
      {
        if (_presence[i])
        {
          sum += _knapsack.ItemValues[i * 2 + 1];
        }
      }
      return sum;
    }

    public int SumWeight()
    {
      int sum = 0;
      for (int i = 0; i < _presence.Length; i++)
      {
        if (_presence[i])
        {
          sum += _knapsack.ItemValues[i * 2];
        }
      }
      return sum;
    }

    private float SumRatio()
    {
      float sum = 0;
      for (int i = 0; i < _presence.Length; i++)
      {
        if (_presence[i])
        {
          sum += _knapsack.ItemValues[i * 2 + 1] / (float) _knapsack.ItemValues[i * 2];
        }
      }
      return sum;
    }

    public float Evaluate()
    {
      float sumCost = SumCost();
      float sumWeight = SumWeight();
      if (sumWeight > _knapsack.Capacity)
      {
        sumCost *= 0.3f;
        sumCost *= _knapsack.Capacity / sumWeight;
      }
      return sumCost;
    }

    public bool Same(Configuration configuration)
    {
      for (int i = 0; i < _presence.Length; i++)
      {
        if (_presence[i] != configuration._presence[i])
        {
          return false;
        }
      }
      return true;
    }
  }
}
