using System;
using LibGit2Sharp;
using System.Text.Json;

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
      
        //提交0的文件列表 == diff(commit0.tree, commit1.tree)
        TreeChanges tree_diff =GetFileListOfCommit0(repoPtr, commit0.Sha,  commit1.Sha);
        string tree_diff__jsonTxt=JsonSerializer.Serialize(tree_diff);
        Console.WriteLine($"tree_diff__jsonTxt={tree_diff__jsonTxt}");
        // tree_diff__jsonTxt=[{"Path":"readme.md","Mode":33188,"Oid":{"RawId":"0J6Pk1ViaG9HGPZHDXRzFIzR+Nk=","Sha":"d09e8f935562686f4718f6470d7473148cd1f8d9"},"Exists":true,"Status":3,"OldPath":"readme.md","OldMode":33188,"OldOid":{"RawId":"XRb1gMw3H8nQK1QEsEZd3UetbVw=","Sha":"5d16f580cc371fc9d02b5404b0465ddd47ad6d5c"},"OldExists":true},{"Path":"work_list.md","Mode":33188,"Oid":{"RawId":"eLEL9P4rTB9Vga/TbR01FIETTBE=","Sha":"78b10bf4fe2b4c1f5581afd36d1d351481134c11"},"Exists":true,"Status":1,"OldPath":"work_list.md","OldMode":0,"OldOid":{"RawId":"AAAAAAAAAAAAAAAAAAAAAAAAAAA=","Sha":"0000000000000000000000000000000000000000"},"OldExists":false}]
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

  /*libgit2sharp获得 提交的文件列表
  提交0的文件列表 == diff(commit0.tree, commit1.tree)
  */
  static TreeChanges GetFileListOfCommit0(Repository repoPtr ,string commit0Hash, string commit1Hash)
  {
    //提交0的文件列表 == diff(commit0.tree, commit1.tree)
    LibGit2Sharp.Commit commit0= repoPtr.Lookup<Commit>( commit0Hash);
    LibGit2Sharp.Commit commit1= repoPtr.Lookup<Commit>(commit1Hash);
    LibGit2Sharp.Tree tree0 = commit0.Tree;
    LibGit2Sharp.Tree tree1 = commit1.Tree;
    TreeChanges tree_diff = repoPtr.Diff.Compare<TreeChanges>(tree1, tree0);//猜测 这行 最终调用了 libgit2的 git_diff_tree_to_tree

    return tree_diff;
  }


}//end_of_class
}//end_of_namespace
