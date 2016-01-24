// Evolution.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "CNFGenerator.h"
#include "BitGen.h"
#include <iostream>
#include "Population.h"
#include "Config.h"
#include "Cycle.h"


int main()
{
  auto problem = CNFGenerator::GenerateOne(100, 1000);
  Config<CNFProblem> config;
  config._populationSize = 40;
  config._bitCount = 100;
  config._crossCount = 5;
  config._crossBitCount = 5;
  config._mutationCount = 5;
  config._mutationBitCount = 5;
  config._problem = problem;

  Cycle<CNFProblem> cycle(config);
  for (int i = 0; i < 200; i++)
  {
    cycle.Step();
    //cycle.Print();
    std::cout << "[" << i << "] Best: " << cycle.GetBestFitness() << std::endl;
  }
  std::cout << " Problem: " << *problem << std::endl;
  std::cout << " Best vector: " << cycle.GetBestIndividual() << std::endl;
  std::cout << " Best: " << cycle.GetBestFitness() << std::endl;
  
  return 0;
}
