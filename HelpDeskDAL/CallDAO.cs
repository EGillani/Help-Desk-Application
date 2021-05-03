/**
 * Class Name:CallDAO.cs
 * Purpose: create the methods that allow access to the database.
 *         Will work in conjunction with one or more of the existing domain classes and act as a server to the ViewModel layer.
 * Coder: Eraj Gillani 0858887
 * Date: December 6th 2020
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
    public class CallDAO
    {
        readonly IRepository<Calls> repository;

        //creates a "link" to the respository part of our data access layer
        public CallDAO()
        {
            repository = new HelpdeskRepository<Calls>();
        }


        //passing in an id and searching for it in the database and returning the result 
        //does a catch if its unable to or fails 
        public Calls GetById(int id)
        {
            try
            {
                return repository.GetByExpression(c => c.Id == id).FirstOrDefault();
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
        public List<Calls> GetAll()
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

        //passes in a call object and adds it to the database using the add method and returns an employee id
        //does a catch if its unable to or fails 
        public int Add(Calls newCall)
        {
            try
            {
                newCall = repository.Add(newCall);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return newCall.Id;


        }
        //passes in a call object of the updated call info and updates information in the database 
        //does a catch if its unable to or fails 
        //returns the update status (if it worked our not) 
        public UpdateStatus Update(Calls updatedCall)
        {
            UpdateStatus operationStatus = UpdateStatus.Failed;

            try
            {
                operationStatus = repository.Update(updatedCall);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return operationStatus;

        }

        //passes in an id and deletes the call in the database that has that id 
        //does a catch if its unable to or fails 
        //returns the id of the call deleted 
        public int Delete(int id)
        {
            int callDeleted = -1;

            try
            {
                callDeleted = repository.Delete(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return callDeleted;


        }

    }
}
