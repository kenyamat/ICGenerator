using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator.Extensions;

namespace InterfaceContractsGenerator.Appenders
{
    /// <summary>
    /// AppenderBase class
    /// </summary>
    public abstract class AppenderBase
    {
        /// <summary>
        /// Appends a member into output buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="node">the node</param>
        /// <param name="stack">the stack</param>
        /// <param name="traverseFunc">the tracerse function</param>
        public abstract void Append(StringBuilder buffer, AstNode node, int stack, Action<StringBuilder, IEnumerable<AstNode>, int> traverseFunc);

        /// <summary>
        /// Appends a property document.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="node">the node</param>
        /// <param name="stack">the stack</param>
        /// <example>
        /// <code>
        ///     /// <summery>
        ///     /// Do Something
        ///     /// </summery>
        ///     int DoSomething();
        /// </code>
        /// </example>
        public static void AppendDocument(StringBuilder buffer, AstNode node, int stack)
        {
            foreach (var child in node.Children)
            {
                if (child is Comment)
                {
                    var comment = child as Comment;
                    buffer.AppendLine(comment.ToString().Trim(), stack);
                }
                else if (child is NewLineNode)
                {
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Appends node attributes.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="node">the node</param>
        /// <param name="stack">the stack</param>
        /// <example>
        /// <code>
        ///     [XmlIgnore]
        ///     [ConditionalAttribute("CODE_ANALYSIS")]
        ///     int DoSomething();
        /// </code>
        /// </example>
        public static void AppendAttributes(StringBuilder buffer, EntityDeclaration node, int stack)
        {
            foreach (var attribute in node.Attributes)
            {
                buffer.AppendLine(attribute.ToString().Trim(), stack);
            }
        }
    }
}
