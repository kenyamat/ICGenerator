using System.IO;
using InterfaceContractsGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterfaceContractsGeneratorTests.Integration
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void Test1WithNormal()
        {
            // Arrange
            var input = File.ReadAllText(@"Integration\TestData\Test1InputCode.txt");
            var expected = File.ReadAllText(@"Integration\TestData\Test1OutputCode.txt");

            // Act
            var actual = Generator.Generate(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test2WithInnerUsings()
        {
            // Arrange
            var input = File.ReadAllText(@"Integration\TestData\Test2InputCode.txt");
            var expected = File.ReadAllText(@"Integration\TestData\Test2OutputCode.txt");

            // Act
            var actual = Generator.Generate(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test3WithNoUsings()
        {
            // Arrange
            var input = File.ReadAllText(@"Integration\TestData\Test3InputCode.txt");
            var expected = File.ReadAllText(@"Integration\TestData\Test3OutputCode.txt");

            // Act
            var actual = Generator.Generate(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test4WithIndexerInSecond()
        {
            // Arrange
            var input = File.ReadAllText(@"Integration\TestData\Test4InputCode.txt");
            var expected = File.ReadAllText(@"Integration\TestData\Test4OutputCode.txt");

            // Act
            var actual = Generator.Generate(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

    }
}
