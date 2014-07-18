using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator.Extensions;

namespace InterfaceContractsGenerator.Appenders
{
    /// <summary>
    /// IndexerAppender class.
    /// </summary>
    public class IndexerAppender : AppenderBase
    {
        /// <summary>
        /// Appends a indexer property into output buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="node">the node</param>
        /// <param name="stack">the stack</param>
        /// <param name="traverseFunc">the tracerse function</param>
        /// <example>
        /// Input
        /// <code><![CDATA[
        ///     object this[string name] { get; set; }
        /// ]]></code>
        /// 
        /// Output
        /// <code><![CDATA[
        ///     public object this[string name]
        ///     {
        ///         get
        ///         {
        ///             Contract.Requires<System.ArgumentNullException>(name != null, "'name' must not be null.");
        ///             return default(object);
        ///         }
        ///         set
        ///         {
        ///             Contract.Requires<System.ArgumentNullException>(name != null, "'name' must not be null.");
        ///             Contract.Requires<System.ArgumentNullException>(value != null, "'value' must not be null.");
        ///         }
        ///     }
        /// ]]></code>
        /// </example>
        public override void Append(StringBuilder buffer, AstNode node, int stack, Action<StringBuilder, IEnumerable<AstNode>, int> traverseFunc)
        {
            var declaration = node as IndexerDeclaration;

            AppendDocument(buffer, declaration, stack);
            AppendAttributes(buffer, declaration, stack);

            var parameter = declaration.Parameters.First();
            var type = parameter.Type.ToString();
            var name = parameter.NameToken.ToString();
            buffer.AppendLine(string.Format("public {0} this{1}",
                    declaration.ReturnType.ToString(),
                    string.Format("[{0} {1}]", type, name)),
                stack);

            buffer.AppendLine("{", stack);
            if (!declaration.Getter.IsNull && !Generator.IsSkipType(parameter.Type))
            {
                buffer.AppendLine("get", stack + 1);
                buffer.AppendLine("{", stack + 1);
                buffer.AppendLine(string.Format("Contract.Requires<System.ArgumentNullException>({0} != null, \"'{0}' must not be null.\");", name), stack + 2);
                buffer.AppendLine(string.Format("return default({0});", declaration.ReturnType.ToString()), stack + 2);
                buffer.AppendLine("}", stack + 1);
            }

            if (!declaration.Setter.IsNull && !Generator.IsSkipType(declaration.ReturnType))
            {
                buffer.AppendLine("set", stack + 1);
                buffer.AppendLine("{", stack + 1);
                buffer.AppendLine(string.Format("Contract.Requires<System.ArgumentNullException>({0} != null, \"'{0}' must not be null.\");", name), stack + 2);
                buffer.AppendLine("Contract.Requires<System.ArgumentNullException>(value != null, \"'value' must not be null.\");", stack + 2);
                buffer.AppendLine("}", stack + 1);
            }

            buffer.AppendLine("}", stack);
        }
    }
}
