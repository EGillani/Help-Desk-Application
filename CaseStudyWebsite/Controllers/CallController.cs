/**
 * Class Name:CallController.cs
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
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        //gets the information of the call by id from the viewmodel
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                CallViewModel viewModel = new CallViewModel();
                viewModel.Id = id;
                viewModel.GetById();
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);//something went wrong 
            }
        }

        //update the state (call) on the server
        [HttpPut]
        public ActionResult Put(CallViewModel viewmodel)
        {
            try
            {
                int retVal = viewmodel.Update();
                return retVal switch
                {
                    1 => Ok(new { msg = "Call " + viewmodel.Id + " updated!" }),
                    -1 => Ok(new { msg = "Call " + viewmodel.Id + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for " + viewmodel.Id + ", Call not updated!" }),
                    _ => Ok(new { msg = "Call " + viewmodel.Id + " not updated!" }),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        //gets all the calls and returns a list from the viewlist
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                CallViewModel viewmodel = new CallViewModel();
                List<CallViewModel> allCalls = viewmodel.GetAll();
                return Ok(allCalls);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("problem in" + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //create a state on the server
        //add a a call in this case 
        [HttpPost]
        public ActionResult Post([FromBody] CallViewModel viewmodel)
        {
            try
            {
                viewmodel.Add();
                return viewmodel.Id > 1
                ? Ok(new { msg = "Call " + viewmodel.Id + " added!" })
                : Ok(new { msg = "Call " + viewmodel.Id + " not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
              MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //deletes a call from the database using the method in the viewmodel 
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                CallViewModel viewModel = new CallViewModel { Id = id };
                return viewModel.Delete() == 1
                    ? Ok(new { msg = "Call " + id + " deleted!" })
                    : Ok(new { msg = "Call " + id + " not deleted!" });
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
