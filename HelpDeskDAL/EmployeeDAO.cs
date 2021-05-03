/**
 * Class Name:EmployeeDAO.cs
 * Purpose: create the methods that allow access to the database.
 *         Will work in conjunction with one or more of the existing domain classes and act as a server to the ViewModel layer.
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Buffers;

namespace HelpDeskDAL
{
    public class EmployeeDAO
    {
        readonly IRepository<Employees> repository;

        //creates a "link" to the respository part of our data access layer
        public EmployeeDAO()
        {
            repository = new HelpdeskRepository<Employees>();
        }

        //passing in an string and searching for it in the database and returning the result 
        //does a catch if its unable to or fails 
        public Employees GetByEmail(string email)
        {
            try
            {
                return repository.GetByExpression(emp => emp.Email == email).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        //passing in an string and searching for it in the database and returning the result 
        //does a catch if its unable to or fails 
        public Employees GetByLastName(string lastName)
        {
            try
            {
                return repository.GetByExpression(emp => emp.LastName == lastName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        //passing in an id and searching for it in the database and returning the result 
        //does a catch if its unable to or fails 
        public Employees GetById(int id)
        {
            try
            {
                return repository.GetByExpression(stu => stu.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        //returns all the employees in the database as a list  
        //does a catch if its unable to or fails 
        public List<Employees> GetAll()
        {
            try
            {
                return repository.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
          
        }
        //passes in a employee object and adds it to the database using the add method and returns an employee id
        //does a catch if its unable to or fails 
        public int Add(Employees newEmployee)
        {
            try
            {
                newEmployee = repository.Add(newEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return newEmployee.Id;

          
        }
        //passes in a employee object of the updated employee and updates information in the database 
        //does a catch if its unable to or fails 
        //returns the update status (if it worked our not) 
        public UpdateStatus Update(Employees updatedEmployee)
        {
            UpdateStatus operationStatus = UpdateStatus.Failed;

            try
            {
                operationStatus = repository.Update(updatedEmployee);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return operationStatus;

        }

        //passes in an id and deletes the employee in the database that has that id 
        //does a catch if its unable to or fails 
        //returns the id of the employee deleted 
        public int Delete(int id)
        {
            int employeeDeleted = -1;

            try
            {
                employeeDeleted = repository.Delete(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return employeeDeleted;

         
        }

    }
}
