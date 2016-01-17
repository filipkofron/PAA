#include "CNFGenerator.h"
#include <ctime>
#include <iostream>

std::random_device CNFGenerator::_rd;
// pseudo random generator
std::mt19937 CNFGenerator::_gen;

void CNFGenerator::GenerateWeights(const std::shared_ptr<CNFProblem>& problem)
{
  std::uniform_int_distribution<> distribution(1, 50);

  auto varNum = problem->GetVarNum();
  auto weights = problem->GetWeights();

  for (int i = 0; i < varNum; i++)
    weights[i] = distribution(_gen);;
}

void CNFGenerator::GenerateForm(const std::shared_ptr<CNFProblem>& problem)
{
  std::uniform_int_distribution<> presenceDistr(-1, 1);

  auto& prob = *problem;

  auto varNum = prob.GetVarNum();
  auto mulNum = prob.GetMulNum();

  for (int i = 0; i < mulNum; i++)
  {
    for (int j = 0; j < varNum; j++)
    {
      prob[i][j] = presenceDistr(_gen);
    }
  }
  std::cout << std::endl;
}

std::shared_ptr<CNFProblem> CNFGenerator::GenerateOne(size_t varNum, size_t mulNum)
{
  auto problem = std::make_shared<CNFProblem>(varNum, mulNum);
  
  GenerateWeights(problem);
  GenerateForm(problem);
  

  return problem;
}