using System;
using System.ServiceProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalRBroadcastServiceSample;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;
using SignalrDomain;

namespace UnitTests
{
    [TestClass]
    public class TestCurrencyExchangeService
    {
        private CurrencyExchangeService _service = new CurrencyExchangeService();

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your 
        //tests:
        //Use ClassInitialize to run code before running the first test in the
        //class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {

        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {

        }

        #endregion

        [TestMethod]
        public void TestClientGetMarketStateFromHub()
        {
            // Make sure to call WebApp.Start:
            PrivateObject privateObject = new PrivateObject(_service);
            privateObject.Invoke("OnStart", new object[] { null });

            // Create client proxy and call hub method
            using (HubConnection hub = new HubConnection(String.Format("http://{0}:8084", "localhost")))
            {
                IHubProxy proxy = hub.CreateHubProxy("CurrencyExchangeHub");
                hub.Start().Wait();

                var state = proxy.Invoke<string>("GetMarketState").Result;
                Assert.IsNotNull(state);
                Assert.IsTrue(state.Length > 0);
            }
        }

        [TestMethod]
        public void TestClientGetAllCurrenciesFromHub()
        {
            // Make sure to call WebApp.Start:
            PrivateObject privateObject = new PrivateObject(_service);
            privateObject.Invoke("OnStart", new object[] { null });

            // Create client proxy and call hub method
            using (HubConnection hub = new HubConnection(String.Format("http://{0}:8084", "localhost")))
            {
                IHubProxy proxy = hub.CreateHubProxy("CurrencyExchangeHub");
                hub.Start().Wait();

                var currencies = proxy.Invoke<IEnumerable<Currency>>("GetAllCurrencies").Result;
                Assert.IsNotNull(currencies);
                Assert.IsTrue(currencies.ToString().Length > 0);
            }
        }

        [TestMethod]
        public void TestClientOpenCloseMarketFromHub()
        {
            // Make sure to call WebApp.Start:
            PrivateObject privateObject = new PrivateObject(_service);
            privateObject.Invoke("OnStart", new object[] { null });

            // Create client proxy and call hub method
            using (HubConnection hub = new HubConnection(String.Format("http://{0}:8084", "localhost")))
            {
                IHubProxy proxy = hub.CreateHubProxy("CurrencyExchangeHub");
                hub.Start().Wait();

                var state = proxy.Invoke<bool>("OpenMarket").Result;
                Assert.IsNotNull(state);
                Assert.IsTrue(state == true);

                state = proxy.Invoke<bool>("CloseMarket").Result;
                Assert.IsNotNull(state);
                Assert.IsTrue(state == true);
            }
        }

        [TestMethod]
        public void TestGetMarketStateFromHub()
        {
            CurrencyExchangeHub hub = new CurrencyExchangeHub(CurrencyExchangeService.Instance);
            var state = hub.GetMarketState();
            Assert.IsNotNull(state);
        }

        [TestMethod]
        public void TestOpenCloseMarket()
        {
            var currencies = CurrencyExchangeService.Instance.GetAllCurrencies();
            Assert.IsNotNull(currencies);
            bool expected = true;
            bool actual = CurrencyExchangeService.Instance.OpenMarket();
            Assert.AreEqual(expected, actual);
            actual = CurrencyExchangeService.Instance.OpenMarket();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestOpenCloseMarketFromHub()
        {
            var hub = new CurrencyExchangeHub(CurrencyExchangeService.Instance);
            var currencies = hub.GetAllCurrencies();
            Assert.IsNotNull(currencies);
            bool expected = true;
            bool actual = hub.OpenMarket();
            Assert.AreEqual(expected, actual);
            actual = hub.OpenMarket();
            Assert.AreEqual(expected, actual);
        }
    }
}
