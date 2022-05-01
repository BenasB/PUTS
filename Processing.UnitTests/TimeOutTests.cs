using System.IO;
using Xunit;

namespace Processing.UnitTests
{
  public class TimeOutTests
  {
    UserProgram userProgram = new UserProgram();
    UserProgram.Result.StatusType successful = UserProgram.Result.StatusType.Successful;
    UserProgram.Result.StatusType failed = UserProgram.Result.StatusType.Failed;

    [Fact]
    public void SetSource_TimeoutTestFileGiven_ShouldBeSuccessful()
    {
      var result = userProgram.SetSource(Path.Combine("TestFiles", "test_timeout.cpp"));

      Assert.Equal(successful, result.Status);
    }

    [Fact]
    public void Compile_TimeoutTestFileGiven_ShouldBeSuccessful()
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_timeout.cpp"));
      var result = userProgram.Compile();

      Assert.Equal(successful, result.Status);
    }

    [Fact]
    public void Execute_TimeoutTestFileGiven_ShouldFail()
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_timeout.cpp"));
      userProgram.Compile();
      var result = userProgram.Execute();

      Assert.Equal(failed, result.Status);
    }
  }
}
