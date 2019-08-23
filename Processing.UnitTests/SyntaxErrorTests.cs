using Xunit;

namespace Processing.UnitTests
{
    public class SyntaxErrorTests
    {
        UserProgram userProgram = new UserProgram();
        UserProgram.Result.StatusType successful = UserProgram.Result.StatusType.Successful;
        UserProgram.Result.StatusType failed = UserProgram.Result.StatusType.Failed;

        [Fact]
        public void SetSource_TestWithSyntaxError_ShouldBeSuccessful()
        {
            var result = userProgram.SetSource(@"TestFiles\test_syntax_error.cpp");

            Assert.Equal(successful, result.Status);
        }

        [Fact]
        public void Compile_TestWithSyntaxError_ShouldFail()
        {
            userProgram.SetSource(@"TestFiles\test_syntax_error.cpp");
            var result = userProgram.Compile();

            Assert.Equal(failed, result.Status);
        }
    }
}
