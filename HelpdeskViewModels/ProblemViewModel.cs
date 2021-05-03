/**
 * Class Name:ProblemViewModel.cs
 * Purpose: Provide methods and state for a GUI application (View): provide data and functionality to be used by Views
 *           View -> presentation layer in HTML
 *           Client of the DAO in the DAL and a Server to the Controller in the website
 *           This gets the information from the departments table 
 * Coder: Eraj Gillani 0858887
 * Date: December 5th 2020
 **/


using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using HelpDeskDAL;

namespace HelpdeskViewModels
{
    public class ProblemViewModel
    {
        public string Description { get; set; }
        public int Id { get; set; }
        public string Timer { get; set; }

        readonly private ProblemDAO _dao;

        public ProblemViewModel()
        {
            _dao = new ProblemDAO();
        }
        public void GetByDescription()
        {
            try
            {
                Problems prob = _dao.GetByDescription(Description);
                Id = prob.Id;
                Timer = Convert.ToBase64String(prob.Timer);
               
            }
            catch (Exception ex)
            {
                Description = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        //gets a list of all the problems stored in the problems table in the database 
        public List<ProblemViewModel> GetAllProblems()
        {
            List<ProblemViewModel> allVms = new List<ProblemViewModel>();
            try
            {
                List<Problems> allProblems = _dao.GetAll();
                foreach (Problems prob in allProblems)
                {
                    ProblemViewModel probVm = new ProblemViewModel
                    {
                        Id = prob.Id,
                        Description = prob.Description,
                    };
                    allVms.Add(probVm);
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
