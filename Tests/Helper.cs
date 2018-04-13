using CreateObjectAfterFieldParsing.Infrastructure;
using kCura.Relativity.Client;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Relativity.Test.Helpers;
using Relativity.Test.Helpers.ImportAPIHelper;
using Relativity.Test.Helpers.SharedTestHelpers;
using Relativity.Test.Helpers.WorkspaceHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace CreateObjectAfterFieldParsing
{
    class Helper
    {
        private TestHelper relativityHelper;
        private IWebDriver driver => TestEnvironment.Instance.Driver;
        int testWorkspaceID;

        public Helper()
        {
            PageFactory.InitElements(driver, this);
            relativityHelper = new TestHelper();
        }

        [FindsBy(How = How.LinkText, Using = "COAFP Test")]
        public IWebElement Button { get; set; }

        [FindsBy(How = How.LinkText, Using = "Case Admin")]
        public IWebElement TabAdmin { get; set; }

        [FindsBy(How = How.LinkText, Using = "Scripts")]
        public IWebElement TabObject { get; set; }

        [FindsBy(How = How.Id, Using = "fil_itemListFUI")]
        public IWebElement TableScripts { get; set; }

        [FindsBy(How = How.Id, Using = "qnOuterContainer")]
        public IWebElement List { get; set; }

        [FindsBy(How = How.Id, Using = "viewMenu")]
        public IWebElement ComboboxTemplate { get; set; }

        [FindsBy(How = How.LinkText, Using = "Run Script")]
        public IWebElement RunForm { get; set; }

        [FindsBy(How = How.Id, Using = "iSearch_dropDownList")]
        public IWebElement ComboboxSaveSearch { get; set; }

        [FindsBy(How = How.Id, Using = "_run_button")]
        public IWebElement BtnRun { get; set; }

        [FindsBy(How = How.Id, Using = "fil_itemListFUI")]
        public IWebElement TableElemet { get; set; }

        [FindsBy(How = How.Id, Using = "_editTemplate__kCuraScrollingDiv__scriptRadioButtonList_radioButtonListField_radioButtonList_1")]
        public IWebElement CheckSelectet { get; set; }

        [FindsBy(How = How.Id, Using = "_editTemplate__kCuraScrollingDiv__relativityScriptPopUp_pick")]
        public IWebElement Explorate { get; set; }

        [FindsBy(How = How.Id, Using = "_artifacts_itemList_FILTER-BOOLEANSEARCH[Name]-T")]
        public IWebElement SearchScript { get; set; }

        [FindsBy(How = How.Id, Using = "_artifacts_itemList_listTable")]
        public IWebElement table { get; set; }

        [FindsBy(How = How.Id, Using = "_ok2_button")]
        public IWebElement BtnOk { get; set; }

        [FindsBy(How = How.Id, Using = "_editTemplate_saveAndBack1_button")]
        public IWebElement BtnSaveandBack { get; set; }

        [FindsBy(How = How.Id, Using = "iDestinationField_dropDownList")]
        public IWebElement iDestinationField { get; set; }

        [FindsBy(How = How.Id, Using = "iChoiceField1_dropDownList")]
        public IWebElement ComboboxChoiceField1 { get; set; }

        [FindsBy(How = How.Id, Using = "iTextField1_dropDownList")]
        public IWebElement ComboboxTextField1 { get; set; }

        [FindsBy(How = How.Id, Using = "iChoiceField2_dropDownList")]
        public IWebElement ComboboxChoiceField2 { get; set; }

        [FindsBy(How = How.Id, Using = "iTextField2_dropDownList")]
        public IWebElement ComboboxTextField2 { get; set; }

        [FindsBy(How = How.Id, Using = "iChoiceField3_dropDownList")]
        public IWebElement ComboboxChoiceField3 { get; set; }

        [FindsBy(How = How.Id, Using = "iTextField3_dropDownList")]
        public IWebElement ComboboxTextField3 { get; set; }

        //verifica que un elemento esté presente en el DOM de una página y sea visible.Esto no significa necesariamente que el elemento sea visible.
        public IWebElement WaitForPageUntilElementExists(By locator, int maxSeconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(maxSeconds))
                .Until(ExpectedConditions.ElementExists((locator)));
        }

        //verifica que un elemento esté presente en el DOM de una página y sea visible.Tiene que ser visible
        public IWebElement WaitForPageUntilElementIsVisible(By locator, int maxSeconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(maxSeconds))
                .Until(ExpectedConditions.ElementIsVisible((locator)));
        }

        //verifica que todos los elementos presentes en la página web que coinciden con el localizador sean visibles.Tiene que ser visible
        public IReadOnlyCollection<IWebElement> VisibilityOfAllElementsLocatedBy(By locator, int maxSeconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(maxSeconds))
                .Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((locator)));
        }

        //verifica un elemento ews visible y está habilitada para que pueda hacer Click en él.
        public IWebElement WaitForPageUntilElementToBeClickable(By locator, int maxSeconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(maxSeconds))
                .Until(ExpectedConditions.ElementToBeClickable((locator)));
        }

        public void ProvisionEnvironment()
        {
            testWorkspaceID = createTestWorkspace();
            importTestDocuments(testWorkspaceID);
            uploadScript(testWorkspaceID);
        }

        public void CleanEnvironment()
        {
            DeletedWorkspace(testWorkspaceID);
        }

        private void uploadScript(int testWorkspaceID)
        {
            using (var client = TestEnvironment.Instance.ServicesManager.CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System))
            {
                Relativity.Test.Helpers.Application.ApplicationHelpers.ImportApplication(client, testWorkspaceID, true, @"\RA_CombineFieldsIntoLongText_20180406213208.rap");
            }
        }

        private int createTestWorkspace()
        {
            int workspaceID;
            using (var client = TestEnvironment.Instance.ServicesManager.CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System))
            {
                workspaceID = CreateWorkspace.Create(client, ConfigurationHelper.TEST_WORKSPACE_NAME, ConfigurationHelper.TEST_WORKSPACE_TEMPLATE_NAME);
            }
            driver.Navigate().Refresh();
            return workspaceID;
        }

        private void importTestDocuments(int testWorkspaceID)
        {
            ImportAPIHelper.ImportDocumentsInFolder(testWorkspaceID, ConfigurationHelper.TEST_DATA_LOCATION, true);
        }

        public void ClickToWorkSpace()
        {
            Thread.Sleep(2000);
            WebDriverWait obj = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("_externalPage")));
            var selectElementSaveSearch = new SelectElement(ComboboxTemplate);
            selectElementSaveSearch.SelectByValue("_s1");
            Thread.Sleep(0500);
            driver.FindElement(By.LinkText(ConfigurationHelper.TEST_WORKSPACE_NAME)).Click();
        }

        public void ClickTabAdmin()
        {
            VisibilityOfAllElementsLocatedBy(By.Id("_externalPage"), 10);
            TabAdmin.Click();
            WaitForPageUntilElementIsVisible(By.LinkText("Scripts"), 10);
            TabObject.Click();
        }

        public void RunScript()
        {
            WebDriverWait OBJ = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            OBJ.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("_externalPage")));
            VisibilityOfAllElementsLocatedBy(By.Id("fil_itemListFUI"), 10);
            IList<IWebElement> all = TableScripts.FindElements(By.TagName("tr"));
            IList<IWebElement> all1 = all[3].FindElements(By.TagName("td"));
            IList<IWebElement> all2 = all1[5].FindElements(By.TagName("a"));
            all2[0].Click();
        }

        public void DeletedWorkspace(int testWorkspaceID)
        {
            using (var client = TestEnvironment.Instance.ServicesManager.CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System))
            {
                DeleteWorkspace.Delete(client, testWorkspaceID);
            }
        }

        public bool SelectTypeSearchCombineFields()
        {
            RunForm.Click();
            driver.SwitchTo().Window(driver.WindowHandles.ToList().Last());
            var selectElementSaveSearch = new SelectElement(ComboboxSaveSearch);
            selectElementSaveSearch.SelectByValue("1038052");
            Thread.Sleep(0500);
            RandomChoiceField1();
            Thread.Sleep(0500);
            RandomTextField1();
            RandomChoiceField2();
            Thread.Sleep(0500);
            RandomTextField2();
            RandomChoiceField3();
            Thread.Sleep(0500); ;
            RandomTextField3();
            var selectDestinationField = new SelectElement(iDestinationField);
            selectDestinationField.SelectByValue("EmailTo");
            BtnRun.Click();
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles.ToList().First());

            return true;
        }



        public void RandomChoiceField1()
        {
            IList<IWebElement> all = ComboboxChoiceField1.FindElements(By.TagName("option"));
            Random random = new Random();
            int posicion = random.Next(0, all.Count);
            var selectElementField1 = new SelectElement(ComboboxChoiceField1);
            all[posicion].Click();
        }

        public void RandomTextField1()
        {
            IList<IWebElement> all = ComboboxTextField1.FindElements(By.TagName("option"));
            Random random = new Random();
            int posicion = random.Next(0, all.Count);
            var selectElementField1 = new SelectElement(ComboboxTextField1);
            all[posicion].Click();
        }

        public void RandomChoiceField2()
        {
            IList<IWebElement> all = ComboboxChoiceField2.FindElements(By.TagName("option"));
            Random random = new Random();
            int posicion = random.Next(0, all.Count);
            var selectElementField1 = new SelectElement(ComboboxChoiceField2);
            all[posicion].Click();
        }

        public void RandomTextField2()
        {
            IList<IWebElement> all = ComboboxTextField2.FindElements(By.TagName("option"));
            Random random = new Random();
            int posicion = random.Next(0, all.Count);
            var selectElementField1 = new SelectElement(ComboboxTextField2);
            all[posicion].Click();
        }

        public void RandomChoiceField3()
        {
            IList<IWebElement> all = ComboboxChoiceField3.FindElements(By.TagName("option"));
            Random random = new Random();
            int posicion = random.Next(0, all.Count);
            var selectElementField1 = new SelectElement(ComboboxChoiceField3);
            all[posicion].Click();
        }

        public void RandomTextField3()
        {
            IList<IWebElement> all = ComboboxTextField3.FindElements(By.TagName("option"));
            Random random = new Random();
            int posicion = random.Next(0, all.Count);
            var selectElementField1 = new SelectElement(ComboboxTextField3);
            all[posicion].Click();
        }

    }
}
