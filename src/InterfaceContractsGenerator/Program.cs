using System;
using System.Collections.Generic;
using ManyConsole;

namespace InterfaceContractsGenerator
{
    class Program
    {
        /// <summary>
        /// Please see GeneratorCommand.Run.
        /// </summary>
        /// <param name="args">the agrs</param>
        /// <returns>code</returns>
        static int Main(string[] args)
        {
            var commands = GetCommands();
            return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }

        public static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
        }
    }
}
