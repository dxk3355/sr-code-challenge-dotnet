using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTest
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            Compensation compensation = new Compensation()
            {
                EffectiveDate = DateTime.Now,
                Salary = 65000.00M,
                EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3"
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.EmployeeId, newCompensation.EmployeeId);
            Assert.IsNotNull(newCompensation.Id);
        }


        [TestMethod]
        public void CreateCompensation_ForSameEmployee_Returns_Created()
        {
            // Arrange
            Compensation compensation = new Compensation()
            {
                EffectiveDate = DateTime.Now.AddDays(-365).AddHours(4),
                Salary = 90000.00M,
                EmployeeId = "1B149305-3BAD-4EBA-BDB8-E84CAC86F1FA"
            };

            // Arrange
            Compensation promotionCompensation = new Compensation()
            {
                EffectiveDate = DateTime.Now,
                Salary = 100000.00M,
                EmployeeId = "1B149305-3BAD-4EBA-BDB8-E84CAC86F1FA"
            };


            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var response = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json")).Result;

            // Assert #1
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.EmployeeId, newCompensation.EmployeeId);
            Assert.IsNotNull(newCompensation.Id);


            // Create a promotion
            var promotionRequestContent = new JsonSerialization().ToJson(promotionCompensation);

            var promotionResponse = _httpClient.PostAsync("api/compensation",
           new StringContent(promotionRequestContent, Encoding.UTF8, "application/json")).Result;


            // Assert the promotion Results
            Assert.AreEqual(HttpStatusCode.Created, promotionResponse.StatusCode);
            var secondCompensation = promotionResponse.DeserializeContent<Compensation>();
            Assert.AreEqual(promotionCompensation.Salary, secondCompensation.Salary);
            Assert.AreEqual(promotionCompensation.EffectiveDate, secondCompensation.EffectiveDate);
            Assert.AreEqual(promotionCompensation.EmployeeId, secondCompensation.EmployeeId);
            Assert.IsNotNull(secondCompensation.Id);

            GetCompensation_ForSameEmployee_Returns_ProperComensation(promotionCompensation);

        }

        public void GetCompensation_ForSameEmployee_Returns_ProperComensation(Compensation compensation)
        {
            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{compensation.EmployeeId}");
            var getReponse = getRequestTask.Result;

            // Assert that we get the most recent one
            Assert.AreEqual(HttpStatusCode.OK, getReponse.StatusCode);
            var getCompensation = getReponse.DeserializeContent<Compensation>();
            Assert.AreEqual(compensation.EffectiveDate, getCompensation.EffectiveDate);
            Assert.AreEqual(compensation.Salary, getCompensation.Salary);

        }


        /// <summary>
        /// Tests if the salaries are put with the current salary first and the older salary later you still get the proper salaries when doing creates and
        /// </summary>
        [TestMethod]
        public void CreateCompensation_PutSalaryOutOfOrder_Returns_Created()
        {
            // Arrange
            Compensation compensation = new Compensation()
            {
                EffectiveDate = DateTime.Now.AddDays(-365),
                Salary = 70000.00M,
                EmployeeId = "53599FE3-96F7-4DF5-B04F-AD45ADC309A0"
            };

            // Arrange
            Compensation promotionCompensation = new Compensation()
            {
                EffectiveDate = DateTime.Now,
                Salary = 100000.00M,
                EmployeeId = "53599FE3-96F7-4DF5-B04F-AD45ADC309A0"
            };

            var promotionRequestContent = new JsonSerialization().ToJson(promotionCompensation);

            var promotionResponse = _httpClient.PostAsync("api/compensation",
           new StringContent(promotionRequestContent, Encoding.UTF8, "application/json")).Result;


            // Assert #1
            Assert.AreEqual(HttpStatusCode.Created, promotionResponse.StatusCode);
            var secondCompensation = promotionResponse.DeserializeContent<Compensation>();
            Assert.AreEqual(promotionCompensation.Salary, secondCompensation.Salary);
            Assert.AreEqual(promotionCompensation.EffectiveDate, secondCompensation.EffectiveDate);
            Assert.AreEqual(promotionCompensation.EmployeeId, secondCompensation.EmployeeId);
            Assert.IsNotNull(secondCompensation.Id);


            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var response = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json")).Result;

            // Assert #2
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.EmployeeId, newCompensation.EmployeeId);
            Assert.IsNotNull(newCompensation.Id);

            GetCompensation_ForSameEmployee_Returns_ProperComensation(promotionCompensation);
        }



        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            // Arrange
            var EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3";
            Decimal expectedSalary = 65000.00M;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{EmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(expectedSalary, compensation.Salary);
            Assert.AreEqual(EmployeeId, compensation.EmployeeId);
        }

        [TestMethod]
        public void GetCompensationById_Returns_FailBecauseOfSpaces()
        {
            // Arrange
            var EmployeeId = "b7839309- 3348-463b-a7e3-5de1c168beb3";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{EmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

    }
}
