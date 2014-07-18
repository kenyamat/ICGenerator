using System.Text;
using ICSharpCode.NRefactory.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterfaceContractsGenerator.Appenders;

namespace InterfaceContractsGeneratorTests.Appenders
{
    [TestClass]
    public class NamespaceAppendersTests
    {
        private readonly CSharpParser parser = new CSharpParser(new CompilerSettings { WarningLevel = 0 });

        [TestMethod]
        public void TestAppend()
        {
            // Arrange
            const string code = "namespace Test {}";
            var node = parser.Parse(code);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("namespace Test.Contracts");
            expected.AppendLine("{");
            expected.AppendLine("}");

            // Act
            new NamespaceAppender().Append(actual, node.FirstChild, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
