using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator.Extensions;

namespace InterfaceContractsGenerator.Appenders
{
    /// <summary>
    /// MethodAppender class
    /// </summary>
    public class MethodAppender : AppenderBase
    {
        /// <summary>
        /// Appends a method property into output buffer.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="node">the node</param>
        /// <param name="stack">the stack</param>
        /// <param name="traverseFunc">the tracerse function</param>
        /// <example>
        /// Input
        /// <code><![CDATA[
        ///     T Get<T, V>(string key) where T : class, new() where V : class, new();
        /// ]]></code>
        /// 
        /// Output
        /// <code><![CDATA[
        ///     public T Get<T, V>(string key) where T : class, new() where V : class, new()
        ///     {
        ///         Contract.Requires<System.ArgumentNullException>(key != null, "'key' must not be null.");
        ///         Contract.Ensures(Contract.Result<T>() != null, "The return value must not be null.");
        ///         return default(T);
        ///     }
        /// ]]></code>
        /// </example>
        public override void Append(StringBuilder buffer, AstNode node, int stack, Action<StringBuilder, IEnumerable<AstNode>, int> traverseFunc)
        {
            var declaration = node as MethodDeclaration;

            AppendDocument(buffer, declaration, stack);
            AppendAttributes(buffer, declaration, stack);
            
            // T Create<T>(ref string name = null) where T : new()
            buffer.AppendLine(string.Format("public {0} {1}{2}{3}{4}",
                    declaration.ReturnType.ToString(), // "T"
                    declaration.Name, // "Create"
                    declaration.TypeParameters.Count > 0 ? string.Format("<{0}>", string.Join(", ", declaration.TypeParameters.Select(t => t.Name))) : string.Empty, // "<T>"
                    string.Format("({0})", string.Join(", ", declaration.Parameters.Select(t =>
                        string.Format("{0}{1} {2}{3}", 
                            t.ParameterModifier != ParameterModifier.None ? t.ParameterModifier.ToString().ToLower() + " " : null, // "ref", "out", "params"
                            t.Type, // "string"
                            t.NameToken.ToString(), // "name"
                            !t.DefaultExpression.IsNull ? " = " + t.DefaultExpression : null)))), // "(ref string name = null)"
                    declaration.Constraints.Count > 0 ? string.Format("{0}", string.Join("", declaration.Constraints.Select(t => t.ToString()))) : string.Empty),
                stack);

            buffer.AppendLine("{", stack);

            // parameters
            foreach (var parameter in declaration.Parameters)
            {
                var name = parameter.NameToken.ToString();

                if (parameter.ParameterModifier != ParameterModifier.None)
                {
                    if (parameter.ParameterModifier == ParameterModifier.Params)
                    {
                        continue;
                    }

                    if (parameter.ParameterModifier == ParameterModifier.Ref
                        || parameter.ParameterModifier == ParameterModifier.Out)
                    {
                        buffer.AppendLine(string.Format("{0} = default({1});", name, parameter.Type), stack + 1);
                        if (!Generator.IsSkipType(parameter.Type))
                        {
                            buffer.AppendLine(string.Format("Contract.Ensures({0} != null, \"The {1} parameter '{0}' must not be null.\");", name, parameter.ParameterModifier.ToString().ToLower()), stack + 1);
                        }

                        continue;
                    }
                }

                if (Generator.IsSkipType(parameter.Type))
                {
                    continue;
                }

                var isDefaultExpressionNull = !parameter.DefaultExpression.IsNull && parameter.DefaultExpression.ToString() == "null";
                if (isDefaultExpressionNull)
                {
                    continue;
                }

                buffer.AppendLine(string.Format("Contract.Requires<System.ArgumentNullException>({0} != null, \"'{0}' must not be null.\");", name), stack + 1);
            }

            // return
            var returnTypeName = declaration.ReturnType.ToString();
            if (returnTypeName != "void")
            {
                if (!Generator.IsSkipType(declaration.ReturnType))
                {
                    buffer.AppendLine(string.Format("Contract.Ensures(Contract.Result<{0}>() != null, \"The return value must not be null.\");", returnTypeName), stack + 1);
                }

                buffer.AppendLine(string.Format("return default({0});", returnTypeName), stack + 1);
            }

            buffer.AppendLine("}", stack);
        }
    }
}
