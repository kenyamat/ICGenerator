using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator.Appenders.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterfaceContractsGeneratorTests.Appenders
{
    [TestClass]
    public class AppenderBaseTests
    {
        private readonly CSharpParser parser = new CSharpParser(new CompilerSettings { WarningLevel = 0 });

        [TestMethod]
        public void TestAppendDocument()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                    /// <summary>
                    /// Gets or sets the name.
                    /// </summary>
                    string Name { get; set; }
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is PropertyDeclaration);
            var actual = new StringBuilder();
            
            var expected = new StringBuilder();
            expected.AppendLine("/// <summary>");
            expected.AppendLine("/// Gets or sets the name.");
            expected.AppendLine("/// </summary>");

            // Act
            StubAppenderBase.AppendDocument(actual, node, 0);

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendAttributes()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                    [Attribute1]
                    [Attribute2]
                    string Name { get; set; }
                 }";

            var node = parser.Parse(code).Members.First().Children.First(t => t is PropertyDeclaration) as EntityDeclaration;
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("[Attribute1]");
            expected.AppendLine("[Attribute2]");

            // Act
            StubAppenderBase.AppendAttributes(actual, node, 0);

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
