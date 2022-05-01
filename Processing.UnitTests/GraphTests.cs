using System.IO;
using Xunit;

namespace Processing.UnitTests
{
  public class GraphTests
  {
    UserProgram userProgram = new UserProgram();
    UserProgram.Result.StatusType successful = UserProgram.Result.StatusType.Successful;
    UserProgram.Result.StatusType failed = UserProgram.Result.StatusType.Failed;

    [Fact]
    public void SetSource_GraphFileGiven_ShouldBeSuccessful()
    {
      var result = userProgram.SetSource(Path.Combine("TestFiles", "test_graph.cpp"));

      Assert.Equal(successful, result.Status);
    }

    [Fact]
    public void Compile_GraphFileGiven_ShouldBeSuccessful()
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_graph.cpp"));
      var result = userProgram.Compile();

      Assert.Equal(successful, result.Status);
    }

    [Theory]
    [InlineData("4 4 0 3 0 1 1 0 2 2 1 3 4 2 3 2")]
    public void Execute_GraphFileGiven_ShouldBeSuccessful(string args)
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_graph.cpp"));
      userProgram.Compile();
      var result = userProgram.Execute(args);

      Assert.Equal(successful, result.Status);
    }

    [Theory]
    [InlineData("")]
    [InlineData("4 4 0 3 0 1 1 0 2 2 1 3 4 2 3")]
    [InlineData("4 5 0 3 0 1 1 0 2 2 1 3 4 2 3 2")]
    public void Execute_GraphFileGiven_ShouldTimeout(string args)
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_graph.cpp"));
      userProgram.Compile();
      var result = userProgram.Execute(args);

      Assert.Equal(failed, result.Status);
    }

    [Theory]
    [InlineData("4 3 0 3 0 1 1 1 2 3 2 3 2", "6")]
    [InlineData("4 4 0 3 0 1 1 0 2 2 1 3 4 2 3 2", "4")]
    [InlineData("8 0 1 4 0 2 6 1 2 8 1 3 3 2 3 2 4 5 7 4 6 5 5 6 8 6 7 2", "-1")]
    public void Evaluate_MatchingOutputGiven_ShouldBeSuccessful(string args, string evaluation)
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_graph.cpp"));
      userProgram.Compile();
      userProgram.Execute(args);
      var result = userProgram.Evaluate(evaluation);

      Assert.Equal(successful, result.Status);
    }

    [Theory]
    [InlineData("4 4 0 3 0 1 1 0 2 2 1 3 4 2 3 2", "5")]
    public void Evaluate_NotMatchingOutputGiven_ShouldFail(string args, string evaluation)
    {
      userProgram.SetSource(Path.Combine("TestFiles", "test_graph.cpp"));
      userProgram.Compile();
      userProgram.Execute(args);
      var result = userProgram.Evaluate(evaluation);

      Assert.Equal(failed, result.Status);
    }
  }
}
