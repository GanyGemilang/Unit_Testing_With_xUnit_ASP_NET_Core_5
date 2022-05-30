using Castle.Core.Configuration;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Unit_Testing_xUnit_ASP_NET_Core;
using Unit_Testing_xUnit_ASP_NET_Core.Controllers;
using Unit_Testing_xUnit_ASP_NET_Core.Models;
using Unit_Testing_xUnit_ASP_NET_Core.Repositories;
using Unit_Testing_xUnit_ASP_NET_Core.Utilities;
using Xunit;

namespace Test_xUnit_ASP_NET_CORE_5
{
    public class UnitTest1
    {
        public string token;
        public UnitTest1()
        {
            token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VybmFtZSI6InN0cmluZyIsImV4cCI6MTY1MzQ2NjU1MywiaXNzIjoiVXNlck1hbmFnZW1lbnRBUElBdXRoZW50aWNhdGlvblNlcnZlciIsImF1ZCI6IlVzZXJNYW5hZ2VtZW50QVBJU2VydmljZVBvc3RtYW5DbGllbnQifQ.2A24n-WuSEHuey6jaNRfZ8Az8f-nrZRkjIXBunTOenI";
        }

    // Start Test Case Login
        [Fact]
        public void Login200()
        {
            //arrange
            modelLogin model = new modelLogin();
            /*model.username = "ganygemilang";
            model.password = "j44SC0n5";*/
            model.username = "string";
            model.password = "string";

            //act;
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + session);
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync("https://localhost:44391/api/User/Login", content).Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<dynamic>(readResult);

            //Assert  
            //Assert.IsType<HttpResponseMessage>(result);
            var expected = "ok";
            var status = result.StatusCode.ToString().ToLower();
            Assert.Equal(expected, status);
        }

        [Fact]
        public void Login404UserOnline()
        {
            //arrange
            modelLogin model = new modelLogin();
            model.username = "string";
            model.password = "string";

            //act;
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + session);
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync("https://localhost:44391/api/User/Login", content).Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ResponseModel>(readResult);
            
            //Assert  
            var expected = "notfound";
            var status = result.StatusCode.ToString().ToLower();
            
            var expectedMessage = "user is online";
            var message = res.Message.ToLower();

