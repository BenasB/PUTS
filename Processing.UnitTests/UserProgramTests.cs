using System.IO;
using Xunit;

namespace Processing.UnitTests
{
  public class UserProgramTests
  {
    UserProgram userProgram = new UserProgram();
    UserProgram.Result.StatusType failed = UserProgram.Result.StatusType.Failed;

    [Fact]
    public void SetSource_PathGiven_ShouldFail()
    {
      var result = userProgram.SetSource(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()));

      Assert.Equal(failed, result.Status);
    }

    [Fact]
    public void SetSource_WrongFileGiven_ShouldFail()
    {
      var result = userProgram.SetSource(Path.Combine("TestFiles", "testfake.cpp"));

      Assert.Equal(failed, result.Status);
    }

    [Fact]
    public void SetSource_WrongFileExtention_ShouldFail()
    {
      var result = userProgram.SetSource(Path.Combine("TestFiles", "test.txt"));

      Assert.Equal(failed, result.Status);
    }
  }
}
