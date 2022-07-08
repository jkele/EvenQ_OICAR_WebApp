using Eveq_Oicar_web.Controllers;
using Eveq_Oicar_web.Models;
using NUnit.Framework;

namespace UnitTest
{
    public class Tests
    {
        private HomeController home;
        private Login login;


        [SetUp]
        public void Setup()
        {
            home = new HomeController();
            login = new Login()
            {
                Email = "TestEmail",
                Password = "Password"
            };

        }

        [Test]
        public void LoginFieldsFilled()
        {
            var result = home.SignIn(login);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void LoginEmailFilled()
        {
            login.Password = null;
            var result = home.SignIn(login);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void LoginPasswordFilled()
        {
            login.Email = null;
            var result = home.SignIn(login);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void LoginNothingFilled()
        {
            login.Email = null;
            login.Password = null;
            var result = home.SignIn(null);
            Assert.That(result, Is.Not.Null);
        }
    }
}