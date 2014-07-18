using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterfaceContractsGenerator;
using InterfaceContractsGenerator.Appenders;

namespace InterfaceContractsGeneratorTests.Appenders
{
    [TestClass]
    public class MethodAppendersTests
    {
        private readonly CSharpParser parser = new CSharpParser(new CompilerSettings { WarningLevel = 0 });

        [TestMethod]
        public void TestAppend()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string Create(string name);
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is MethodDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string Create(string name)");
            expected.AppendLine("{");
            expected.AppendLine("    Contract.Requires<System.ArgumentNullException>(name != null, \"'name' must not be null.\");");
            expected.AppendLine("    Contract.Ensures(Contract.Result<string>() != null, \"The return value must not be null.\");");
            expected.AppendLine("    return default(string);");
            expected.AppendLine("}");

            // Act
            new MethodAppender().Append(actual, node, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithParameters()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string Create(string name, string value);
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is MethodDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string Create(string name, string value)");
            expected.AppendLine("{");
            expected.AppendLine("    Contract.Requires<System.ArgumentNullException>(name != null, \"'name' must not be null.\");");
            expected.AppendLine("    Contract.Requires<System.ArgumentNullException>(value != null, \"'value' must not be null.\");");
            expected.AppendLine("    Contract.Ensures(Contract.Result<string>() != null, \"The return value must not be null.\");");
            expected.AppendLine("    return default(string);");
            expected.AppendLine("}");

            // Act
            new MethodAppender().Append(actual, node, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithDefaultExpression()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string Create(string name = null);
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is MethodDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string Create(string name = null)");
            expected.AppendLine("{");
            expected.AppendLine("    Contract.Ensures(Contract.Result<string>() != null, \"The return value must not be null.\");");
            expected.AppendLine("    return default(string);");
            expected.AppendLine("}");

            // Act
            new MethodAppender().Append(actual, node, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithRefModifier()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string Create(ref string name);
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is MethodDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string Create(ref string name)");
            expected.AppendLine("{");
            expected.AppendLine("    name = default(string);");
            expected.AppendLine("    Contract.Ensures(name != null, \"The ref parameter 'name' must not be null.\");");
            expected.AppendLine("    Contract.Ensures(Contract.Result<string>() != null, \"The return value must not be null.\");");
            expected.AppendLine("    return default(string);");
            expected.AppendLine("}");

            // Act
            new MethodAppender().Append(actual, node, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithOutModifier()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string Create(out string name);
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is MethodDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string Create(out string name)");
            expected.AppendLine("{");
            expected.AppendLine("    name = default(string);");
            expected.AppendLine("    Contract.Ensures(name != null, \"The out parameter 'name' must not be null.\");");
            expected.AppendLine("    Contract.Ensures(Contract.Result<string>() != null, \"The return value must not be null.\");");
            expected.AppendLine("    return default(string);");
            expected.AppendLine("}");

            // Act
            new MethodAppender().Append(actual, node, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithParamsModifier()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string Create(params string[] names);
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is MethodDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string Create(params string[] names)");
            expected.AppendLine("{");
            //expected.AppendLine("    name = default(string);");
            //expected.AppendLine("    Contract.Ensures(name != null, \"The ref parameter 'name' must not be null.\");");
            expected.AppendLine("    Contract.Ensures(Contract.Result<string>() != null, \"The return value must not be null.\");");
            expected.AppendLine("    return default(string);");
            expected.AppendLine("}");

            // Act
            new MethodAppender().Append(actual, node, 0, (b, d, s) => { });

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
                     string Create(string name);
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is MethodDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string Create(string name)");
            expected.AppendLine("{");
            expected.AppendLine("    return default(string);");
            expected.AppendLine("}");

            // Act
            Generator.SkipTypes = new[] { "string" };
            new MethodAppender().Append(actual, node, 0, (b, d, s) => { });
            Generator.SkipTypes = null;

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithTypeParameter()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string Create<T>(string name) where T : new();
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is MethodDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string Create<T>(string name) where T : new()");
            expected.AppendLine("{");
            expected.AppendLine("    Contract.Requires<System.ArgumentNullException>(name != null, \"'name' must not be null.\");");
            expected.AppendLine("    Contract.Ensures(Contract.Result<string>() != null, \"The return value must not be null.\");");
            expected.AppendLine("    return default(string);");
            expected.AppendLine("}");

            // Act
            new MethodAppender().Append(actual, node, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
