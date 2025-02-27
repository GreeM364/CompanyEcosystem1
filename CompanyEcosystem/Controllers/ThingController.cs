﻿using System.Collections.Generic;
using System.IO;
using AutoMapper;
using CompanyEcosystem.BL.DataTransferObjects;
using CompanyEcosystem.BL.Infrastructure;
using CompanyEcosystem.BL.Interfaces;
using CompanyEcosystem.PL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEcosystem.PL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThingController : ControllerBase
    {
        private readonly IThingService _thingService;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IMapper _mapper;
        
        public ThingController(IThingService service, IWebHostEnvironment appEnvironment, IMapper mapper)
        {
            _thingService = service;
            _appEnvironment = appEnvironment;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var source = await _thingService.GetThings();

                var things = _mapper.Map<IEnumerable<ThingDto>, List<ThingViewModel>>(source);

                return Ok(things);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            try
            {
                var source = await _thingService.GetThing(id);

                var thing = _mapper.Map<ThingDto, ThingViewModel>(source);

                return Ok(thing);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ThingCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            if (model.Images == null)
            {
                BadRequest(model);
                return BadRequest("Added photographs");
            }

            try
            {
                var thingDto = _mapper.Map<ThingCreateUpdateViewModel, ThingDto>(model);

                var directoryPath = Path.Combine(_appEnvironment.WebRootPath, "img", "things");

                await _thingService.CreateThingAsync(thingDto, model.Images, directoryPath);

                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(ThingCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            try
            {
                var thingDto = _mapper.Map<ThingCreateUpdateViewModel, ThingDto>(model);

                var directoryPath = Path.Combine(_appEnvironment.WebRootPath, "img", "things");

                await _thingService.UpdateThingAsync(thingDto, model.Images, directoryPath);

                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                await _thingService.DeleteThingAsync(id);

                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
