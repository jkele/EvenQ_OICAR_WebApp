using Eveq_Oicar_web.Controllers;
using Eveq_Oicar_web.Models;
using NUnit.Framework;


namespace UnitTest
{
    class RegisterTest
    {
        private HomeController home;
        private Register register;


        [SetUp]
        public void Setup()
        {
            home = new HomeController();
            register = new Register()
            {
                FirstName = "test",
                LastName = "test",
                Age = 18,
                Password = "PASS",
                ConfirmPassword = "PASS",
                Email = "PASS",
                ReferralCode = "test"

            };

        }

        [Test]
        public void RegisterFieldsFilled()
        {
            var result = home.Register(register);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void RegisterPasswordNotFilled()
        {
            register.Password = null;
            var result = home.Register(register);
            Assert.That(result, Is.Not.Null);
        }


        [Test]
        public void RegisterFirstNameNotFilled()
        {
            register.FirstName = null;
            var result = home.Register(register);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void RegisterLastNameNotFilled()
        {
            register.LastName = null;
            var result = home.Register(register);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void RegisterEmailNotFilled()
        {
            register.Email = null;
            var result = home.Register(register);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void RegisterNothingFilled()
        {
            register = null;
            var result = home.Register(register);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void RegisterConfirmPasswordNotFilled()
        {
            register.ConfirmPassword = null;
            var result = home.Register(register);
            Assert.That(result, Is.Not.Null);
        }
    }
}
