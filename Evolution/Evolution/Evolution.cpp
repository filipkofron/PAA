// Evolution.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Log.h"
#include "CNFGenerator.h"
#include "BitGen.h"
#include <iostream>
#include "Population.h"
#include "Config.h"
#include "Cycle.h"
#include "3sat.h"

Log Conf("Config");

void Evolution()
{
  const int variableNum = 50;
  const int multiplesNum = 100;
  const int seed = 456789;

  Conf() << "Variables,Multiples,Seed" << std::endl;
  Conf() << variableNum << "," << multiplesNum << "," << seed << std::endl;

  //auto problem = CNFGenerator::GenerateOne(100, 1000);
  auto problem = gen_3_sat(variableNum, multiplesNum, 50, seed);
  Config<CNFProblem> config;
  config._populationSize = 500;
  config._bitCount = variableNum;
  config._crossCount = config._populationSize * 2 / 3;
  config._mutationCount = config._populationSize * 2 / 3;
  config._mutationBitCount = variableNum / 20;
  config._mutationBitCount = config._mutationBitCount < 1 ? 1 : config._mutationBitCount;
  config._problem = problem;

  Cycle<CNFProblem> cycle(config);
  for (int i = 0; i < 200; i++)
  {
/*    if (cycle.HasSolution())
      break;
      */
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
