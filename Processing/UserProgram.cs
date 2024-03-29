﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Processing
{
  /// <summary>
  /// Represents a program (solution to a specific problem) that the User creates
  /// </summary>
  public class UserProgram
  {
    public struct Result
    {
      public enum StatusType { Successful, Failed }
      public StatusType Status { get; set; }
      public string Message { get; set; }
    }

    public static readonly string[] AllowedExtentions = { ".cpp", ".c" };
    const int timeoutInterval = 2000;

    string sourceFilePath;
    List<string> programResult;

    /// <summary>
    /// Sets the path to the source file that will be compiled
    /// </summary>
    public Result SetSource(string filePath)
    {
      if (File.Exists(filePath))
      {
        if (Path.HasExtension(filePath) && AllowedExtentions.Any(extension => extension.Equals(Path.GetExtension(filePath))))
        {
          sourceFilePath = filePath;
          return new Result { Status = Result.StatusType.Successful, Message = "Source file set" };
        }
        else
          return new Result { Status = Result.StatusType.Failed, Message = "File extension not supported" };
      }
      else if (Directory.Exists(filePath))
      {
        return new Result { Status = Result.StatusType.Failed, Message = "This is a directory, please specify a file" };
      }

      return new Result { Status = Result.StatusType.Failed, Message = "File not found" };
    }

    /// <summary>
    /// Compiles the source file and places the output in the same place as the source file
    /// </summary>
    public Result Compile()
    {
      ProcessStartInfo compilerStartInfo = new ProcessStartInfo()
      {
        FileName = "g++",
        Arguments = sourceFilePath + " -o " + Path.ChangeExtension(sourceFilePath, ".exe") + " -std=c++11",
        RedirectStandardOutput = true,
        RedirectStandardInput = true,
        RedirectStandardError = true
      };

      Console.WriteLine("Trying to compile file at: " + sourceFilePath);

      try
      {
        using (Process compiler = new Process())
        {
          compiler.StartInfo = compilerStartInfo;
          compiler.Start();
          compiler.WaitForExit();

          if (compiler.ExitCode != 0)
            return new Result() { Status = Result.StatusType.Failed, Message = $"Compiler error occured\n{compiler.StandardError.ReadToEnd().Replace(sourceFilePath, "solution.cpp")}" };
          else
          {
            return new Result() { Status = Result.StatusType.Successful, Message = "Compiled successfully" };
          }
        }
      }
      catch (Exception e)
      {
        return new Result() { Status = Result.StatusType.Failed, Message = $"Compiler process error\n{e.Message.Replace(sourceFilePath, "solution.cpp")}" };
      }
    }

    /// <summary>
    /// Executes the compiled user program and prints the initial data to stdin
    /// </summary>
    public Result Execute(string initialData = "")
    {
      ProcessStartInfo programStartInfo = new ProcessStartInfo()
      {
        FileName = Path.ChangeExtension(sourceFilePath, ".exe"),
        RedirectStandardOutput = true,
        RedirectStandardInput = true,
        RedirectStandardError = true
      };

      using (Process program = new Process())
      {
        try
        {
          program.StartInfo = programStartInfo;
          program.Start();

          program.StandardInput.WriteLine(initialData);

          if (!program.WaitForExit(timeoutInterval))
          {
            program.Kill();
            return new Result() { Status = Result.StatusType.Failed, Message = $"Program timed out" };
          }

          programResult = new List<string>();
          string line;
          while ((line = program.StandardOutput.ReadLine()) != null)
          {
            programResult.Add(line.TrimEnd());
          }
          return new Result() { Status = Result.StatusType.Successful, Message = "Program executed successfully" };
        }
        catch (Exception e)
        {
          program.Kill();
          return new Result() { Status = Result.StatusType.Failed, Message = $"Program process error\n{e.Message}" };
        }
      }
    }

    /// <summary>
    /// Evaluates the program result with the expected result
    /// </summary>
    public Result Evaluate(string expected)
    {
      if (String.Join(' ', programResult).Trim().Equals(expected))
        return new Result() { Status = Result.StatusType.Successful, Message = "Output matches" };
      else
        return new Result() { Status = Result.StatusType.Failed, Message = $"Output doesn't match\nExpected: {expected}\nReturned: {programResult}" };
    }

    /// <summary>
    /// Evaluates the program result with the expected result
    /// </summary>
    public Result Evaluate(string[] expected)
    {
      if (programResult.SequenceEqual(expected))
        return new Result() { Status = Result.StatusType.Successful, Message = "Output matches" };
      else
        return new Result() { Status = Result.StatusType.Failed, Message = $"Output doesn't match\nExpected: {expected}\nReturned: {programResult}" };
    }

    /// <summary>
    /// Evaluates the program result and returns the result of the program in the Message property if the evaluation is not true
    /// </summary>
    public Result EvaluateAndGetResultIfFailed(string[] expected)
    {
      if (programResult.SequenceEqual(expected))
        return new Result() { Status = Result.StatusType.Successful, Message = "Output matches" };
      else
        return new Result() { Status = Result.StatusType.Failed, Message = String.Join('\n', programResult) };
    }
  }
}
