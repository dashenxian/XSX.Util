using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace XSX.Helper
{
    public static class CmdHelper
    {
        public static int SuccessfulExitCode = 0;

        public static void Run(string file, string arguments)
        {
            var procStartInfo = new ProcessStartInfo(file, arguments);
            Process.Start(procStartInfo)?.WaitForExit();
        }

        public static int RunCmd(string command)
        {
            var procStartInfo = new ProcessStartInfo(
                GetFileName(),
                GetArguments(command)
            );

            using (var process = Process.Start(procStartInfo))
            {
                process?.WaitForExit();
                return process?.ExitCode ?? 0;
            }
        }

        public static string RunCmdAndGetOutput(string command)
        {
            return RunCmdAndGetOutput(command, out int _);
        }

        public static string RunCmdAndGetOutput(string command, out bool isExitCodeSuccessful)
        {
            var output = RunCmdAndGetOutput(command, out int exitCode);
            isExitCodeSuccessful = exitCode == SuccessfulExitCode;
            return output;
        }

        public static string RunCmdAndGetOutput(string command, out int exitCode)
        {
            string output;

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(CmdHelper.GetFileName())
                {
                    Arguments = CmdHelper.GetArguments(command),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                process.Start();

                using (var standardOutput = process.StandardOutput)
                {
                    using (var standardError = process.StandardError)
                    {
                        output = standardOutput.ReadToEnd();
                        output += standardError.ReadToEnd();
                    }
                }

                process.WaitForExit();

                exitCode = process.ExitCode;
            }

            return output.Trim();
        }

        public static string GetArguments(string command)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "-c \"" + command + "\"";
            }

            //Windows default.
            return "/C \"" + command + "\"";
        }

        public static string GetFileName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                //TODO: Test this. it should work for both operation systems.
                return "/bin/bash";
            }

            //Windows default.
            return "cmd.exe";
        }
        public static List<string> RunCmdAndGetOutput(string command, string arguments)
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            process.Start();

            var errorBuilder = new StringBuilder();
            var output = new List<string>();
            process.OutputDataReceived += (_, args) =>
            {
                if (args.Data != null)
                {
                    output.Add(args.Data);
                }
            };
            process.BeginOutputReadLine();
            process.ErrorDataReceived += (_, args) =>
            {
                if (args.Data != null)
                {
                    errorBuilder.AppendLine(args.Data);
                }
            };
            process.BeginErrorReadLine();
            if (!process.DoubleWaitForExit())
            {
                var timeoutError = $@"Process timed out. Command line: {command} {arguments}.
Output: {string.Join(Environment.NewLine, output)}
Error: {errorBuilder}";
                throw new Exception(timeoutError);
            }

            if (process.ExitCode == 0)
            {
                return output;
            }


            var errors = errorBuilder.ToString();
            if (errors.Contains("Cannot find a tool in the manifest file that has a command named"))
            {
                throw new Exception("The dotnet tool `dotnet-symbol` was not found.");
            }

            var error = $@"Could not execute process. Command line: {command} {arguments}.
Output: {string.Join(Environment.NewLine, output)}
Error: {errors}";
            throw new Exception(error);
        }

        public static bool DoubleWaitForExit(this Process process)
        {
            //4min30sec
            var timeout = 270000;
            var result = process.WaitForExit(timeout);
            if (result)
            {
                process.WaitForExit();
            }

            return result;
        }
    }
}
