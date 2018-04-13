using CreateObjectAfterFieldParsing.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Relativity.Test.Helpers.SharedTestHelpers;
using System;

namespace SeleniumUnitTest
{
    class PageInitial
    {
        private IWebDriver driver => TestEnvironment.Instance.Driver;

        public PageInitial()
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));
            //PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "_email")]
        public IWebElement UserName { get; set; }

        [FindsBy(How = How.Id, Using = "_password__password_TextBox")]
        public IWebElement Password { get; set; }

        [FindsBy(How = How.Id, Using = "continue")]
        public IWebElement Submit { get; set; }

        [FindsBy(How = How.Id, Using = "_login")]
        public IWebElement Submit1 { get; set; }


        public void GoToPage()
        {
            //driver.Navigate().GoToUrl("https://hongkongtest.relativity.one/Relativity/");
            driver.Navigate().GoToUrl(ConfigurationHelper.RELATIVITY_INSTANCE_ADDRESS);
        }

        public void LoginToApplication()
        {
            UserName.SendKeys(ConfigurationHelper.ADMIN_USERNAME);
            //UserName.SendKeys("adus@cod.com");
            Submit.Submit();
            Password.SendKeys(ConfigurationHelper.DEFAULT_PASSWORD);
            Submit1.Submit();

        }

    }

}


