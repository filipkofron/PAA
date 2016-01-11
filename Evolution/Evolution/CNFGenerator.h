#pragma once
#include <memory>
#include "CNFProblem.h"
#include <random>

// Weighted CNF random generator class, doesn't need an instance
class CNFGenerator
{
private:
  // random device
  static std::random_device _rd;
  // pseudo random generator
  static std::mt19937 _gen;

  // Generates weights in the given problem
  static void GenerateWeights(const std::shared_ptr<CNFProblem>& problem);
  // Generates whole form in the given problem
  static void GenerateForm(const std::shared_ptr<CNFProblem>& problem);
public:
  // Generates exactly one random CNF problem
  static std::shared_ptr<CNFProblem> GenerateOne(size_t varNum, size_t mulNum);
};