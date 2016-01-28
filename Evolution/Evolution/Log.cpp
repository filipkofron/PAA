#include "Log.h"

static int CreateDirectoryRecursively(const std::string& path)
{
  return SHCreateDirectoryEx(nullptr, path.c_str(), nullptr);
}

static std::string CurrentDateTime()
{
  time_t     now = time(0);
  struct tm  tstruct;
  char       buf[80];
  tstruct = *localtime(&now);
  strftime(buf, sizeof(buf), "%Y-%m-%d__%X", &tstruct);
  char* ptr = buf;
  while(*ptr)
  {
    if (*ptr == ':')
      *ptr = '_';
    ptr++;
  }
  return buf;
}

static std::string CurrTime;

Log::Log(const std::string& logName)
{
  CreateDirectoryRecursively(GetDirPath());
  _out.open(GetDirPath() + "/" + logName + ".csv");
}

Log::~Log()
{
  _out.close();
}

std::ostream& Log::operator()()
{
  return _out;
}

std::string Log::GetDirPath() const
{
  if (CurrTime.empty())
  {
    CurrTime = CurrentDateTime();
  }
  char pwd[MAX_PATH];
  GetCurrentDirectory(MAX_PATH, pwd);
  return std::string(pwd) + "/" + CurrTime;
}
