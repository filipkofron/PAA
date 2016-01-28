#pragma once
#include "Individual.h"
#include <algorithm>

template<typename Problem>
class Population
{
private:
  Problem& _problem;
  std::shared_ptr<std::vector<Individual<Problem> *> > _individuals;
  std::shared_ptr<std::vector<Individual<Problem> *> > _individualsOld;
  std::vector<float> _fitnesses;

public:
  Population(Problem& problem, size_t initSize, size_t individualLength)
    : _problem(problem), _individuals(std::make_shared<std::vector<Individual<Problem> *> >()), _individualsOld(std::make_shared<std::vector<Individual<Problem> *>>())
  {
    for (size_t i = 0; i < initSize; i++)
    {
      _individuals->push_back(new Individual<Problem> (individualLength, _problem));
      _individualsOld->push_back(new Individual<Problem>(individualLength, _problem));
      _fitnesses.push_back(0.0f);
    }
  }

  ~Population()
  {
    for (size_t i = 0; i < _individuals->size(); i++)
    {
      delete (*_individuals)[i];
      delete (*_individualsOld)[i];
    }
  }

  void Mutate(size_t indCount, size_t bitCount)
  {
    for (size_t i = 0; i < indCount; i++)
    {
      (*_individuals)[i]->Mutate(bitCount);
    }
  }

  void Randomize(size_t start)
  {
    // last is the best one left to survive
    size_t maxInd = _individuals->size() - 1;
    for (size_t i = start; i < maxInd; i++)
      (*_individuals)[i]->Mutate(_problem.GetVarNum());
  }

  size_t FindFitness(float fitness)
  {
    size_t indCount = _individuals->size();
    for (size_t i = 0; i < indCount; i++)
    {
      if (fitness < _fitnesses[i])
        return i;
    }
    return indCount - 1;
  }

  void Cross(size_t indCount)
  {
    for (size_t i = 0; i < indCount; i++)
    {
      size_t a = FindFitness(BitGen::GetRandomFloat01());
      size_t b = FindFitness(BitGen::GetRandomFloat01());
      
      size_t varNum = _problem.GetVarNum() / 2;

      *(*_individuals)[i] = *(*_individualsOld)[a];
      (*_individuals)[i]->Cross(*(*_individualsOld)[b], varNum);
    }
  }
  /*
  Individual<Problem>** Sort(Problem& problem)
  {
    std::sort(_individuals.begin(), _individuals.end(),
    [&] (Individual<Problem>* a, Individual<Problem>* b)
    {
      return a->GetFitness(problem) < b->GetFitness(problem);
    }
    );
    return _individuals.data();
  }*/

  static int IndividualCompare(const void* va, const void* vb)
  {
    void **vva = (void **) va;
    void **vvb = (void **) vb;
    Individual<Problem>** a = reinterpret_cast<Individual<Problem>**> (vva);
    Individual<Problem>** b = reinterpret_cast<Individual<Problem>**> (vvb);

    float res = (*a)->GetFitness((*a)->GetProblem()) - (*b)->GetFitness((*b)->GetProblem());

    if (res < 0)
      return -1;

    if (res > 0)
      return 1;

    return 0;
  }

  void Sort()
  {
    Individual<Problem>** data = _individuals->data();
    std::qsort(data, _individuals->size(), sizeof(Individual<Problem>*), &IndividualCompare);
  }

  void CalculateFitnesses()
  {
    size_t maxInd = _individuals->size();
    float fitnessSum = 0.0f;
    for (size_t i = 0; i < maxInd; i++)
    {
      fitnessSum += (*_individuals)[i]->GetFitness(_problem);
      _fitnesses[i] = fitnessSum;
    }

    // normalize
    for (size_t i = 0; i < maxInd; i++)
      _fitnesses[i] /= fitnessSum;
  }

  void SwapIndividuals()
  {
    auto oldInd = _individuals;
    _individuals = _individualsOld;
    _individualsOld = oldInd;

    *(*_individuals)[_individuals->size() - 1] = *(*_individualsOld)[_individualsOld->size() - 1];
  }

  Individual<Problem>& GetBestIndividual()
  {
    return *(*_individuals)[_individuals->size() - 1];
  }

  bool HasSolution() const
  {
    Individual<Problem>& ind = *(*_individuals)[_individuals->size() - 1];
    return abs(1.0f - ind.GetSolutionPercentage(_problem)) < 0.000001;

  }

  void Print()
  {
    for (size_t i = 0; i < _individuals->size(); i++)
    {
      std::cout << *(*_individuals)[i] << std::endl;
    }
  }
};
