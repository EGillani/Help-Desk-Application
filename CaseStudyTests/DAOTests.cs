using Xunit;
using HelpDeskDAL;
using System.Collections.Generic;
using Xunit.Abstractions;
using System;

namespace CaseStudyTests
{
    public class DAOTests
    {
        private readonly ITestOutputHelper output;
        //constructor to declare an variable called output 
        public DAOTests(ITestOutputHelper output)
        {
            this.output = output; 
        }
        [Fact]
        public void Employee_GetByEmail()
        {
            EmployeeDAO dao = new EmployeeDAO();
            Employees selectedEmployee = dao.GetByEmail("bs@abc.com");
            Assert.NotNull(selectedEmployee);

        }

        [Fact]
        public void Employee_GetById()
        {
            EmployeeDAO dao = new EmployeeDAO();
            Employees selectedEmployee = dao.GetById(2);
            Assert.NotNull(selectedEmployee);
        }
        [Fact]
        public void Employee_GetAll()
        {
            EmployeeDAO dao = new EmployeeDAO();
            List<Employees> allEmployees = dao.GetAll();
            Assert.NotNull(allEmployees);
        }
        [Fact]
        public void Employee_AddTest()
        {
            EmployeeDAO dao = new EmployeeDAO();
            Employees newEmployee = new Employees
            {
                FirstName = "Eraj",
                LastName = "Gillani",
                PhoneNo = "(555) 555-1234",
                Title = "Ms.",
                DepartmentId = 100,
                Email = "eg@someemail.ca"
            };
            int newEmployeeId = dao.Add(newEmployee);
            Assert.True(newEmployeeId > 1);
        }
        [Fact]
        public void Employee_UpdateTest()
        {
            //int EmployeesUpdated = -1;
            EmployeeDAO dao = new EmployeeDAO();
            Employees employeeToUpdate = dao.GetByEmail("eg@someemail.ca");
            if (employeeToUpdate != null)
            {
                string oldPhoneNo = employeeToUpdate.PhoneNo;
                string newPhoneNo = oldPhoneNo == "(555) 555-1234" ? "(555) 555-5555" : "(555) 555-1234";
                employeeToUpdate.PhoneNo = newPhoneNo;
                //EmployeesUpdated = dao.Update(EmployeeToUpdate);

            }
            Assert.True(dao.Update(employeeToUpdate) == UpdateStatus.Ok);
            /*Assert.True(EmployeesUpdated != -1);*/
        }
        [Fact]
        public void Employee_ConcurrencyTest()
        {
            EmployeeDAO dao1 = new EmployeeDAO();
            EmployeeDAO dao2 = new EmployeeDAO();
            Employees employeeToUpdate1 = dao1.GetByEmail("eg@someemail.ca");
            Employees employeeToUpdate2 = dao2.GetByEmail("eg@someemail.ca");

            if (employeeToUpdate1 != null)
            {
                string oldPhoneNo = employeeToUpdate1.PhoneNo;
                string newPhoneNo = oldPhoneNo == "(555) 555-1234" ? "(555) 555-5555" : "(555) 555-1234";
                employeeToUpdate1.PhoneNo = newPhoneNo;
                if (dao1.Update(employeeToUpdate1) == UpdateStatus.Ok)
                {
                    employeeToUpdate2.PhoneNo = "(666) 666-6666";
                    Assert.True(dao2.Update(employeeToUpdate2) == UpdateStatus.Stale);
                }
                else
                {
                    Assert.True(false);
                }
            }
        }

        [Fact]
        public void Employee_DeleteTest()
        {
            EmployeeDAO dao = new EmployeeDAO();
            int EmployeesDeleted = dao.Delete(dao.GetByEmail("eg@someemail.ca").Id);
            Assert.True(EmployeesDeleted != -1);

        }

        [Fact]
        public void Employee_LoadPicsTest()
        {
            DALUtil util = new DALUtil();
            Assert.True(util.AddEmployeePicsToDb());
        }

        [Fact]
        public void Call_ComprehensiveTest()
        {
            CallDAO cdao = new CallDAO();
            EmployeeDAO edao = new EmployeeDAO();
            ProblemDAO pdao = new ProblemDAO();
            Calls call = new Calls
            {
                DateOpened = DateTime.Now,
                DateClosed = null,
                OpenStatus = true,
                EmployeeId = edao.GetByLastName("Gillani").Id,
                TechId = edao.GetByLastName("Burner").Id,
                ProblemId = pdao.GetByDescription("Hard Drive Failure").Id,
                Notes = "Eraj's drive is shot Burner to fix it"

            };
            //add a new call to see if our add works 
            int newCallId = cdao.Add(call);
            output.WriteLine("New Call Generated - Id = " + newCallId);
            call = cdao.GetById(newCallId);
            byte[] oldtimer = call.Timer;
            output.WriteLine("New Call Retrived");
            //update a property of the call
            call.Notes += "\nOrdered new drive!";
            //make sure update works 
            if(cdao.Update(call) == UpdateStatus.Ok)
            {
                output.WriteLine("Call was updated " + call.Notes);
            }
            else
            {
                output.WriteLine("Call was not updated!");
            }
            //test to make sure its stale if the timer is not working 
            call.Timer = oldtimer;
            call.Notes = "doesn't matter data is stale now";
            if(cdao.Update(call) == UpdateStatus.Stale) 
            {
                output.WriteLine("Call was not updated due to stale data");
            }
            cdao = new CallDAO();
            call = cdao.GetById(newCallId);
            if(cdao.Delete(newCallId) == 1)
            {
                output.WriteLine("Call was deleted!");
            }
            else
            {
                output.WriteLine("Call was not deleted!");
            }
            Assert.Null(cdao.GetById(newCallId));
        }
    }
}
