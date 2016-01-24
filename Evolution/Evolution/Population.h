#pragma once
#include "Individual.h"
#include <algorithm>

template<typename Problem>
class Population
{
private:
  std::vector<Individual<Problem> *> _individuals;
  std::vector<Individual<Problem> *> _individualsOld;

public:
  Population(size_t initSize, size_t individualLength)
  {
    for (size_t i = 0; i < initSize; i++)
    {
      _individuals.push_back(new Individual<Problem> (individualLength));
      _individualsOld.push_back(new Individual<Problem>(individualLength));
    }
  }

  ~Population()
  {
    for (size_t i = 0; i < _individuals.size(); i++)
    {
      delete _individuals[i];
      delete _individualsOld[i];
    }
  }

  void Mutate(size_t indCount, size_t bitCount, Individual<Problem>*** start)
  {
    size_t maxInv = _individuals.size();
    for (size_t i = 0; i < indCount; i++)
    {
      size_t idx = BitGen::GetRandomUInt32() % maxInv;
      ***start = *_individuals[idx];
      (**start)->Mutate(bitCount);
      ++(*start);
    }
  }

  void Cross(size_t indCount, size_t bitCount, Individual<Problem>*** start)
  {
    size_t maxInv = _individuals.size();
    for (size_t i = 0; i < indCount; i++)
    {
      size_t idx = BitGen::GetRandomUInt32() % maxInv;
      size_t idx2 = BitGen::GetRandomUInt32() % maxInv;
      ***start = *_individuals[idx];
      (**start)->Cross(*_individuals[idx2], bitCount);
      ++(*start);
    }
  }

  Individual<Problem>** Sort(Problem& problem)
  {
    std::sort(_individuals.begin(), _individuals.end(),
    [&] (Individual<Problem>* a, Individual<Problem>* b)
    {
      return a->GetFitness(problem) < b->GetFitness(problem);
    }
    );
    return _individuals.data();
  }

  Individual<Problem>& GetBestIndividual() const
  {
    return *_individuals[_individuals.size() - 1];
  }

  void Print()
  {
    for (size_t i = 0; i < _individuals.size(); i++)
    {
      std::cout << *_individuals[i] << std::endl;
    }
  }
};
