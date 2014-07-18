using System;
using System.Configuration;
using System.IO;
using ManyConsole;

namespace InterfaceContractsGenerator.ConsoleCommands
{
    /// <summary>
    /// Generator Command.
    /// </summary>
    public class GeneratorCommand : ConsoleCommand
    {
        /// <summary>
        /// Gets or sets the input directory path.
        /// </summary>
        public string InputDirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets the output directory path.
        /// </summary>
        public string OutputDirectoryPath { get; set; }

        /// <summary>
        /// Generator Command
        /// </summary>
        /// <example>
        /// This sample shows how to call the interface generator command.
        /// <code>
        ///     InterfaceContractsGenerator.exe inputDirectoryPath="C:\Project\Library" outputDirectoryPath="C:\output"
        /// </code>
        /// </example>
        public GeneratorCommand()
        {
            IsCommand("generator", "Generates interface contracts.");

            HasRequiredOption(
                "i|inputDirectoryPath=",
                "The {INPUT DIRECTORY} path to find all c# files in this directory and sub directories.",
                v => InputDirectoryPath = v);

            HasRequiredOption(
                "o|outputDirectoryPath=",
                "The {OUTPUT DIRECTORY} path for output.",
                v => OutputDirectoryPath = v);
        }

        /// <summary>
        /// Run the command.
        /// </summary>
        /// <param name="remainingArguments">the remainingArguments</param>
        /// <returns>status code</returns>
        public override int Run(string[] remainingArguments)
        {
            if (!Directory.Exists(OutputDirectoryPath))
            {
                Directory.CreateDirectory(OutputDirectoryPath);
            }

            var types = ConfigurationManager.AppSettings["skipTypes"];
            if (types != null)
            {
                Generator.SkipTypes = types.Split(',');
            }

            foreach (var file in new DirectoryInfo(InputDirectoryPath).GetFiles(@"*.cs", SearchOption.AllDirectories))
            {
                var inputFilePath = file.FullName;
                var contractsFilePath = Generator.GetContractsFilePath(InputDirectoryPath, inputFilePath, OutputDirectoryPath);

                var code = File.ReadAllText(inputFilePath);
                if (string.IsNullOrWhiteSpace(code))
                {
                    continue;
                }

                var result = Generator.Generate(code);
                if (string.IsNullOrWhiteSpace(result))
                {
                    continue;
                }

                Console.WriteLine("inputFile    =" + inputFilePath);
                Console.WriteLine("outputputFile=" + contractsFilePath);
                Console.WriteLine("  generated.");

                if (!string.IsNullOrWhiteSpace(result))
                {
                    var newFile = new FileInfo(contractsFilePath);
                    if (!newFile.Directory.Exists)
                    {
                        newFile.Directory.Create();
                    }

                    if (newFile.Exists)
                    {
                        newFile.Delete();
                    }

                    File.WriteAllText(contractsFilePath, result);
                }
            }

            return 0;
        }
    }
}