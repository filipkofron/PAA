// Evolution.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "CNFGenerator.h"
#include "BitGen.h"
#include <iostream>
#include "Population.h"
#include "Config.h"
#include "Cycle.h"
#include "3sat.h"

void Evolution()
{
  const int variableNum = 40;
  const int multiplesNum = 160;
  const int seed = 111;

  //auto problem = CNFGenerator::GenerateOne(100, 1000);
  auto problem = gen_3_sat(variableNum, multiplesNum, 50, seed);
  Config<CNFProblem> config;
  config._populationSize = 200;
  config._bitCount = variableNum;
  config._crossCount = config._populationSize * 2 / 3;
  config._mutationCount = config._populationSize * 2 / 3;
  config._mutationBitCount = variableNum / 20;
  config._mutationBitCount = config._mutationBitCount < 1 ? 1 : config._mutationBitCount;
  config._problem = problem;

  Cycle<CNFProblem> cycle(config);
  for (int i = 0; i < 500; i++)
  {
    cycle.Step();
    //cycle.Print();
    std::cout << "[" << i << "] Best: " << cycle.GetBestFitness()
      << " sol: " << cycle.GetBestIndividual().GetSolutionPercentage(*problem)
      << " weight: " << cycle.GetBestIndividual().GetBestWeight(*problem) << std::endl;
  }
  //std::cout << " Problem: " << *problem << std::endl;
  std::cout << " Best vector: " << cycle.GetBestIndividual() << std::endl;
  std::cout << " Best weight: " << cycle.GetBestIndividual().GetBestWeight(*problem) << std::endl;
  std::cout << " Best fitness: " << cycle.GetBestFitness() << std::endl;
}

int main()
{
  Evolution();
  
  return 0;
}
