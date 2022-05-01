using System.IO;
using Xunit;

namespace Processing.UnitTests
{
  public class HelloWorldTests
  {
    UserProgram userProgram = new UserProgram();
    UserProgram.Result.StatusType successful = UserProgram.Result.StatusType.Successful;
    UserProgram.Result.StatusType failed = UserProgram.Result.StatusType.Failed;

    [Fact]
    public void SetSource_TestFileGiven_ShouldBeSuccessful()
    {
      var result = userProgram.SetSource(Path.Combine("TestFiles", "test_helloworld.cpp"));

      Assert.Equal(successful, result.Status);
    }

    [Fact]
    public void Compile_TestFileGiven_ShouldBeSuccessful()
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_helloworld.cpp"));
      var result = userProgram.Compile();

      Assert.Equal(successful, result.Status);
    }

    [Fact]
    public void Execute_TestFileGiven_ShouldBeSuccessful()
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_helloworld.cpp"));
      userProgram.Compile();
      var result = userProgram.Execute();

      Assert.Equal(successful, result.Status);
    }

    [Fact]
    public void Evaluate_MatchingOutputGiven_ShouldBeSuccessful()
    {
      string expected = "Hello World!";

      userProgram.SetSource(Path.Combine("TestFiles", "test_helloworld.cpp"));
      userProgram.Compile();
      userProgram.Execute();
      var result = userProgram.Evaluate(expected);

      Assert.Equal(successful, result.Status);
    }

    [Fact]
    public void Evaluate_NotMatchingOutputGiven_ShouldFail()
    {
      string notExpected = "Some sample text";

      userProgram.SetSource(Path.Combine("TestFiles", "test_helloworld.cpp"));
      userProgram.Compile();
      userProgram.Execute();
      var result = userProgram.Evaluate(notExpected);

      Assert.Equal(failed, result.Status);
    }
  }
}
