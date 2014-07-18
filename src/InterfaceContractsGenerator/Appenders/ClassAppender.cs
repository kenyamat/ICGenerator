using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator.Extensions;

namespace InterfaceContractsGenerator.Appenders
{
    /// <summary>
    /// ClassAppender class
    /// </summary>
    public class ClassAppender : AppenderBase
    {
        /// <summary>
        /// Creates a contracts class name from the given interface name.
        /// </summary>
        /// <param name="name">interface name</param>
        /// <returns>contracts class name</returns>
        /// <example>
        /// <code>
        /// var interfaceName = "ITest";
        /// var contractsName = CreateContractsClassName(interfaceName); // TestContracts
        /// </code>
        /// </example>
        public static string CreateContractsClassName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            if (name.Length < 2) { throw new ArgumentException("'name' is too short."); }

            return name.Remove(0, 1) + "Contracts";
        }

        /// <summary>
        /// Appends a class declaration into output buffer.
        /// Need to modify the generated code manually if you use inheritance.
        /// </summary>
        /// <param name="buffer">the buffer</param>
        /// <param name="node">the node</param>
        /// <param name="stack">the stack</param>
        /// <param name="traverseFunc">the tracerse function</param>
        /// <example>
        /// example 1.
        /// <code><![CDATA[
        ///     public interface ITest
        ///  -> internal abstract class TestContracts : ITest
        /// ]]></code>
        /// example 2. (inheritance)
        /// <code><![CDATA[
        ///     public interface ITest : IWork
        ///  -> internal abstract class TestContracts : WorkContratcs, ITest // Need to modify manually
        /// ]]></code>
        /// example1.
        /// <code><![CDATA[
        ///     public interface ITest<T, U> where T : struct where U : new()
        ///  -> internal abstract class TestContracts where T : struct where U : new()
        /// ]]></code>
        /// </example>
        public override void Append(StringBuilder buffer, AstNode node, int stack, Action<StringBuilder, IEnumerable<AstNode>, int> traverseFunc)
        {
            var declaration = node as TypeDeclaration;
            if (declaration.ClassType != ClassType.Interface)
            {
                throw new ArgumentException("It is not interface. [" + declaration.ToString() + "]");
            }

            var baseTypes = declaration.BaseTypes.Select(t => CreateContractsClassName(t.FirstChild.ToString())).ToList();
            baseTypes.Add(declaration.Name);

            AppendDocument(buffer, declaration, stack);
            AppendAttributes(buffer, declaration, stack);
            buffer.AppendLine(string.Format("[ContractClassFor(typeof({0}))]", declaration.Name), stack);
            buffer.AppendLine(string.Format("internal abstract class {0}{1} : {2}{3}",
                    CreateContractsClassName(declaration.Name),
                    declaration.TypeParameters.Count > 0 ? string.Format("<{0}>", string.Join(", ", declaration.TypeParameters.Select(t => t.Name))) : string.Empty,
                    string.Join(", ", baseTypes),
                    declaration.Constraints.Count > 0 ? string.Format("{0}", string.Join("", declaration.Constraints.Select(t => t.ToString()))) : string.Empty),
                stack);
            buffer.AppendLine("{", stack);
            traverseFunc(buffer, declaration.Members, stack + 1);
            buffer.AppendLine("}", stack);
        }
    }
}
