/**
 * Class Name:EmployeeViewModel.cs
 * Purpose: Provide methods and state for a GUI application (View): provide data and functionality to be used by Views
 *           View -> presentation layer in HTML
 *           Client of the DAO in the DAL and a Server to the Controller in the website
 *           This gets the information from the employees table 
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/

using HelpDeskDAL; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;

namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        private readonly EmployeeDAO _dao;

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Timer { get; set; } //ensures no data is lost or modified during the transfer  
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int id { get; set; }
        public bool isTech { get; set; }
        public string StaffPicture64 { get; set; } 

        //constructor 
        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }

        //get employee information by email  
        public void GetByEmail()
        {
            try
            {
                Employees emp = _dao.GetByEmail(Email);
                Title = emp.Title;
                FirstName = emp.FirstName;
                LastName = emp.LastName;
                PhoneNo = emp.PhoneNo;
                Email = emp.Email;
                id = emp.Id;
                DepartmentID = emp.DepartmentId;
                isTech = emp.IsTech ?? false; 

                //if(emp.IsTech != null)
                //{
                //    isTech = Convert.ToBoolean(emp.IsTech);
                //}

                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                    
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Email = "notfound";
                throw nex;
            }
            catch (Exception ex)
            {
                Email = "notfound";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }


        //get employee information by lastName  
        public void GetByLastName()
        {
            try
            {
                Employees emp = _dao.GetByLastName(LastName);
                Title = emp.Title;
                FirstName = emp.FirstName;
                LastName = emp.LastName;
                PhoneNo = emp.PhoneNo;
                Email = emp.Email;
                id = emp.Id;
                DepartmentID = emp.DepartmentId;
                isTech = emp.IsTech ?? false;
                //if (emp.IsTech != null)
                //{
                //    isTech = Convert.ToBoolean(emp.IsTech);
                //}

                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);

                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Email = "notfound";
                throw nex;
            }
            catch (Exception ex)
            {
                Email = "notfound";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        //get employee information by providing an id 
        public void GetById()
        {
            try
            {
                Employees emp = _dao.GetById(id);
                Title = emp.Title;
                FirstName = emp.FirstName;
                LastName = emp.LastName;
                PhoneNo = emp.PhoneNo;
                Email = emp.Email;
                id = emp.Id;
                DepartmentID = emp.DepartmentId;

                if (emp.IsTech != null)
                {
                    isTech = Convert.ToBoolean(emp.IsTech);
                }

                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                id = 0;
                throw nex;
            }
            catch (Exception ex)
            {
                id = 0;
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        //get a list of all the employees in the database
        public List<EmployeeViewModel> GetAll()
        {
            List<EmployeeViewModel> allVms = new List<EmployeeViewModel>();
            try
            {
                List<Employees> allEmployees = _dao.GetAll();
                foreach (Employees emp in allEmployees)
                {
                    EmployeeViewModel empVm = new EmployeeViewModel();
                    empVm.Title = emp.Title;
                    empVm.FirstName = emp.FirstName;
                    empVm.LastName = emp.LastName;
                    empVm.PhoneNo = emp.PhoneNo;
                    empVm.Email = emp.Email;
                    empVm.id = emp.Id;
                    empVm.DepartmentID = emp.DepartmentId;
                    empVm.DepartmentName = emp.Department.DepartmentName;
                    empVm.isTech = Convert.ToBoolean(emp.IsTech);
                    empVm.Timer = Convert.ToBase64String(emp.Timer);
                    if (emp.StaffPicture != null)
                    {
                        empVm.StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                    }

                    allVms.Add(empVm);
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
        //adds a new employee
        public void Add()
        {
            id = -1;
            try
            {
                Employees emp = new Employees
                {
                    Title = Title,
                    FirstName = FirstName,
                    LastName = LastName,
                    PhoneNo = PhoneNo,
                    Email = Email,
                    DepartmentId = DepartmentID,
                    IsTech = isTech
                };
                if (StaffPicture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(StaffPicture64);
                }
                id = _dao.Add(emp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        //updates an existing employee
        public int Update()
        {
            UpdateStatus employeeUpdated = UpdateStatus.Failed;
            //int EmployeesUpdated = -1;
            try
            {
                Employees emp = new Employees
                {
                    Title = Title,
                    FirstName = FirstName,
                    LastName = LastName,
                    PhoneNo = PhoneNo,
                    Email = Email,
                    Id = id,
                    DepartmentId = DepartmentID,
                };

                if (emp.IsTech != null)
                {
                    isTech = Convert.ToBoolean(emp.IsTech);
                }

                if (StaffPicture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(StaffPicture64);
                }

                emp.Timer = Convert.FromBase64String(Timer);
                employeeUpdated = _dao.Update(emp);
                //EmployeesUpdated = _dao.Update(emp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(employeeUpdated);
            //return EmployeesUpdated;
        }

        //deletes an existing employee
        public int Delete()
        {
            int EmployeesDeleted = -1;

            try
            {
                EmployeesDeleted = _dao.Delete(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                  MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return EmployeesDeleted;
        }

    }
}

