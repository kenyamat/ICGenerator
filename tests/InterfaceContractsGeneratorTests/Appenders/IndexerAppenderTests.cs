using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterfaceContractsGenerator.Appenders;

namespace InterfaceContractsGeneratorTests.Appenders
{
    [TestClass]
    public class IndexerAppendersTests
    {
        private readonly CSharpParser parser = new CSharpParser(new CompilerSettings { WarningLevel = 0 });

        [TestMethod]
        public void TestAppend()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string this[string i] { get; set; }
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is IndexerDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string this[string i]");
            expected.AppendLine("{");
            expected.AppendLine("    get");
            expected.AppendLine("    {");
            expected.AppendLine("        Contract.Requires<System.ArgumentNullException>(i != null, \"'i' must not be null.\");");
            expected.AppendLine("        return default(string);");
            expected.AppendLine("    }");
            expected.AppendLine("    set");
            expected.AppendLine("    {");
            expected.AppendLine("        Contract.Requires<System.ArgumentNullException>(i != null, \"'i' must not be null.\");");
            expected.AppendLine("        Contract.Requires<System.ArgumentNullException>(value != null, \"'value' must not be null.\");");
            expected.AppendLine("    }");
            expected.AppendLine("}");

            // Act
            new IndexerAppender().Append(actual, node, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithNoSetter()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string this[string i] { get; }
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is IndexerDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string this[string i]");
            expected.AppendLine("{");
            expected.AppendLine("    get");
            expected.AppendLine("    {");
            expected.AppendLine("        Contract.Requires<System.ArgumentNullException>(i != null, \"'i' must not be null.\");");
            expected.AppendLine("        return default(string);");
            expected.AppendLine("    }");
            expected.AppendLine("}");

            // Act
            new IndexerAppender().Append(actual, node, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithNoGetter()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string this[string i] { set; }
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is IndexerDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string this[string i]");
            expected.AppendLine("{");
            expected.AppendLine("    set");
            expected.AppendLine("    {");
            expected.AppendLine("        Contract.Requires<System.ArgumentNullException>(i != null, \"'i' must not be null.\");");
            expected.AppendLine("        Contract.Requires<System.ArgumentNullException>(value != null, \"'value' must not be null.\");");
            expected.AppendLine("    }");
            expected.AppendLine("}");

            // Act
            new IndexerAppender().Append(actual, node, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithSkipType()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string this[string i] { get; set; }
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is IndexerDeclaration);
            var actual = new StringBuilder();


            var expected = new StringBuilder();
            expected.AppendLine("public string this[string i]");
            expected.AppendLine("{");
            expected.AppendLine("}");

            // Act
            Generator.SkipTypes = new[] { "string" };
            new IndexerAppender().Append(actual, node, 0, (b, d, s) => { });
            Generator.SkipTypes = null;

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
