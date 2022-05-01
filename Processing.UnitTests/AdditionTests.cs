using System.IO;
using Xunit;

namespace Processing.UnitTests
{
  public class AdditionTests
  {
    UserProgram userProgram = new UserProgram();
    UserProgram.Result.StatusType successful = UserProgram.Result.StatusType.Successful;
    UserProgram.Result.StatusType failed = UserProgram.Result.StatusType.Failed;

    [Fact]
    public void SetSource_AdditionFileGiven_ShouldBeSuccessful()
    {
      var result = userProgram.SetSource(Path.Combine("TestFiles", "test_addition.cpp"));

      Assert.Equal(successful, result.Status);
    }

    [Fact]
    public void Compile_AdditionFileGiven_ShouldBeSuccessful()
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_addition.cpp"));
      var result = userProgram.Compile();

      Assert.Equal(successful, result.Status);
    }

    [Theory]
    [InlineData("1 2")]
    public void Execute_AdditionFileGiven_ShouldBeSuccessful(string args)
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_addition.cpp"));
      userProgram.Compile();
      var result = userProgram.Execute(args);

      Assert.Equal(successful, result.Status);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    public void Execute_AdditionFileGiven_ShouldTimeout(string args)
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_addition.cpp"));
      userProgram.Compile();
      var result = userProgram.Execute(args);

      Assert.Equal(failed, result.Status);
    }

    [Theory]
    [InlineData("5 3", "8")]
    [InlineData("-7 2", "-5")]
    [InlineData("5 3\n\n", "8")]
    [InlineData("-7\n2", "-5")]
    public void Evaluate_MatchingOutputGiven_ShouldBeSuccessful(string args, string evaluation)
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_addition.cpp"));
      userProgram.Compile();
      userProgram.Execute(args);
      var result = userProgram.Evaluate(evaluation);

      Assert.Equal(successful, result.Status);
    }

    [Theory]
    [InlineData("7 8", "11")]
    [InlineData("8 -11", "-1")]
    public void Evaluate_NotMatchingOutputGiven_ShouldFail(string args, string evaluation)
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_addition.cpp"));
      userProgram.Compile();
      userProgram.Execute(args);
      var result = userProgram.Evaluate(evaluation);

      Assert.Equal(failed, result.Status);
    }
  }
}
