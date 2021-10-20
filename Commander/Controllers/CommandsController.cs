using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Commander.Controllers
{
    [Route("api/commands")] // how you get to API endpoint
    [ApiController]  // gives default behavior
    public class CommandsController : ControllerBase
    {

        private readonly IServices _service;
        private readonly IMapper _mapper;

        public CommandsController(IServices service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET api/commands
        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetAllCommands()
        {
            var commandItems = _service.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        //GET api/commands/{id}
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _service.GetCommandById(id);
            if (commandItem != null)
            {
                // mapp from command to CommandReadDto
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }
            return NotFound();
        }

        // POST api/commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            // mapp from CommandCreateDto to Command
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _service.CreateCommand(commandModel);
            _service.SaveChanges();

            // mapp from command to CommandReadDto
            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            // nameof(GetCommandById) -> name of route 
            // new { Id = commandReadDto.Id } -> id number
            // commandReadDto -> returned body
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);

        }

        // PUT api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _service.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            // mapp from commandUpdateDto to commandModelFromRepo
            // updated commandModelFromRepo by mapping
            _mapper.Map(commandUpdateDto, commandModelFromRepo);
            _service.UpdateCommand(commandModelFromRepo);
            _service.SaveChanges();

            return NoContent();

        }

        // PATCH api/commands/{id}
        [HttpPatch("{id}")]

        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _service.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            // from command to CommandUpdateDto
            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);

            // applies changes to commandToPatch
            // ModelState takes care of validations
            patchDoc.ApplyTo(commandToPatch, ModelState);
            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            //update
            _mapper.Map(commandToPatch, commandModelFromRepo);
            _service.UpdateCommand(commandModelFromRepo);
            _service.SaveChanges();

            return NoContent();
        }

        // DELETE api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _service.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            _service.DeleteCommand(commandModelFromRepo);
            _service.SaveChanges();

            return NoContent();
        }
    }
}
