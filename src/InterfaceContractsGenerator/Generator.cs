using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator.Appenders;
using InterfaceContractsGenerator.Extensions;

namespace InterfaceContractsGenerator
{
    /// <summary>
    /// Generates a Interface Contracts code from the Interface code.
    /// </summary>
    public class Generator
    {
        private bool hasUsingDeclarations;
        private bool hasInterface;

        /// <summary>
        /// Gets or sets SkipType.
        /// </summary>
        public static IEnumerable<string> SkipTypes { get; set; }

        /// <summary>
        /// Generates a Interface Contracts code from the Interface code.
        /// </summary>
        /// <param name="interfaceCode">interface source code</param>
        /// <returns>contracts source code</returns>
        public static string Generate(string interfaceCode)
        {
            if (string.IsNullOrWhiteSpace(interfaceCode)) throw new ArgumentNullException("interfaceCode");

            var parser = new CSharpParser(new CompilerSettings { WarningLevel = 0 });
            var syntaxTree = parser.Parse(interfaceCode);
            var buffer = new StringBuilder();

            var generator = new Generator();
            generator.Traverse(buffer, syntaxTree.Members, 0);
            if (!generator.hasInterface)
            {
                buffer.Clear();
            }

            return buffer.ToString().Trim();
        }

        /// <summary>
        /// Gets a contracts file path.
        /// </summary>
        /// <param name="basePath">base path to search interface.</param>
        /// <param name="filePath">file path of interface.</param>
        /// <param name="outputDirPath">contracts output dir path.</param>
        /// <returns>contracts file path</returns>
        /// <example>
        /// <code>
        ///     var basePath = @"C:\Project\Library";
        ///     var filePath = @"C:\Project\Library\Shared\ITest.cs";
        ///     var outputDirPath = @"C:\output";
        ///     var result = GetContractsFilePath(basePath, filePath, outputDirPath); // "C:\output\Shared\Contracts\TestContracts.cs"
        /// </code>
        /// </example>
        public static string GetContractsFilePath(string basePath, string filePath, string outputDirPath)
        {
            var newFileName = ClassAppender.CreateContractsClassName(new FileInfo(filePath).Name.Replace(".cs", string.Empty)) + ".cs";
            var relativePath = filePath.Substring(basePath.Length);
            if (relativePath[0] == Path.DirectorySeparatorChar)
            {
                relativePath = relativePath.Remove(0, 1);
            }

            var tempFile = new FileInfo(Path.Combine(outputDirPath, relativePath));
            var newDir = new DirectoryInfo(Path.Combine(outputDirPath, Path.Combine(tempFile.DirectoryName, "Contracts")));
            return Path.Combine(newDir.FullName, newFileName);
        }


        /// <summary>
        /// Check if the type should be skipped to generate 'Contract.Ensures' and 'Contract.Requires' statemets.
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>true: skip type</returns>
        public static bool IsSkipType(AstType type)
        {
            string typeName = type.HasChildren ? type.FirstChild.ToString() : type.ToString();

            if (SkipTypes == null)
            {
                return false;
            }

            return SkipTypes.Any(s => s.ToUpper() == typeName.ToUpper());
        }

        /// <summary>
        /// Travases code syntax tree.
        /// </summary>
        /// <param name="buffer">output source code buffer</param>
        /// <param name="nodes">syntax tree</param>
        /// <param name="stack">stack number</param>
        public void Traverse(StringBuilder buffer, IEnumerable<AstNode> nodes, int stack)
        {
            bool hasProperty = false;
        
            foreach (var node in nodes)
            {
                if (node is UsingDeclaration && !hasUsingDeclarations)
                {
                    new UsingAppender().Append(buffer, node, stack, Traverse);
                    hasUsingDeclarations = true; 
                }
                else if (node is NamespaceDeclaration)
                {
                    new NamespaceAppender().Append(buffer, node as NamespaceDeclaration, stack, Traverse);
                }
                else if (node is TypeDeclaration)
                {
                    var declaration = node as TypeDeclaration;
                    
                    if (declaration.ClassType == ClassType.Interface)
                    {
                        hasInterface = true;

                        if (!hasUsingDeclarations)
                        {
                            new UsingAppender().Append(buffer, null, stack, Traverse);
                            hasUsingDeclarations = true;
                        }

                        new ClassAppender().Append(buffer, declaration, stack, Traverse);
                    }
                }
                else if (node is IndexerDeclaration)
                {
                    if (hasProperty)
                    {
                        buffer.AppendEmptyLine();
                    }

                    hasProperty = true;
                    new IndexerAppender().Append(buffer, node, stack, Traverse);
                }
                else if (node is PropertyDeclaration)
                {
                    if (hasProperty)
                    {
                        buffer.AppendEmptyLine();
                    }

                    hasProperty = true;
                    new PropertyAppender().Append(buffer, node as PropertyDeclaration, stack, Traverse);
                }
                else if (node is MethodDeclaration)
                {
                    if (hasProperty)
                    {
                        buffer.AppendEmptyLine();
                    }

                    hasProperty = true;
                    new MethodAppender().Append(buffer, node as MethodDeclaration, stack, Traverse);
                }
            }
        }
    }
}