            Assert.Equal(expected, status);
            Assert.Equal(expectedMessage, message);
        }

        [Fact]
        public void Login400UsernameNotNull()
        {
            //arrange
            modelLogin model = new modelLogin();
            

            //act;
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + session);
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync("https://localhost:44391/api/User/Login", content).Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ResponseModel>(readResult);
            
            //Assert  
            var expected = "badrequest";
            var status = result.StatusCode.ToString().ToLower();

            var expectedMessage = "username cannot be null";
            var message = res.Message.ToLower();

            Assert.Equal(expected, status);
            Assert.Equal(expectedMessage, message);
        }

        [Fact]
        public void Login400PasswordNotNull()
        {
            //arrange
            modelLogin model = new modelLogin();
            model.username = "string";

            //act;
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + session);
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync("https://localhost:44391/api/User/Login", content).Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ResponseModel>(readResult);
            
            //Assert  
            var expected = "badrequest";
            var status = result.StatusCode.ToString().ToLower();

            var expectedMessage = "password cannot be null";
            var message = res.Message.ToLower();

            Assert.Equal(expected, status);
            Assert.Equal(expectedMessage, message);
        }
        
        [Fact]
        public void Login404UsernameNotFound()
        {
            //arrange
            modelLogin model = new modelLogin();
            model.username = "testt";
            model.password = "testt";

            //act;
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + session);
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync("https://localhost:44391/api/User/Login", content).Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ResponseModel>(readResult);
            
            //Assert  
            var expected = "notfound";
            var status = result.StatusCode.ToString().ToLower();

            var expectedMessage = "username not found";
            var message = res.Message.ToLower();

            Assert.Equal(expected, status);
            Assert.Equal(expectedMessage, message);
        }
        
        [Fact]
        public void Login400PasswordWrong()
        {
            //arrange
            modelLogin model = new modelLogin();
            model.username = "string";
            model.password = "123";

            //act;
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + session);
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync("https://localhost:44391/api/User/Login", content).Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ResponseModel>(readResult);
            
            //Assert  
            var expected = "badrequest";
            var status = result.StatusCode.ToString().ToLower();

            var expectedMessage = "password is wrong";
            var message = res.Message.ToLower();

            Assert.Equal(expected, status);
            Assert.Equal(expectedMessage, message);
        }
    // End Login Test Case

    //Start Get User Test Case
        [Fact]
        public void GetUser200()
        {
            //arrange
            
            //act;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + token);
            var result = httpClient.GetAsync("https://localhost:44391/api/User/GetUser").Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ResponseModel>(readResult);

            //Assert  
            var expected = "ok";
            var status = result.StatusCode.ToString().ToLower();
            if (status != "unauthorized")
            {
                var expectedMessage = "get data user successful";
                var message = res.Message.ToLower();
                Assert.Equal(expectedMessage, message);
            }

            Assert.Equal(expected, status);
            
        }
        
        [Fact]
        public void GetUser401()
        {
            //arrange
            

            //act;
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + token);
            var result = httpClient.GetAsync("https://localhost:44391/api/User/GetUser").Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ResponseModel>(readResult);

            //Assert  
            var expected = "unauthorized";
            var status = result.StatusCode.ToString().ToLower();

            Assert.Equal(expected, status);
            
        }
     // End Get User Test Case

     //Start Get User By Username Test Case
        [Fact]
        public void GetUserByUsername200()
        {
            //arrange
           

            //act;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + token);
            var result = httpClient.GetAsync("https://localhost:44391/api/User/GetUserByUsername?username=ganygemilang").Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ResponseModel>(readResult);

            //Assert  
            var expected = "ok";
            var status = result.StatusCode.ToString().ToLower();
            if (status != "unauthorized")
            {
                var expectedMessage = "get data user successful";
                var message = res.Message.ToLower();
                Assert.Equal(expectedMessage, message);
            }

            Assert.Equal(expected, status);

        }
        
        [Fact]
        public void GetUserByUsername400UsernameNotNull()
        {
            //arrange
            
            //act;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + token);
            var result = httpClient.GetAsync("https://localhost:44391/api/User/GetUserByUsername?username=").Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ResponseModel>(readResult);

            //Assert  
            var expected = "badrequest";
            var status = result.StatusCode.ToString().ToLower();
            if (status != "unauthorized")
            {
                var expectedMessage = "username cannot be null";
                var message = res.Message.ToLower();
                Assert.Equal(expectedMessage, message);
            }

            Assert.Equal(expected, status);

        }
        
        [Fact]
        public void GetUserByUsername401()
        {
            //arrange
            

            //act;
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + token);
            var result = httpClient.GetAsync("https://localhost:44391/api/User/GetUserByUsername?username=ganygemilang").Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ResponseModel>(readResult);

            //Assert  
            var expected = "unauthorized";
            var status = result.StatusCode.ToString().ToLower();

            Assert.Equal(expected, status);

        }

        [Fact]
        public void GetUserByUsername404UsernameNotFound()
        {
            //arrange

            //act;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer " + token);
            var result = httpClient.GetAsync("https://localhost:44391/api/User/GetUserByUsername?username=123").Result;
            var readResult = result.Content.ReadAsStringAsync().Result;
            var res = JsonConvert.DeserializeObject<ResponseModel>(readResult);

            //Assert  
            var expected = "notfound";
            var status = result.StatusCode.ToString().ToLower();
            if (status != "unauthorized")
            {
                var expectedMessage = "username not found";
                var message = res.Message.ToLower();
                Assert.Equal(expectedMessage, message);
            }

            Assert.Equal(expected, status);

        }

        // End Get User By Username Test Case
    }
}
