using System;
using Processing;

namespace MVP
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please enter a file path");
                return;
            }

            UserProgram userProgram = new UserProgram();
            UserProgram.Result sourceResult = userProgram.SetSource(args[0]);

            Console.WriteLine(sourceResult.Message);
            if (sourceResult.Status == UserProgram.Result.StatusType.Failed)          
                return;

            UserProgram.Result compilationResult = userProgram.Compile();

            Console.WriteLine(compilationResult.Message);
            if (compilationResult.Status == UserProgram.Result.StatusType.Failed)
                return;

            UserProgram.Result programResult = userProgram.Execute(args.Length < 2 ? "" : args[1]);

            Console.WriteLine(programResult.Message);
            if (programResult.Status == UserProgram.Result.StatusType.Failed)
                return;

            string expectedOutput = args.Length < 3 ? "Hello World!" : args[2];
            UserProgram.Result outputResult = userProgram.Evaluate(expectedOutput);
            Console.WriteLine(outputResult.Message);
        }
    }
}