using System;
using System.Text;

namespace InterfaceContractsGenerator.Extensions
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends a line with indents.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="line">the line</param>
        /// <param name="stack">indent depth</param>
        /// <param name="debug">true: write to console. For debugging</param>
        public static void AppendLine(this StringBuilder buffer, string line = null, int stack = 0, bool debug = false)
        {
            var indent = string.Empty.PadLeft(stack*4);
            buffer.Append(indent);
            if (debug) Console.Write(indent);

            buffer.AppendLine(line);
            if (debug) Console.WriteLine(line);
        }

        /// <summary>
        /// Append a empty line with indents.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="debug">true: write to console. For debugging</param>
        public static void AppendEmptyLine(this StringBuilder buffer, bool debug = false)
        {
            buffer.AppendLine();
            if (debug) Console.WriteLine();
        }
    }
}
