using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Int32;

namespace Knapsack
{
  class KnapsackReader
  {
    private readonly StreamReader _streamReader;
    private readonly StreamReader _solutionStreamReader;
    private int _lineNumber = 0;
    
    public Knapsack ReadKnapsack()
    {
      if (_streamReader.EndOfStream || _solutionStreamReader.EndOfStream)
      {
        _streamReader.Close();
        _solutionStreamReader.Close();
        return null;
      }

      var line = _streamReader.ReadLine();
      var solutionLine = _solutionStreamReader.ReadLine();

      if (string.IsNullOrEmpty(solutionLine) || string.IsNullOrEmpty(line))
      {
        _streamReader.Close();
        _solutionStreamReader.Close();
        return null;
      }

      var splitted = line.Split(' ');
      var solutionSplitted = solutionLine.Split(' ');

      if (splitted.Length < 4) throw new FormatException("Invalid format of input data on line " + _lineNumber);
      var n = Parse(splitted[1]);
      if (n < 0) throw new FormatException("Invalid format of input data on line " + _lineNumber);
      var capacity = Parse(splitted[2]);

      var solution = Parse(solutionSplitted[2]);

      var values = new int[n * 2];
      
      _lineNumber++;

      for (var i = 0; i < n; i++)
      {
        values[i * 2] = Parse(splitted[3 + i * 2]);
        values[i * 2 + 1] = Parse(splitted[3 + i * 2 + 1]);
      }

      Knapsack knapsack = new Knapsack(values, capacity, solution);

      return knapsack;
    }

    public KnapsackReader(string file, string solutionFile)
    {
      _streamReader = new StreamReader(file);
      _solutionStreamReader = new StreamReader(solutionFile);
    }
  }
}
