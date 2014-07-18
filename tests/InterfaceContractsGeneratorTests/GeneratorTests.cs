using System;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using InterfaceContractsGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterfaceContractsGeneratorTests
{
    [TestClass]
    public class GeneratorTests
    {
        private readonly CSharpParser parser = new CSharpParser(new CompilerSettings { WarningLevel = 0 });

        [TestMethod]
        public void TestGenerate()
        {
            // Arrange
            const string code =
                @"public interface ITest
                 {
                 }";

            var expected = new StringBuilder();
            expected.AppendLine("using System.Diagnostics.Contracts;");
            expected.AppendLine("");
            expected.AppendLine("[ContractClassFor(typeof(ITest))]");
            expected.AppendLine("internal abstract class TestContracts : ITest");
            expected.AppendLine("{");
            expected.Append("}");

            // Act
            var actual = Generator.Generate(code);

            // Assert
            Assert.AreEqual(expected.ToString(), actual);
        }

        [TestMethod]
        public void TestGenerateWithNoInterface()
        {
            // Arrange
            const string code =
                @"public class Test
                 {
                 }";

            var expected = new StringBuilder();

            // Act
            var actual = Generator.Generate(code);

            // Assert
            Assert.AreEqual(expected.ToString(), actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGenerateWithNullCode()
        {
            Generator.Generate(null);
        }

        [TestMethod]
        public void TestGetContractsFilePath()
        {
            // Arrange
            const string basePath = @"C:\Repository\Library";
            const string filePath = @"C:\Repository\Library\Shared\ITest.cs";
            const string outputDirPath = @"C:\output";
            const string expected = @"C:\output\Shared\Contracts\TestContracts.cs";

            // Act
            var actual = Generator.GetContractsFilePath(basePath, filePath, outputDirPath);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestIsSkipType()
        {
            // Arrange
            var property = new ParameterDeclaration("name")
            {
                Type = new PrimitiveType("string")
            };

            // Act
            var actual = Generator.IsSkipType(property.Type);

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void TestIsSkipTypeWithIncludingType()
        {
            // Arrange
            var property = new ParameterDeclaration("name")
            {
                Type = new PrimitiveType("string")
            };
            
            // Act
            Generator.SkipTypes = new[] { "string" };
            var actual = Generator.IsSkipType(property.Type);
            Generator.SkipTypes = null;

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestIsSkipTypeWithHavingChild()
        {
            // Arrange
            var property = new ParameterDeclaration("pair")
            {
                Type = new SimpleType("KeyValuePair<K, V>")
            };

            // Act
            Generator.SkipTypes = new[] { "KeyValuePair" };
            var actual = Generator.IsSkipType(property.Type);
            Generator.SkipTypes = null;
            
            // Assert
            Assert.IsFalse(actual);
        }
    }
}
