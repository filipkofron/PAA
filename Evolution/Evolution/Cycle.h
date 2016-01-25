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
    : _population(config._populationSize, config._bitCount), _config(config)
  {
    config._problem->PrecomputeMaxWeight();
  }

  void Step()
  {
    Individual<Problem>** startNew = _population.Sort(*_config._problem);
    _population.Cross(_config._crossCount, _config._crossBitCount, &startNew);
    _population.Mutate(_config._mutationCount, _config._crossBitCount, &startNew);
  }

  const Individual<Problem>& GetBestIndividual()
  {
    _population.Sort(*_config._problem);
    return _population.GetBestIndividual();
  }

  float GetBestFitness()
  {
    _population.Sort(*_config._problem);
    return _population.GetBestIndividual().GetFitness(*_config._problem);
  }

  void Print()
  {
    _population.Print();
  }
};
