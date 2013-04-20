using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TmcWinUITest
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class MainFuntionalitiesTest
    {
        public MainFuntionalitiesTest()
        {
        }

        [TestMethod]
        public void StartAndAnalse()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            this.UIMap.StartTMC();
            this.UIMap.DoAnalyse();
            this.UIMap.SaveAnalyseResults();
            //this.UIMap.ChangeToDetailView();
            //this.UIMap.ChangeToBigIconsView();
            this.UIMap.GotoViewTab();
            this.UIMap.ToggleTitleVisibility();


            // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
        }
        [TestMethod]
        public void ChangeView()
        {
            this.UIMap.StartTMC();
            System.Threading.Thread.Sleep(10000);
            this.UIMap.GotoViewTab();
            System.Threading.Thread.Sleep(10000);
            this.UIMap.ToggleTitleVisibility();
            System.Threading.Thread.Sleep(10000);

            this.UIMap.CloseTMC();

        }

        [TestMethod]
        public void TestNameFilter()
        {
            this.UIMap.StartTMC();
            this.UIMap.AddNameFilter();
            this.UIMap.AssertNameFilterWork();
            this.UIMap.CloseTMC();
        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //    // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //    // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
