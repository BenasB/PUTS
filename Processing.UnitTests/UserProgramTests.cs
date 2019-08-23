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
            var result = userProgram.SetSource("C:/");

            Assert.Equal(failed, result.Status);
        }

        [Fact]
        public void SetSource_WrongFileGiven_ShouldFail()
        {
            var result = userProgram.SetSource(@"TestFiles\testfake.cpp");

            Assert.Equal(failed, result.Status);
        }

        [Fact]
        public void SetSource_WrongFileExtention_ShouldFail()
        {
            var result = userProgram.SetSource(@"TestFiles\test.txt");

            Assert.Equal(failed, result.Status);
        }
    }
}
