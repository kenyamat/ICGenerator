using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator.Appenders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterfaceContractsGeneratorTests.Appenders
{
    [TestClass]
    public class PropertyAppendersTests
    {
        private readonly CSharpParser parser = new CSharpParser(new CompilerSettings { WarningLevel = 0 });

        [TestMethod]
        public void TestAppend()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                     string Name { get; set; }
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is PropertyDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string Name");
            expected.AppendLine("{");
            expected.AppendLine("    get { return default(string); }");
            expected.AppendLine("    set {}");
            expected.AppendLine("}");

            // Act
            new PropertyAppender().Append(actual, node, 0, (b, d, s) => { });

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
                     string Name { set; }
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is PropertyDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string Name");
            expected.AppendLine("{");
            expected.AppendLine("    set {}");
            expected.AppendLine("}");

            // Act
            new PropertyAppender().Append(actual, node, 0, (b, d, s) => { });

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
                     string Name { get; };
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is PropertyDeclaration);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("public string Name");
            expected.AppendLine("{");
            expected.AppendLine("    get { return default(string); }");
            expected.AppendLine("}");

            // Act
            new PropertyAppender().Append(actual, node, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
