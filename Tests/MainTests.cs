using NUnit.Framework;
using SeleniumUnitTest;

namespace CreateObjectAfterFieldParsing
{
    [TestFixture]
    public class MainTests
    {
        Helper helper = new Helper();
        [SetUp]
        public void Setup()
        {
            PageInitial home = new PageInitial();
            home.GoToPage();
            home.LoginToApplication();
        }

        [Test]
        [Category("UC1 - CombineField")]
        public void RunScriptCombineFieldTest()
        {
            // -- arrange: dedicated to collect input data
            helper.ProvisionEnvironment();
            helper.ClickToWorkSpace();
            helper.ClickTabAdmin();


            //--- act: run process
            helper.RunScript();
            var retuned = helper.SelectTypeSearchCombineFields();

            //--- assert: validate results
            Assert.IsTrue(retuned);
        }

        [TearDown]
        public void TearDown()
        {
            helper.CleanEnvironment();
        }
    }
}
