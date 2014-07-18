using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterfaceContractsGenerator.Appenders;

namespace InterfaceContractsGeneratorTests.Appenders
{
    [TestClass]
    public class UsingAppendersTests
    {
        private readonly CSharpParser parser = new CSharpParser(new CompilerSettings { WarningLevel = 0 });

        [TestMethod]
        public void TestAppendWithSystem()
        {
            // Arrange
            var expected = new StringBuilder();
            expected.AppendLine("using System;");
            expected.AppendLine("using System.Diagnostics.Contracts;");
            expected.AppendLine();

            const string testCode = "using System;";
            var node = parser.Parse(testCode);
            var actual = new StringBuilder();
            
            // Act
            new UsingAppender().Append(actual, node.FirstChild, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithSystemCollection()
        {
            // Arrange
            var expected = new StringBuilder();
            expected.AppendLine("using System.Collection;");
            expected.AppendLine("using System.Diagnostics.Contracts;");
            expected.AppendLine();

            const string testCode = "using System.Collection;";
            var node = parser.Parse(testCode);
            var actual = new StringBuilder();

            // Act
            new UsingAppender().Append(actual, node.Children.First(), 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithSystemDiagnosticsContracts()
        {
            // Arrange
            var expected = new StringBuilder();
            expected.AppendLine("using System.Diagnostics.Contracts;");
            expected.AppendLine();

            const string testCode = "using System.Diagnostics.Contracts;";
            var node = parser.Parse(testCode);
            var actual = new StringBuilder();

            // Act
            new UsingAppender().Append(actual, node.Children.First(), 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithSystemLinq()
        {
            // Arrange
            var expected = new StringBuilder();
            expected.AppendLine("using System.Diagnostics.Contracts;");
            expected.AppendLine("using System.Linq;");
            expected.AppendLine();

            const string testCode = "using System.Linq;";
            var node = parser.Parse(testCode);
            var actual = new StringBuilder();

            // Act
            new UsingAppender().Append(actual, node.Children.First(), 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithSystemAndSystemLinq()
        {
            // Arrange
            var expected = new StringBuilder();
            expected.AppendLine("using System;");
            expected.AppendLine("using System.Diagnostics.Contracts;");
            expected.AppendLine("using System.Linq;");
            expected.AppendLine();

            const string testCode = @"using System;
                                      using System.Linq;";
            var node = parser.Parse(testCode);
            var actual = new StringBuilder();

            // Act
            new UsingAppender().Append(actual, node.Children.First(), 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithEmptyLine()
        {
            // Arrange
            var expected = new StringBuilder();
            expected.AppendLine("using System;");
            expected.AppendLine("using System.Diagnostics.Contracts;");
            expected.AppendLine("using System.Linq;");
            expected.AppendLine();

            const string testCode = "using System;\n" +
                                     "\n" +
                                     "using System.Linq;";
            
            var node = parser.Parse(testCode);
            var actual = new StringBuilder();

            // Act
            new UsingAppender().Append(actual, node.Children.First(), 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithNotSystem()
        {
            // Arrange
            var expected = new StringBuilder();
            expected.AppendLine("using System.Diagnostics.Contracts;");
            expected.AppendLine("using AAA;");
            expected.AppendLine();

            const string testCode = "using AAA;";

            var node = parser.Parse(testCode);
            var actual = new StringBuilder();

            // Act
            new UsingAppender().Append(actual, node.FirstChild, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithNull()
        {
            // Arrange
            var expected = new StringBuilder();
            expected.AppendLine("using System.Diagnostics.Contracts;");
            expected.AppendLine();

            var actual = new StringBuilder();
            
            // Act
            new UsingAppender().Append(actual, null, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
