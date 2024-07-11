using System;
using LibGit2Sharp;

/*
运行命令:
rm -fr /tmp/gitReopTest01_for_libgit2/ ; dotnet build && dotnet run
*/

namespace CloneRepo
{
  class Program
  {

    static void Main(string[] args){
      string repoUrl = "http://giteaz:3000/bal/cmd-wrap.git";
      string localRepoDir = "/tmp/gitReopTest01_for_libgit2";

      try{
      cloneRepo(repoUrl,localRepoDir);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"An error occurred: {ex.Message}");
      }
    }
    static void cloneRepo(string repoUrl,string localRepoDir)
    {
        // Clone the repository
        Console.WriteLine($"Cloning from '{repoUrl}' to '{localRepoDir}'...");
        Repository.Clone(repoUrl, localRepoDir);
        Console.WriteLine("Repository cloned successfully.");
    }
  }
}
