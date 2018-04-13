using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Relativity.API;
using Relativity.Test.Helpers;
using System;

namespace CreateObjectAfterFieldParsing.Infrastructure
{
    class TestEnvironment : IDisposable
    {
        public static TestEnvironment Instance => _instance.Value;
        private static Lazy<TestEnvironment> _instance = new Lazy<TestEnvironment>(() => new TestEnvironment());

        /// <summary>
        /// In charge of interface interaction
        /// </summary>
        public IWebDriver Driver { get; }
        /// <summary>
        /// In charge of API-BackEnd interaction
        /// </summary>
        public TestHelper BackendHelper { get; }
        /// <summary>
        /// Wrapper for GetServicesManager
        /// </summary>
        public IServicesMgr ServicesManager => BackendHelper.GetServicesManager();

        private TestEnvironment()
        {
            BackendHelper = new TestHelper();
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected internal void Dispose(bool disposing)
        {
            Driver.Close();
            Driver.Dispose();
        }

        ~TestEnvironment()
        {
            Dispose(false);
        }
    }
}
