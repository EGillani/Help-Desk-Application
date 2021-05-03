/**
 * Class Name:ProblemController.cs
 * Purpose: Type of REST service - transfers department information 
 *          between the client and server in order to progress the state of the applicaiton
 * Coder: Eraj Gillani 0858887
 * Date: December 11th 2020
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
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                ProblemViewModel viewmodel = new ProblemViewModel();
                List<ProblemViewModel> allProblems = viewmodel.GetAllProblems();
                return Ok(allProblems);
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
