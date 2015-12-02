using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack.Algorithms
{
  class Heurestic : Algorithm
  {
    private unsafe int* _items;

    class Item : IComparable<Item>
    {
      private readonly int _index;
      readonly unsafe int* _items;

      public unsafe Item(int index, int* items)
      {
        _index = index;
        _items = items;
      }

      public unsafe int Weight()
      {
        return _items[_index * 2];
      }

      public unsafe int Cost()
      {
        return _items[_index * 2 + 1];
      }

      double Ratio()
      {
        return Cost() / (double) Weight();
      }

      int IComparable<Item>.CompareTo(Item other)
      {
        if (this == other) return 0;

        if (Weight() == 0 && other.Weight() == 0)
          return Cost() - other.Cost();

        if (Weight() == 0) return 1;
        if (other.Weight() == 0) return -1;
        var res = Ratio() - other.Ratio();

        if (res < 0) return -1;
        if (res > 0) return 1;
        return (int) res;
      }
    }
    public Heurestic()
    {
    }

    protected unsafe int SolveHeurestic()
    {
      List<Item> items = new List<Item>();
      for (int i = 0; i < _size; i++)
      {
        items.Add(new Item(i, _items));
      }

      items.Sort();

      var accCost = 0;
      var accWeight = 0;

      foreach (var item in items)
      {
        if (accWeight + item.Weight() > _knapsack.Capacity) continue;
        accWeight += item.Weight();
        accCost += item.Cost();
      }

      return accCost;
    }

    public unsafe override int Solve()
    {
      fixed (int* itemsPtr = &_knapsack.ItemValues[0])
      {
        _items = itemsPtr;
        return SolveHeurestic();
      }
    }

    public unsafe override void Clear()
    {
      _items = null;
    }
  }
}
