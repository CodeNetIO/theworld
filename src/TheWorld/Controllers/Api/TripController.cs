using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
	[Route("api/trips")]
	public class TripController : Controller
	{
	    private IWorldRepository _repository;
	    private ILogger<TripController> _logger;

	    public TripController(IWorldRepository repository, ILogger<TripController> logger)
	    {
	        _repository = repository;
	        _logger = logger;
	    }

		[HttpGet("")]
	    public JsonResult Get()
		{
		    var results = Mapper.Map<IEnumerable<TripViewModel>>(_repository.GetAllTripsWithStops());
	        return Json(results);
	    }

		[HttpPost("")]
	    public JsonResult Post([FromBody]TripViewModel vm)
		{
		    try
		    {
		        if (ModelState.IsValid)
		        {
		            var newTrip = Mapper.Map<Trip>(vm);

		            // save to the database
					_logger.LogInformation("Attempting to save a new trip");
		            _repository.AddTrip(newTrip);

		            if (_repository.SaveAll())
		            {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<TripViewModel>(newTrip));
                    }
		        }
		    }
		    catch (Exception ex)
		    {
		        _logger.LogError("Failed to save new trip", ex);
		        Response.StatusCode = (int) HttpStatusCode.BadRequest;
				return Json(new {Message = ex.Message});
		    }
		    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState});
            

        }
	}
}