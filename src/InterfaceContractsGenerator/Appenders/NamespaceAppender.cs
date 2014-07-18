using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator.Extensions;

namespace InterfaceContractsGenerator.Appenders
{
    /// <summary>
    /// NamespaceAppender class
    /// </summary>
    public class NamespaceAppender : AppenderBase
    {
        /// <summary>
        /// Appends a namespace property into output buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="node">the node</param>
        /// <param name="stack">the stack</param>
        /// <param name="traverseFunc">the tracerse function</param>
        /// <example>
        /// Input
        /// <code><![CDATA[
        ///     namespace Cloud.Shared
        /// ]]></code>
        /// 
        /// Output
        /// <code><![CDATA[
        ///     namespace Cloud.Shared.Contracts
        /// ]]></code>
        /// </example>
        public override void Append(StringBuilder buffer, AstNode node, int stack, Action<StringBuilder, IEnumerable<AstNode>, int> traverseFunc)
        {
            var declaration = node as NamespaceDeclaration;
            buffer.AppendLine(string.Format("namespace {0}.Contracts", declaration.Name), stack);
            buffer.AppendLine("{", stack);
            traverseFunc(buffer, declaration.Members, stack + 1);
            buffer.AppendLine("}", stack);
        }
    }
}
