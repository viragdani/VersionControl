using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Controllers;

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
            //Arrange
            var accountController = new AccountController();

            //Act
            var actualResult = accountController.Register(email, password);

            //Assert
            Assert.AreEqual(email, actualResult.Email);
            Assert.AreEqual(password, actualResult.Password);
            Assert.AreNotEqual(Guid.Empty, actualResult.ID);
        }
    }
}
