/**
 * Class Name:DepartmentViewModel.cs
 * Purpose: Provide methods and state for a GUI application (View): provide data and functionality to be used by Views
 *           View -> presentation layer in HTML
 *           Client of the DAO in the DAL and a Server to the Controller in the website
 *           This gets the information from the departments table 
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/


using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using HelpDeskDAL;


namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Timer { get; set; }

        readonly private DepartmentDAO _dao;

        public DepartmentViewModel()
        {
            _dao = new DepartmentDAO();
        }

        //gets a list of all the departments stored in the departments table in the database 
        public List<DepartmentViewModel> GetAll()
        {
            List<DepartmentViewModel> allVms = new List<DepartmentViewModel>();
            try
            {
                List<Departments> allDivisions = _dao.GetAll();
                foreach (Departments div in allDivisions)
                {
                    DepartmentViewModel divVm = new DepartmentViewModel
                    {
                        Id = div.Id,
                        Name = div.DepartmentName,
                    };
                    allVms.Add(divVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return allVms;
        }
    }
}
