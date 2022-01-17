using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;

namespace BookWebIntegrationTest
{
    public class Tests
    {
    //    [SetUp]
    //    public void Setup()
    //    {
           
    //    }

        [Test]
        public void Test1()
        {
            //browser driver
            IWebDriver webDriver = new ChromeDriver();
            //navigate to site
            webDriver.Navigate().GoToUrl("https://localhost:44317");

            //itentify log in
            IWebElement loginBtn = webDriver.FindElement(By.LinkText("Log in"));

            //operation
            loginBtn.Click();

            //username
            var loginBox = webDriver.FindElement(By.Id("login-box"));

            //assertion
            Assert.That(loginBox.Displayed, Is.True);

            var emailInputLogin = webDriver.FindElement(By.Name("Input.Email"));
            var passwordInputLogin = webDriver.FindElement(By.Name("Input.Password"));
            var loginSubmitBtn = webDriver.FindElement(By.Id("login-submit-btn"));
            emailInputLogin.SendKeys("test@test.com");
            passwordInputLogin.SendKeys("Test123!");
            loginSubmitBtn.Click();

            
           
        }
    }
}