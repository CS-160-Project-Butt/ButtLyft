using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using AASC.Partner.Web.Tests.Models;
using Newtonsoft.Json;

namespace AASC.Partner.Web.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public string TOKEN = "782qCDz7Z96KylkTqDa3NZT_BKwPla9M-0JXYEQuHgNtHsrE3Rb__UlWZaU-1jTKOSidq8WN6d1kcHNEuo6JWkNSy7VfTrZjlG9alkBG0oJ1-WvVSQmNPyd41j78MkJsQlIDckBpYtWTiU2Ltjfk0AAFHZ0dS9J5fYqUaY5PCt6PiavG6XNeOzWYEpbY1KJ1ZRYjiETrXSLt6DFZN_cm4voKdidyrSZhfLxZeOBeX_tMT2NaiF00KDtfRbq1pXZgiSgYzX0sGv0gqz28RHESHis8l88G9tIsftBzAwIR4CanX9gheqrafk85wJ_6q9XgHAT9oNIsKeFDJN7w4RAwObdkxgQ5sssquI3UAAOPC0fVRiHlYKs2OR9GXNL6rfRJdl5y4Y2-CsU5KTWpg9dSpgw7OlNSQlQ9Ayelp4tyTlzDM_wnCh5TfN_0OVYbWdLu4SWCRA3EYuIbMrGV_jZDEMJmPnTyEFg05g-9SOC1wDM";
        // create user account
        [TestMethod]
        public void TestCreateUserAccount()
        {
            CreateUserAccounts().Wait();
            //RunCreateUserAccountAsync().Wait();
            //RunChangeUserPasswordAsync().Wait();
        }

        private async Task CreateUserAccounts()
        {
            var users = new List<CreateUserBindingModel>();
            users.Add(new CreateUserBindingModel()
            {
                Email = "eric.shih@outlook.com",
                UserName = "Eric.Shih",
                FirstName = "Eric",
                LastName = "Shih",
                RoleName = "Admin",
                Password = "MySuperP@ss1",                
                ConfirmPassword = "MySuperP@ss1"
            });
            //users.Add(new CreateUserBindingModel()
            //{
            //    Email = "shih.eric@hotmail.com",
            //    UserName = "User123",
            //    FirstName = "Eric",
            //    LastName = "Shih",
            //    RoleName = "User",
            //    Password = "MySuperP@ss1",
            //    ConfirmPassword = "MySuperP@ss1"
            //});
            //users.Add(new CreateUserBindingModel()
            //{
            //    Email = "ednag@advantech.com",
            //    UserName = "Edna.Garcia",
            //    FirstName = "Edna",
            //    LastName = "Garcia",
            //    RoleName = "ContentProvider",
            //    Password = "Edn@123",
            //    ConfirmPassword = "Edn@123"
            //});
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:1272/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                foreach (var user in users)
                {
                    var response = await client.PostAsJsonAsync("api/account/create", user);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                    else
                    {
                        Console.WriteLine(response.RequestMessage);
                    }
                }                
            }
            
        }

        async Task RunCreateUserAccountAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:1272/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var userToCreate = new CreateUserBindingModel()
                {
                    Email = "shih.eric@hotmail.com",
                    UserName = "User123",
                    FirstName = "Eric",
                    LastName = "Shih",
                    RoleName = "User",
                    Password = "MySuperP@ss1",
                    ConfirmPassword = "MySuperP@ss1"
                };
                var response = await client.PostAsJsonAsync("api/account/create", userToCreate);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    Console.WriteLine(response.RequestMessage);
                }
            }
        }

        async Task RunChangeUserPasswordAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:1272/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var changePassword = new ChangePasswordInBatchBindingModel
                {
                    UserName = "Edna.Garcia",
                    OldPassword = "MySuperP@ss!",
                    NewPassword = "Edn@123",
                    ConfirmPassword = "Edn@123",
                };

                var response = await client.PostAsJsonAsync("api/account/changePasswordInBatch", changePassword);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    Console.WriteLine(response.RequestMessage);
                }
            }
        }

        [TestMethod]
        public void TestCreateCompany()
        {
            //CreateCompany().Wait();

            
        }

        [TestMethod]
        public void TestGetCompany()
        {
            //GetCompany().Wait();
        }

        [TestMethod]
        public void TestUpdateCompany()
        {
            UpdateCompany().Wait();
        }

        private async Task CreateCompany()
        {            
            using (var client = new HttpClient())
            {
                var company = new Company()
                {
                    Name = "Advantech",  
                    CreatedById = "a209fc33-0f44-4bdf-8c2c-49cafe083953",
                    Partners = new List<AASC.Partner.Web.Tests.Models.Partner>(),
                    Departments = new List<Department>()
                };
                client.BaseAddress = new Uri("http://localhost:1272/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("Authorization", "Bearer -GOzNxx1lB9obL5NR8Kt05aftWe8oQGqkCtDfWP7_xQB_8H7v1M2ZJi_HJ-njl2G6NL9J3GyHAGj0fjxzFdBtDGmhxvmjHIoZ-z08f0_BNCsyuHPaffZnB1uND0NUSfUidnR-e36qgivlaffyUE7ZzqQYHjIunn2jnG9byi5TEhCWooH-Npy9wMCY1WRASjGP3jIlNDMk75bVxplG7dUKcbqSVF6GOIVCR_Lg5sKOlEaG2WEXbWa94eTZIng9keEFVycIJ9Lfogg5aVxEQKpfQ4snsfuQh751hP0XlagkYCVDtB62Ew_OFcYCJ3XbBVM_rjGLyF09PF-woqKxs_hIstywB1DarPNOlAElKlzL3lkUywxZSs9PXe5CZO1np5aZ7sRJJYrRLfCsd3Lg60AOwTJjmLzruruB2H0FcpN-slTgGmBUFZ-j12t6sDEACFGJ_g2Q53dnfoLj5tbHnySXtq3PXROyMJxPaRFuzbtbfY");
                var response = await client.PostAsJsonAsync("api/companies", company);                
                
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    Console.WriteLine(response.RequestMessage);
                }

                Assert.AreEqual(response.IsSuccessStatusCode, true);
            }
        }

        private async Task GetCompany()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:1272/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("Authorization", "Bearer -GOzNxx1lB9obL5NR8Kt05aftWe8oQGqkCtDfWP7_xQB_8H7v1M2ZJi_HJ-njl2G6NL9J3GyHAGj0fjxzFdBtDGmhxvmjHIoZ-z08f0_BNCsyuHPaffZnB1uND0NUSfUidnR-e36qgivlaffyUE7ZzqQYHjIunn2jnG9byi5TEhCWooH-Npy9wMCY1WRASjGP3jIlNDMk75bVxplG7dUKcbqSVF6GOIVCR_Lg5sKOlEaG2WEXbWa94eTZIng9keEFVycIJ9Lfogg5aVxEQKpfQ4snsfuQh751hP0XlagkYCVDtB62Ew_OFcYCJ3XbBVM_rjGLyF09PF-woqKxs_hIstywB1DarPNOlAElKlzL3lkUywxZSs9PXe5CZO1np5aZ7sRJJYrRLfCsd3Lg60AOwTJjmLzruruB2H0FcpN-slTgGmBUFZ-j12t6sDEACFGJ_g2Q53dnfoLj5tbHnySXtq3PXROyMJxPaRFuzbtbfY");

                var response = await client.GetStringAsync("api/companies");

                var result = JsonConvert.DeserializeObject<ResponseResult>(response);

                var data = JsonConvert.DeserializeObject<List<Company>>(result.Data.ToString());
                                
                Assert.AreEqual(result.Total, 1);
                               
            }
        }

        private async Task UpdateCompany()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:1272/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("Authorization", "Bearer -GOzNxx1lB9obL5NR8Kt05aftWe8oQGqkCtDfWP7_xQB_8H7v1M2ZJi_HJ-njl2G6NL9J3GyHAGj0fjxzFdBtDGmhxvmjHIoZ-z08f0_BNCsyuHPaffZnB1uND0NUSfUidnR-e36qgivlaffyUE7ZzqQYHjIunn2jnG9byi5TEhCWooH-Npy9wMCY1WRASjGP3jIlNDMk75bVxplG7dUKcbqSVF6GOIVCR_Lg5sKOlEaG2WEXbWa94eTZIng9keEFVycIJ9Lfogg5aVxEQKpfQ4snsfuQh751hP0XlagkYCVDtB62Ew_OFcYCJ3XbBVM_rjGLyF09PF-woqKxs_hIstywB1DarPNOlAElKlzL3lkUywxZSs9PXe5CZO1np5aZ7sRJJYrRLfCsd3Lg60AOwTJjmLzruruB2H0FcpN-slTgGmBUFZ-j12t6sDEACFGJ_g2Q53dnfoLj5tbHnySXtq3PXROyMJxPaRFuzbtbfY");

                var response = await client.GetStringAsync("api/companies");

                var result = JsonConvert.DeserializeObject<ResponseResult>(response);

                var data = JsonConvert.DeserializeObject<List<Company>>(result.Data.ToString());

                var customer = data[0];

                customer.Name = "Advantech";

                var updateResult = await client.PutAsJsonAsync(string.Format("api/companies/{0}", customer.Id), customer);

                Assert.AreEqual(updateResult.IsSuccessStatusCode, true);
            }
        }

        [TestMethod]
        public void TestCreateDepartment()
        {
           // CreateDepartment().Wait();
        }

        private async Task CreateDepartment()
        {
            using (var client = new HttpClient())
            {                
                var department = new DepartmentViewModel()
                {
                    Name = "App",
                    CompanyId = Guid.Parse("153A216E-CC1E-4C73-BDEC-F636E877EC85"),
                    ParentDepartmentId = Guid.Parse("3492420B-C1B1-48E9-8CBF-D183E2B69FB4"),
                    DepartmentHeadEmployeeId = null,
                    Employees = new List<EmployeeViewModel>()
                };

                client.BaseAddress = new Uri("http://localhost:1272/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", TOKEN));
                var response = await client.PostAsJsonAsync("api/departments", department);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    Console.WriteLine(response.RequestMessage);
                }

                Assert.AreEqual(response.IsSuccessStatusCode, true);
            }
        }

        [TestMethod]
        public void TestCreateEmployee()
        {
            CreateEmployee().Wait();
        }

        private async Task CreateEmployee()
        {
            using (var client = new HttpClient())
            {
                var employee = new EmployeeViewModel
                {
                    //JobTitle = "IT Director",
                    //CompanyId = Guid.Parse("345f7376-b8e1-4242-b7fb-e6e693ef1fbc"),
                    //ApplicationUserId = "4e74e1e3-85b0-418f-872a-105a21949382",
                    JobTitle = "Application Manager",
                    CompanyId = Guid.Parse("345f7376-b8e1-4242-b7fb-e6e693ef1fbc"),
                    ApplicationUserId = "b2339fda-2417-4aec-8728-2ec09da04227"
                };                               

                client.BaseAddress = new Uri("http://localhost:1272/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", TOKEN));
                var response = await client.PostAsJsonAsync("api/employees", employee);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    Console.WriteLine(response.RequestMessage);
                }

                Assert.AreEqual(response.IsSuccessStatusCode, true);
            }
        }

        [TestMethod]
        public void TestModifyEmployee()
        {
            ModifyEmployee().Wait();
        }

        private async Task ModifyEmployee()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:1272/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", TOKEN));

                var response = await client.GetStringAsync("api/employees/getemployee/CF60D836-41AF-41FC-87DF-8D75B581C24A");

                var result = JsonConvert.DeserializeObject<ResponseResult>(response);

                var data = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(result.Data.ToString());

                var employee = data[0];

                Console.WriteLine(employee.ApplicationUser.Username);
                
                //var updateResult = await client.PutAsJsonAsync("api/employees", employee);

                //if (updateResult.IsSuccessStatusCode)
                //{
                //    Console.WriteLine(updateResult.StatusCode);
                //}
                //else
                //{
                //    Console.WriteLine(updateResult.RequestMessage);
                //}

                //Assert.AreEqual(updateResult.IsSuccessStatusCode, true);
            }
        }
    }

    public class ResponseResult
    {
        public object Data { get; set; }
        public int Total { get; set; }
    }

    public class CreateUserBindingModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string RoleName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordInBatchBindingModel
    {
        public string UserName { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
