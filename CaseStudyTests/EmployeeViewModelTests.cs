
/**
 * Class Name:EmployeeViewModelTests.cs
 * Purpose: Test our queries to ensure our entity framework and database is set up correctly 
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/

using System;
using Xunit;
using HelpdeskViewModels;
using System.Diagnostics;
using System.Collections.Generic;
using Xunit.Abstractions;
namespace CaseStudyTests
{
   public class EmployeeViewModelTests
    {
        private readonly ITestOutputHelper output;
        //constructor to declare an variable called output 
        public EmployeeViewModelTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void Call_ComprehensiveVMTest()
        {
            CallViewModel cvm = new CallViewModel();
            EmployeeViewModel evm = new EmployeeViewModel();
            ProblemViewModel pvm = new ProblemViewModel();
            cvm.DateOpened = DateTime.Now;
            cvm.DateClosed = null;
            cvm.OpenStatus = true;
            evm.Email = "eg@abc.com";
            evm.GetByEmail();
            cvm.EmployeeId = evm.id;
            evm.Email = "sc@abc.com";
            evm.GetByEmail();
            cvm.TechId = evm.id;
            pvm.Description = "Memory Upgrade";
            pvm.GetByDescription();
            cvm.ProblemId = pvm.Id;
            cvm.Notes = "Eraj has bad RAM, Burner to fix it";
            cvm.Add();
            output.WriteLine("New Call Generated - Id = " + cvm.Id);
            int id = cvm.Id; //need id for delete later
            cvm.GetById();
            cvm.Notes += "\nOrdered new RAM!";

            if(cvm.Update() == 1)
            {
                output.WriteLine("Call was updated " + cvm.Notes);
            }
            else
            {
                output.WriteLine("Call was not updated ");
            }
            cvm.Notes = "Another change to comments that should not work";
            if(cvm.Update() == -2)
            {
                output.WriteLine("Call was not updated data was stale");
            }

            cvm = new CallViewModel
            {
                Id = id
            };
            //cvm.Id = id;
            //need to reset because of concurrency error
            cvm.GetById(); 
            if(cvm.Delete() == 1)
            {
                output.WriteLine("Call was deleted!");
            }
            else
            {
                output.WriteLine("Call was not deleted");
            }

            Exception ex = Assert.Throws<NullReferenceException>(() => cvm.GetById()); // should throw expected exception 
            Assert.Equal("Object reference not set to an instance of an object.", ex.Message);
        }

        
        //test if we can find employee information by providing an email 
        [Fact]
        public void Employee_GetByEmail()
        {
            EmployeeViewModel vm = new EmployeeViewModel { Email = "bs@abc.com" };

            vm.GetByEmail();
            Assert.NotNull(vm.Email);
        }

        //test if we can find employee information by providing an id 
        [Fact]
        public void Employee_GetById()
        {
            EmployeeViewModel vm = new EmployeeViewModel { id = 2 };
            //vm.GetByEmail();
            vm.GetById();
            Assert.NotNull(vm.FirstName);
        }

        //test if we return a list of employees by using the GetAll method 
        [Fact]
        public void Employee_GetAllTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            List<EmployeeViewModel> allEmployees = vm.GetAll();
            Assert.True(allEmployees.Count > 0);
        }

        //test to see if we can add an employee 
        [Fact]
        public void Employee_AddTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel
            {
                Title = "Ms.",
                FirstName = "Eraj",
                LastName = "Gillani",
                PhoneNo = "(555)555-5551",
                DepartmentID = 100,
                Email = "eg@random.ca",
                isTech = true
            };
            vm.Add();
            Assert.True(vm.id > 0);
        }

        //test to see if we can update an employee 
        [Fact]
        public void Employee_UpdateTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel { Email = "eg@random.ca" };
            vm.GetByEmail(); // Employee just added
            //ternary operation makes sure the data changes each time the test is executed
            //EF is smart enough to know if data doesn't change, it won't do the update 
            vm.PhoneNo = vm.PhoneNo == "(555)555-5551" ? "(555)555-5552" : "(555)555-5551";
            int EmployeesUpdated = vm.Update();
            Assert.True(EmployeesUpdated > 0);
        }

        //test to see if we can delete an employee 
        [Fact]
        public void Employee_DeleteTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel { Email = "eg@random.ca" };
            vm.GetByEmail();
            int EmployeesDeleted = vm.Delete();
            Assert.True(EmployeesDeleted == 1);
        }
    }
}
