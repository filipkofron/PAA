using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack
{
  abstract class Algorithm
  {
    protected Knapsack _knapsack = null;
    protected int _size;

    public Knapsack Knapsack
    {
      get { return _knapsack; }
      set
      {
        _knapsack = value;
        _size = _knapsack.ItemValues.Length / 2;
      }
    }

    public abstract int Solve();
  }
}
