/**
 * Class Name:DepartmentController.cs
 * Purpose: Type of REST service - transfers department information 
 *          between the client and server in order to progress the state of the applicaiton
 * Coder: Eraj Gillani 0858887
 * Date: November 12th 2020
 **/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelpdeskViewModels;

namespace CaseStudyWebsite.Controllers
{
    //how the client will call the methods 
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        //returns a list of all the departments in the database 
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                DepartmentViewModel viewmodel = new DepartmentViewModel();
                List<DepartmentViewModel> allEmployees = viewmodel.GetAll();
                return Ok(allEmployees);
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
