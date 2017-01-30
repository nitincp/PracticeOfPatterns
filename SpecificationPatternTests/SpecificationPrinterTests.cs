using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecificationPattern;

namespace SpecificationPatternTests
{
    [TestClass]
    public class SpecificationPrinterTests
    {
        public class UserInfo
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        [TestMethod]
        public void TestAndSpec()
        {
            // Arrange
            var spec = new AndSpecification<UserInfo>(new ValueSpecification<UserInfo>("abcd"), new ValueSpecification<UserInfo>("abcd"));
            var specPrinter = new SpecificationPrinter<UserInfo>();
            // Act
            var condition = specPrinter.Visit(spec);

            // Assert
            Assert.IsTrue(condition != string.Empty);
        }

        [TestMethod]
        public void TestComplexSpec()
        {
            // Arrange
            var spec = new AndSpecification<UserInfo>(
                new AndSpecification<UserInfo>(
                    new AndSpecification<UserInfo>(
                        new ValueSpecification<UserInfo>("1"),
                        new ValueSpecification<UserInfo>("2")),
                    new AndSpecification<UserInfo>(
                        new ValueSpecification<UserInfo>("3"),
                        new ValueSpecification<UserInfo>("4"))),
                new ValueSpecification<UserInfo>("5"));
            var specPrinter = new SpecificationPrinter<UserInfo>();
            // Act
            var condition = specPrinter.Visit(spec);

            // Assert
            Assert.IsTrue(condition != string.Empty);
        }
    }
}
