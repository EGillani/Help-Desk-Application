/**
 * Class Name:ProblemDAO.cs
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
    public class ProblemDAO
    {
        readonly IRepository<Problems> repository;
        //creates a "link" to the respository part of our data access layer
        public ProblemDAO()
        {
            repository = new HelpdeskRepository<Problems>();
        }
        //passing in an int and searching for it in the database and returning the result 
        //does a catch if its unable to or fails 
        public Problems GetById(int id)
        {
            try
            {
                return repository.GetByExpression(prob => prob.Id == id).FirstOrDefault();
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
        public Problems GetByDescription(string description)
        {
            try
            {
                return repository.GetByExpression(prob => prob.Description == description).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        //returns all the descriptions in the database as a list  
        //does a catch if its unable to or fails 
        public List<Problems> GetAll()
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
    }
}
