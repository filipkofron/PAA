#include "CNFGenerator.h"
#include <ctime>
#include <iostream>

std::random_device CNFGenerator::_rd;
// pseudo random generator
std::mt19937 CNFGenerator::_gen;

void CNFGenerator::GenerateWeights(const std::shared_ptr<CNFProblem>& problem)
{
  std::uniform_int_distribution<> distribution(1, 50);
  for (int i = 0; i < 100; i++)
  {
    std::cout << "<" << distribution(_gen) << "> ";
  }
  std::cout << std::endl;
}

void CNFGenerator::GenerateForm(const std::shared_ptr<CNFProblem>& problem)
{
  std::uniform_int_distribution<> presenceDistr(-1, 1);
  for (int i = 0; i < 100; i++)
  {
    std::cout << "<" << presenceDistr(_gen) << "> ";
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