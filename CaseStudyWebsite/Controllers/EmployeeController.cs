/**
 * Class Name:EmployeeController.cs
 * Purpose: Type of REST service - transfers employee information 
 *          between the client and server in order to progress the state of the applicaiton
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using HelpdeskViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudyWebsite.Controllers
{
    //how the client will call the methods 
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {   
        //gets the information of the employee with that email from the viewmodel
        [HttpGet("{email}")]
        public IActionResult GetByEmail(string email)
        {
            try
            {
                EmployeeViewModel viewModel = new EmployeeViewModel();
                viewModel.Email = email;
                viewModel.GetByEmail();
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);//something went wrong 
            }
        }

        //update the state (employee) on the server
        [HttpPut]
        public ActionResult Put(EmployeeViewModel viewmodel)
        {
            try
            {
                int retVal = viewmodel.Update();
                return retVal switch
                {
                    1 => Ok(new { msg = "Employee " + viewmodel.LastName + " updated!" }),
                    -1 => Ok(new { msg = "Employee " + viewmodel.LastName + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for " + viewmodel.LastName + ", Employee not updated!" }),
                    _ => Ok(new { msg = "Employee " + viewmodel.LastName + " not updated!" }),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        //gets all the employees and returns a list from the viewlist
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                EmployeeViewModel viewmodel = new EmployeeViewModel();
                List<EmployeeViewModel> allEmployee = viewmodel.GetAll();
                return Ok(allEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("problem in" + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //create a state on the server
        //add an employee in this case 
        [HttpPost]
        public ActionResult Post(EmployeeViewModel viewmodel)
        {
            try
            {
                viewmodel.Add();
                return viewmodel.id > 1
                ? Ok(new { msg = "Employee " + viewmodel.LastName + " added!" })
                : Ok(new { msg = "Employee " + viewmodel.LastName + " not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
              MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //deletes an employee from the database using the method in the viewmodel 
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                EmployeeViewModel viewModel = new EmployeeViewModel { id = id };
                return viewModel.Delete() == 1
                    ? Ok(new { msg = "Employee " + id + " deleted!" })
                    : Ok(new { msg = "Employee " + id + " not deleted!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
             MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
