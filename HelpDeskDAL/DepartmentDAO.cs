/**
 * Class Name:DepartmentDAO.cs
 * Purpose: create the getAll that allow access to the database with the department information we need 
 *         will work in conjunction with one or more of the existing domain classes and act as a server to the ViewModel layer.
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/
using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace HelpDeskDAL
{
    public class DepartmentDAO
    {
        //only read-only operations allowed (we just want to populate our drop down menu) 
        readonly IRepository<Departments> repository;
        public DepartmentDAO()
        {
            //creates a "link" to the respository part of our data access layer
            repository = new HelpdeskRepository<Departments>();
        }

        //get a list of all the departments in the table (in our database) and return does
        public List<Departments> GetAll()
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
