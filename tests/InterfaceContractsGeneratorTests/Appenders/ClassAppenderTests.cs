using System;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterfaceContractsGenerator.Appenders;

namespace InterfaceContractsGeneratorTests.Appenders
{
    [TestClass]
    public class ClassAppendersTests
    {
        private readonly CSharpParser parser = new CSharpParser(new CompilerSettings { WarningLevel = 0 });

        [TestMethod]
        public void TestGetContractsClassName()
        {
            // Arrange & Act
            var actual = ClassAppender.CreateContractsClassName("ITest");

            // Assert
            Assert.AreEqual("TestContracts", actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetContractsClassNameWithNull()
        {
            ClassAppender.CreateContractsClassName(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetContractsClassNameWithShortName()
        {
            ClassAppender.CreateContractsClassName("T");
        }

        [TestMethod]
        public void TestAppend()
        {
            // Arrange
            const string code = "public interface ITest {}";
            var node = parser.Parse(code);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("[ContractClassFor(typeof(ITest))]");
            expected.AppendLine("internal abstract class TestContracts : ITest");
            expected.AppendLine("{");
            expected.AppendLine("}");

            // Act
            new ClassAppender().Append(actual, node.FirstChild, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAppendWithNotInterface()
        {
            // Arrange
            const string code = "public class Test {}";
            var node = parser.Parse(code);
            var actual = new StringBuilder();

            // Act
            new ClassAppender().Append(actual, node.FirstChild, 0, (b, d, s) => { });
        }

        [TestMethod]
        public void TestAppendWithExtendingAInterface()
        {
            // Arrange
            const string code = "public interface ITest : IOne {}";
            var node = parser.Parse(code);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("[ContractClassFor(typeof(ITest))]");
            expected.AppendLine("internal abstract class TestContracts : OneContracts, ITest");
            expected.AppendLine("{");
            expected.AppendLine("}");

            // Act
            new ClassAppender().Append(actual, node.FirstChild, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithExtendingInterfaces()
        {
            // Arrange
            const string code = "public interface ITest : IOne, ITwo {}";
            var node = parser.Parse(code);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("[ContractClassFor(typeof(ITest))]");
            expected.AppendLine("internal abstract class TestContracts : OneContracts, TwoContracts, ITest");
            expected.AppendLine("{");
            expected.AppendLine("}");

            // Act
            new ClassAppender().Append(actual, node.FirstChild, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithTypeParameter()
        {
            // Arrange
            const string code = "public interface ITest<T> {}";
            var node = parser.Parse(code);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("[ContractClassFor(typeof(ITest))]");
            expected.AppendLine("internal abstract class TestContracts<T> : ITest");
            expected.AppendLine("{");
            expected.AppendLine("}");

            // Act
            new ClassAppender().Append(actual, node.FirstChild, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void TestAppendWithTypeParameterAndConstraints()
        {
            // Arrange
            const string code = "public interface ITest<T> where T : new(), class {}";
            var node = parser.Parse(code);
            var actual = new StringBuilder();

            var expected = new StringBuilder();
            expected.AppendLine("[ContractClassFor(typeof(ITest))]");
            expected.AppendLine("internal abstract class TestContracts<T> : ITest where T : new(), class");
            expected.AppendLine("{");
            expected.AppendLine("}");

            // Act
            new ClassAppender().Append(actual, node.FirstChild, 0, (b, d, s) => { });

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
