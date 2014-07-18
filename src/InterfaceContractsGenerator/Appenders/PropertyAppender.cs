using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator.Extensions;

namespace InterfaceContractsGenerator.Appenders
{
    /// <summary>
    /// PropertyAppender class
    /// </summary>
    public class PropertyAppender : AppenderBase
    {
        /// <summary>
        /// Appends a property into output buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="node">the node</param>
        /// <param name="stack">the stack</param>
        /// <param name="traverseFunc">the tracerse function</param>
        /// <example>
        /// Input
        /// <code><![CDATA[
        ///     string Name { get; set; }
        /// ]]></code>
        /// 
        /// Output
        /// <code><![CDATA[
        ///     public string Name
        ///     {
        ///         get { return default(string); }
        ///         set {}
        ///     }
        /// ]]></code>
        /// </example>
        public override void Append(StringBuilder buffer, AstNode node, int stack, Action<StringBuilder, IEnumerable<AstNode>, int> traverseFunc)
        {
            var declaration = node as PropertyDeclaration;

            var type = declaration.ReturnType.ToString();
            var name = declaration.Name;

            AppendDocument(buffer, declaration, stack);
            AppendAttributes(buffer, declaration, stack);
            buffer.AppendLine(string.Format("public {0} {1}",
                    type,
                    name),
                stack);

            buffer.AppendLine("{", stack);
            if (!declaration.Getter.IsNull)
            {
                buffer.AppendLine(string.Format("get {{ return default({0}); }}", declaration.ReturnType.ToString()), stack + 1);
            }

            if (!declaration.Setter.IsNull)
            {
                buffer.AppendLine("set {}", stack + 1);
            }

            buffer.AppendLine("}", stack);
        }
    }
}
