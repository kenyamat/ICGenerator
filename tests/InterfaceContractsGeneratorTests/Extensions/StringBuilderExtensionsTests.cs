using System.Fakes;
using System.Text;
using InterfaceContractsGenerator.Extensions;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterfaceContractsGeneratorTests.Extensions
{
    [TestClass]
    public class StringBuilderExtensionsTests
    {
        [TestMethod]
        public void TestAppendLine()
        {
            // Arrange
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("test");

            // Act
            actual.AppendLine("test", 0);

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendLineWithStack()
        {
            // Arrange
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("    test");

            // Act
            actual.AppendLine("test", 1);

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendLineWithDebug()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var actual = new StringBuilder();
                ShimConsole.WriteString = s => { };
                ShimConsole.WriteLineString = s => { };

                var expected = new StringBuilder();
                expected.AppendLine("test");

                // Act
                actual.AppendLine("test", 0, true);

                // Assert
                Assert.AreEqual(expected.ToString(), actual.ToString());
            }
        }

        [TestMethod]
        public void TestAppendLineWithStackAndDebug()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var actual = new StringBuilder();
                ShimConsole.WriteString = s => { };
                ShimConsole.WriteLineString = s => { };

                var expected = new StringBuilder();
                expected.AppendLine("    test");

                // Act
                actual.AppendLine("test", 1, true);

                // Assert
                Assert.AreEqual(expected.ToString(), actual.ToString());
            }
        }

        [TestMethod]
        public void TestAppendEmptyLine()
        {
            // Arrange
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine();

            // Act
            actual.AppendEmptyLine();

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendEmptyLineWithDebug()
        {
            using (ShimsContext.Create())
            {

                // Arrange
                var actual = new StringBuilder();
                ShimConsole.WriteString = s => { };
                ShimConsole.WriteLineString = s => { };

                var expected = new StringBuilder();
                expected.AppendLine();

                // Act
                actual.AppendEmptyLine(true);

                // Assert
                Assert.AreEqual(expected.ToString(), actual.ToString());
            }
        }
    }
}
