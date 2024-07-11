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
      //克隆仓库
      cloneRepo(repoUrl,localRepoDir);

      using (Repository repoPtr = new Repository(localRepoDir))
      {
        //获得 仓库提交列表
        List<LibGit2Sharp.Commit> commitLs=GetGitCommits(repoPtr);
        LibGit2Sharp.Commit endCommit=commitLs[commitLs.Count-1];
        LibGit2Sharp.Commit commit0=commitLs[0];
        LibGit2Sharp.Commit commit1=commitLs[1];
        Console.WriteLine($"commitLs.Count={commitLs.Count}, commitLs[0]= {commit0} , commitLs[1]= {commit1} , commitLs[-1]= {endCommit}  ");
        // commitLs.Count=601, commitLs[0]= a1e7272fa856d38eb3d59ba15532e2280381419d , commitLs[1]= 53810ef8bf8140f19284eb85ea809f3ea8a7ca74 , commitLs[-1]= 5681647c83fca0b18d0c8ba4c53ea4a48f57e35f  
      
      }

    }
    catch (Exception ex)
    {
    Console.WriteLine($"An error occurred: {ex.Message}");
    }
  }

  /*libgit2sharp克隆仓库
  libgit2sharp并没有带出libgit2的repoPtr, 
  但是 libgit2sharp的` new Repository` 返回值 基本就代表了libgit2中的repoPtr  ， 以后用repoPtr进行各种操作即可
  */
  static void cloneRepo(string repoUrl,string localRepoDir)
  {
    Console.WriteLine($"Cloning from '{repoUrl}' to '{localRepoDir}'...");
    // 克隆仓库
    Repository.Clone(repoUrl, localRepoDir);
    Console.WriteLine("Repository cloned successfully.");
  }

  /*libgit2sharp获得 仓库提交列表
  */
  static List<LibGit2Sharp.Commit> GetGitCommits(Repository repoPtr )
  {
    return repoPtr.Commits.ToList();
  }


}//end_of_class
}//end_of_namespace
