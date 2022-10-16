﻿using AutoMapper;
using CompanyEcosystem.BL.DataTransferObjects;
using CompanyEcosystem.BL.Infrastructure;
using CompanyEcosystem.BL.Interfaces;
using CompanyEcosystem.DAL.Entities;
using CompanyEcosystem.DAL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CompanyEcosystem.BL.Services
{
    public class ThingService : IThingService
    {
        private readonly IRepository<Thing> _repositoryThing;
        private readonly IRepository<PhotoThing> _repositoryPhoto;
        private readonly IMapper _mapper;
        
        public ThingService(IRepository<Thing> repositoryThing, IRepository<PhotoThing> repositoryPhoto,IMapper mapper)
        {
            _repositoryThing = repositoryThing;
            _repositoryPhoto = repositoryPhoto;
            _mapper = mapper;
        }

        public IEnumerable<ThingDto> GetThings()
        {
            var things = _repositoryThing.GetAll();
            if (things.Count() == 0)
                throw new ValidationException("Things not found", "");

<<<<<<< HEAD
            return _mapper.Map<IEnumerable<Thing>, List<ThingDto>>(_repository.GetAll());
=======
            return _mapper.Map<IEnumerable<Thing>, List<ThingDTO>>(_repositoryThing.GetAll());
>>>>>>> Test
        }

        public ThingDto GetThing(int? id)
        {
            if (id == null)
                throw new ValidationException("Thing ID not set", "");
            var thing = _repositoryThing.Get(id.Value);
            if (thing == null)
                throw new ValidationException("Thing not found", "");

            return _mapper.Map<Thing, ThingDto>(thing);
        }

<<<<<<< HEAD
        public void CreateThing(ThingDto thingDto)
        {
            var thing = _mapper.Map<ThingDto, Thing>(thingDto);
=======
        public void CreateThing(ThingDTO thingDto, IFormFileCollection formFileCollection, string directoryPath)
        {
            var thing = _mapper.Map<ThingDTO, Thing>(thingDto);
            _repositoryThing.Create(thing);
>>>>>>> Test

            if (formFileCollection != null && formFileCollection.Any() && !string.IsNullOrWhiteSpace(directoryPath))
            {
                directoryPath = Path.Combine(directoryPath, thing.Id.ToString());
            }

            foreach (var uploadedImage in formFileCollection)
            {

                var path = $"/img/things/{thingDto.Id}/{uploadedImage.FileName}";

                using (var fileStream = new FileStream(Path.Combine(directoryPath, uploadedImage.FileName), FileMode.Create))
                {
                    uploadedImage.CopyToAsync(fileStream);
                }

                var photo = _mapper.Map<PhotoThingDTO, PhotoThing>(new PhotoThingDTO { ThingId = thing.Id, Path = path });
                _repositoryPhoto.Create(photo);
            }
        }

        public void UpdateThing(ThingDto thingDto)
        {
            var thing = _mapper.Map<ThingDto, Thing>(thingDto);

            _repositoryThing.Update(thing);
        }

        public void DeleteThing(int? id)
        {
            if (id == null)
                throw new ValidationException("Thing ID not set", "");

            _repositoryThing.Delete(id.Value);
        }
    }
}
