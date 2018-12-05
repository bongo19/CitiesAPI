using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFirstApi.DataStores;
using MyFirstApi.Models;
using MyFirstApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFirstApi.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller
    {
        private ILogger<PointOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointOfInterestController(ILogger<PointOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }


        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            { 
                if (!_cityInfoRepository.CheckIfCityExists(cityId))
                {
                    _logger.LogInformation($"City with id of {cityId} was not found when accessing points of interest.");
                    return NotFound();
                }

                var pointsOfInterestForCity = _cityInfoRepository.GetPointOfInterestsForCity(cityId);
                var poiResults = Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity);

                return Ok(poiResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"City with id of {cityId} was not found when accessing points of interest.",ex);
                return StatusCode(500, "A problem happened while handling your request");
            }

        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CheckIfCityExists(cityId))
            {
                _logger.LogInformation($"City with id of {cityId} was not found when accessing points of interest.");
                return NotFound();
            }

            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

            if(pointOfInterest == null)
            {
                return NotFound();
            }

            var poiResults = Mapper.Map<PointOfInterestDto>(pointOfInterest);

            return Ok(poiResults);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId,[FromBody] PointOfInterestCreationDto pointOfInterestCreation)
        {
            if(pointOfInterestCreation == null)
            {
                return BadRequest();
            }

            if(pointOfInterestCreation.Name == pointOfInterestCreation.Description)
            {
                ModelState.AddModelError("Description", "Name of city and city description cannot be the same.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = _cityInfoRepository.CheckIfCityExists(cityId);

            if (!city)
            {
                return NotFound();
            }

            var finalPointOfInterest = Mapper.Map<Entities.PointOfInterest>(pointOfInterestCreation);

            _cityInfoRepository.AddPointOfInterestForCity(finalPointOfInterest, cityId);

            if (!_cityInfoRepository.SavePointOfInterest())
            {
                return StatusCode(500, "A problem happened while handling your request");
            }

            var createdPointOfInterestToReturn = Mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",new
            { cityId = cityId, id = finalPointOfInterest.Id}, createdPointOfInterestToReturn);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, 
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {

            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "Name of city and city description cannot be the same.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            

            if (!_cityInfoRepository.CheckIfCityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestFromEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

            if(pointOfInterestFromEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(pointOfInterest, pointOfInterestFromEntity);

            if (!_cityInfoRepository.SavePointOfInterest())
            {
                return StatusCode(500, "A problem happened while handling your request");
            }

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            if (!_cityInfoRepository.CheckIfCityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestFromEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestFromEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestFromEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Name == pointOfInterestToPatch.Description)
            {
                ModelState.AddModelError("Description", "Name of city and city description cannot be the same.");
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(pointOfInterestToPatch, pointOfInterestFromEntity);

            if (!_cityInfoRepository.SavePointOfInterest())
            {
                return StatusCode(500, "A problem happened while handling your request");
            }

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CheckIfCityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestFromEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestFromEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestFromEntity);

            if (!_cityInfoRepository.SavePointOfInterest())
            {
                return StatusCode(500, "A problem happened while handling your request");
            }

            _mailService.SendMessage("Point of interest deleted",
                $"Point of interest {pointOfInterestFromEntity.Name} with id {pointOfInterestFromEntity.Id} was deleted");

            return NoContent();

        }
    }
}
