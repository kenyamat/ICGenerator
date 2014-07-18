using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator.Extensions;

namespace InterfaceContractsGenerator.Appenders
{
    public class UsingAppender : AppenderBase
    {
        /// <summary>
        /// Appends usings into output buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="node">the node</param>
        /// <param name="stack">the stack</param>
        /// <param name="traverseFunc">the tracerse function</param>
        /// <example>
        /// Input
        /// <code><![CDATA[
        ///     using System;
        ///     using System.Linq;
        /// ]]></code>
        /// 
        /// Output
        /// <code><![CDATA[
        ///     using System;
        ///     using System.Diagnostics.Contracts;
        ///     using System.Linq;
        /// ]]></code>
        /// </example>
        public override void Append(StringBuilder buffer, AstNode node, int stack, Action<StringBuilder, IEnumerable<AstNode>, int> traverseFunc)
        {
            bool appendedContracts = false;
            const string namespaceOfContracts = "System.Diagnostics.Contracts";

            AstNode item = node;
            while(item != null)
            {
                if (item is UsingDeclaration)
                {
                    var usingD = item as UsingDeclaration;

                    if (!appendedContracts)
                    {
                        var result = namespaceOfContracts.CompareTo(usingD.Namespace);
                        if (result == 0)
                        {
                            // already exists.
                            appendedContracts = true;
                        }
                        else if (result < 1 || !usingD.Namespace.StartsWith("System"))
                        {
                            // appends contracts namespace.
                            appendedContracts = true;
                            buffer.AppendLine(string.Format("using {0};", namespaceOfContracts), stack);
                        }
                    }

                    buffer.AppendLine(string.Format("using {0};", usingD.Namespace), stack);
                }

                item = item.NextSibling;
            }

            if (!appendedContracts)
            {
                buffer.AppendLine(string.Format("using {0};", namespaceOfContracts), stack);
            }

            buffer.AppendEmptyLine();
        }
    }
}
