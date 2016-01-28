#pragma once
#include "Population.h"
#include "Config.h"

template<typename Problem>
class Cycle
{
private:
  Population<Problem> _population;
  Config<Problem> _config;
public:
  Cycle(const Config<Problem>& config)
    : _population(*config._problem, config._populationSize, config._bitCount), _config(config)
  {
    config._problem->PrecomputeMaxWeight();
  }

  void Step()
  {
    _population.Sort();
    _population.CalculateFitnesses();
    _population.SwapIndividuals();
    _population.Cross(_config._crossCount);
    _population.Mutate(_config._mutationCount, _config._mutationBitCount);

    // current method requires both to be equal
    // Assert(_config._crossCount == _config._mutationCount);

    _population.Randomize(_config._crossCount);
  }

  const Individual<Problem>& GetBestIndividual()
  {
    return _population.GetBestIndividual();
  }

  float GetBestFitness()
  {
    return _population.GetBestIndividual().GetFitness(*_config._problem);
  }

  void Print()
  {
    _population.Print();
  }

  bool HasSolution() const
  {
    return _population.HasSolution();
  }
};
