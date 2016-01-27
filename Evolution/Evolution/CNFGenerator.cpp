#include "CNFGenerator.h"
#include <ctime>
#include <iostream>

std::random_device CNFGenerator::_rd;
// pseudo random generator
std::mt19937 CNFGenerator::_gen(_rd());

std::uniform_int_distribution<> _distribution(1, 50);
std::uniform_int_distribution<> _presenceDistr(-1, 1);

void CNFGenerator::GenerateWeights(const std::shared_ptr<CNFProblem>& problem)
{
  auto varNum = problem->GetVarNum();
  auto weights = problem->GetWeights();

  for (int i = 0; i < varNum; i++)
    weights[i] = _distribution(_gen);

  problem->PrecomputeMaxWeight();
}

void CNFGenerator::GenerateForm(const std::shared_ptr<CNFProblem>& problem)
{
  auto& prob = *problem;

  auto varNum = prob.GetVarNum();
  auto mulNum = prob.GetMulNum();

  for (int i = 0; i < mulNum; i++)
  {
    for (int j = 0; j < varNum; j++)
    {
      if (_presenceDistr(_gen) != 0 || _presenceDistr(_gen) != 0)
        prob[i][j] = 0;
      else
        prob[i][j] = _presenceDistr(_gen);
    }
  }
}

std::shared_ptr<CNFProblem> CNFGenerator::GenerateOne(size_t varNum, size_t mulNum)
{
  auto problem = std::make_shared<CNFProblem>(varNum, mulNum);
  
  GenerateWeights(problem);
  GenerateForm(problem);
  

  return problem;
}