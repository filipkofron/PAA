#pragma once
#undef UNICODE
#include <string>
#include <fstream>
#include <ctime>
#define _WIN32_WINNT 0x0500
#include <windows.h>
#include <shlobj.h>

class Log
{
private:
  std::ofstream _out;
public:
  Log(const std::string& logName);
  ~Log();
  std::ostream& operator()();
  std::string GetDirPath() const;
};