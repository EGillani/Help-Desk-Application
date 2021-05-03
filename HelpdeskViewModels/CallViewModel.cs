/**
 * Class Name:CallViewModel.cs
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
    public class CallViewModel
    {
        readonly private CallDAO _dao;
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProblemId { get; set; }
        public string EmployeeName { get; set; }
        public string ProblemDescription { get; set; }
        public string TechName { get; set; }
        public int TechId { get; set; } //is employee technician (bit set to true?)
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; } //? means its an optional field 
        public bool OpenStatus { get; set; }
        public string Notes { get; set; }
        public string Timer { get; set; }

        public CallViewModel()
        {
            _dao = new CallDAO();
        }
        //get call information by providing an id 
        public void GetById()
        {
            try
            {
                Calls call = _dao.GetById(Id);
                EmployeeId = call.EmployeeId;
                EmployeeName = call.Employee.FirstName + " " + call.Employee.LastName; //ex of lazy loading doesnt work in debug mode
               //EmployeeName = new EmployeeDAO().GetById(call.EmployeeId).FirstName +" " + new EmployeeDAO().GetById(call.EmployeeId).LastName; //will work in debug mode
                ProblemId = call.ProblemId;
                ProblemDescription = call.Problem.Description;
                //ProblemDescription = new ProblemDAO().GetById(call.ProblemId).Description; 
                TechId = call.TechId;
                TechName = call.Tech.FirstName + " " + call.Tech.LastName;
                //TechName = new EmployeeDAO().GetById(call.TechId).FirstName + " " + new EmployeeDAO().GetById(call.TechId).LastName;
                DateOpened = call.DateOpened;
                DateClosed = call.DateClosed;
                OpenStatus = call.OpenStatus;
                Notes = call.Notes;

                Id = call.Id; 
                Timer = Convert.ToBase64String(call.Timer);
            }
            catch (Exception ex)
            {
                Id = 0;
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        public List<CallViewModel> GetAll()
        {
            List<CallViewModel> allVms = new List<CallViewModel>();
            try
            {
                List<Calls> allCalls = _dao.GetAll();
                foreach (Calls call in allCalls)
                {
                    CallViewModel callVm = new CallViewModel();
                    callVm.Id = call.Id;
                    callVm.TechId = call.TechId;
                    callVm.EmployeeId = call.EmployeeId;
                    callVm.ProblemId = call.ProblemId;
                    callVm.DateOpened = call.DateOpened;
                    callVm.DateClosed = call.DateClosed;
                    callVm.OpenStatus = call.OpenStatus;
                    callVm.Notes = call.Notes;
                    callVm.EmployeeName = call.Employee.FirstName + " " + call.Employee.LastName;
                    callVm.TechName = call.Tech.LastName;
                    callVm.ProblemDescription = call.Problem.Description;
                    callVm.Timer = Convert.ToBase64String(call.Timer);
                    allVms.Add(callVm);
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

        //adds a new call
        public void Add()
        {
            Id = -1;
            try
            {
                Calls call = new Calls
                {

                    EmployeeId = EmployeeId,
                    ProblemId = ProblemId,
                    TechId = TechId,
                    DateOpened = DateOpened,
                    DateClosed = DateClosed,
                    OpenStatus = OpenStatus,
                    Notes = Notes

                };
                Id = _dao.Add(call);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        //updates an existing call
        public int Update()
        {
            UpdateStatus callUpdated = UpdateStatus.Failed;
            try
            {
                Calls call = new Calls
                {
                    Id = Id,
                    EmployeeId = EmployeeId,
                    ProblemId = ProblemId,
                    TechId = TechId,
                    DateOpened = DateOpened,
                    DateClosed = DateClosed,
                    OpenStatus = OpenStatus,
                    Notes = Notes
                };

                call.Timer = Convert.FromBase64String(Timer);
                callUpdated = _dao.Update(call);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(callUpdated);
        }
        //deletes an existing call
        public int Delete()
        {
            int callDeleted = -1;

            try
            {
                callDeleted = _dao.Delete(Id);
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
