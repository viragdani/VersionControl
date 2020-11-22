using Moq;
using NUnit.Framework;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Abstractions;
using UnitTestExample.Controllers;
using UnitTestExample.Entities;

namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {
        
        [Test,TestCase("asd1234", false), TestCase("valami@gmail",false), TestCase("valamigmail.com", false), TestCase("valami@gmail.com",true)]
        public void TestValidateEmail(string email, bool ExpectedResult)
        {
            //Arrange
            var accountController = new AccountController();

            //Act
            var actualresult = accountController.ValidateEmail(email);

            //Assert
            Assert.AreEqual(ExpectedResult, actualresult);
        }
        [Test, TestCase("asdASDasd",false), TestCase("ASDASDASD123", false), TestCase("asdasdasd123",false), TestCase("a1A",false), TestCase("ASDasdASD123",true)]
        public void testValidatePassword(string password, bool expectedResult)
        {
            //Arrange
            var accountController = new AccountController();

            //Act
            var actualresult = accountController.ValidatePassword(password);

            //Assert
            Assert.AreEqual(expectedResult, actualresult);
        }
        [Test]
        public void TestRegisterHappyPath(string email, string password)
        {
            // Arrange
            var accountServiceMock = new Mock<IAccountManager>(MockBehavior.Strict);
            accountServiceMock
                .Setup(m => m.CreateAccount(It.IsAny<Account>()))
                .Returns<Account>(a => a);
            var accountController = new AccountController();
            accountController.AccountManager = accountServiceMock.Object;

            // Act
            var actualResult = accountController.Register(email, password);

            // Assert
            Assert.AreEqual(email, actualResult.Email);
            Assert.AreEqual(password, actualResult.Password);
            Assert.AreNotEqual(Guid.Empty, actualResult.ID);
            accountServiceMock.Verify(m => m.CreateAccount(actualResult), Times.Once);
        }
        [
    Test,
    TestCase("irf@uni-corvinus", "Abcd1234"),
    TestCase("irf.uni-corvinus.hu", "Abcd1234"),
    TestCase("irf@uni-corvinus.hu", "abcd1234"),
    TestCase("irf@uni-corvinus.hu", "ABCD1234"),
    TestCase("irf@uni-corvinus.hu", "abcdABCD"),
    TestCase("irf@uni-corvinus.hu", "Ab1234"),
]
        public void TestRegisterValidateException(string email, string password)
        {
            //Arrange
            try
            {
                var accountController = new AccountController();
                Assert.Fail();
            }
            catch (Exception ex)
            {

                Assert.IsInstanceOf<ValidationException>(ex);
            }
              
        }
    }
}
