using AddressBookRestSharp;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Nodes;

namespace AddressBookTest
{
    public class UnitTest1
    {
        RestClient client;

        [SetUp]
        public void SetUp()
        {
            //Initialize the base URL to execute requests made by the instance
            client = new RestClient("http://localhost:5000");
        }

        private RestResponse GetContactList()
        {
            //Arrange
            //Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("/contacts/list", Method.Get);
            //Act
            // Execute the request
            RestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// UC 1 : Reads the entries from json server.
        /// </summary>
        [Test]
        public void ReadEntriesFromJsonServer()
        {
            RestResponse response = GetContactList();
            // Check if the status code of response equals the default code for the method requested
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Convert the response object to list of employees
            List<Contact> employeeList = JsonConvert.DeserializeObject<List<Contact>>(response.Content);
            Assert.AreEqual(4, employeeList.Count);
            foreach (Contact c in employeeList)
            {
                Console.WriteLine($"Id: {c.Id}\tFullName: {c.FirstName} {c.LastName}\tPhoneNo: {c.PhoneNumber}\tAddress: {c.Address}\tCity: {c.City}\tState: {c.State}\tZip: {c.Zip}\tEmail: {c.Email}");
            }
        }
        /// <summary>
        /// UC 2 Add multiple contacts to the address book JSON server and return the same
        /// </summary>
        [Test]
        public void OnCallingPostAPIForAContactListWithMultipleContacts_ReturnContactObject()
        {
            // Arrange
            List<Contact> contactList = new List<Contact>();
            contactList.Add(new Contact { FirstName = "Arun", LastName = "R", PhoneNumber = "9577456345", Address = "Feroz Shah Kotla", City = "New Delhi", State = "New Delhi", Zip = "547677", Email = "vs@gmail.com" });
            contactList.Add(new Contact { FirstName = "Ankitha", LastName = "H", PhoneNumber = "9756723456", Address = "Chinnaswamy", City = "Bangalore", State = "Karnataka", Zip = "435627", Email = "yc@gmail.com" });
            contactList.Add(new Contact { FirstName = "Yash", LastName = "V", PhoneNumber = "9954564345", Address = "Mohali", City = "Mohali", State = "Punjab", Zip = "113425", Email = "klr@gmail.com" });

            //Iterate the loop for each contact
            foreach (var v in contactList)
            {
                //Initialize the request for POST to add new contact
                RestRequest request = new RestRequest("/contacts/list", Method.Post);
                JsonObject jsonObj = new JsonObject();
                jsonObj.Add("firstname", v.FirstName);
                jsonObj.Add("lastname", v.LastName);
                jsonObj.Add("phoneNumber", v.PhoneNumber);
                jsonObj.Add("address", v.Address);
                jsonObj.Add("city", v.City);
                jsonObj.Add("state", v.State);
                jsonObj.Add("zip", v.Zip);
                jsonObj.Add("email", v.Email);

                //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                RestResponse response = client.Execute(request);

                //Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Contact contact = JsonConvert.DeserializeObject<Contact>(response.Content);
                Assert.AreEqual(v.FirstName, contact.FirstName);
                Assert.AreEqual(v.LastName, contact.LastName);
                Assert.AreEqual(v.PhoneNumber, contact.PhoneNumber);
                Console.WriteLine(response.Content);
            }
        }
    }
}