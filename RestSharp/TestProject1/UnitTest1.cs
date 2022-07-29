using RestSharp;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace EmployeePayrollTest
{
    public class RestsharpTest
    {
        RestClient client = new RestClient("http://localhost:4000");

        [SetUp]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        /// <summary>
        /// Gets the employee list.
        /// </summary>
        /// <returns></returns>
        //class to get list of employees
        public RestResponse GetEmployeeList()
        {
            //Arrange
            //Initialize the request object with proper method and URL
            //passing rest request for employees list api using get method
            RestRequest request = new RestRequest("/employees", Method.Get);
            //Act
            //Execute the request
            RestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// UC 1 : Retrieve all employee details in the json file
        /// </summary>
        [Test]
        public void OnCallingGetAPI_ReturnEmployeeList()
        {
            //calling the method
            RestResponse response = GetEmployeeList();
            //checks if the status code of response equals the employee code for the method requested
            //and checks response as okay or not
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            //convert the response object to list of employees
            //get 
            List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            //checking whether list is equal to count
            Assert.AreEqual(4, employeeList.Count);

            foreach (Employee emp in employeeList)
            {
                Console.WriteLine("Id: " + emp.Id + "\t" + "Name: " + emp.Name + "\t" + "Salary: " + emp.Salary);
            }
        }
    }
}



