namespace WebApi.Controllers;

using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Events;
using WebApi.Services;
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class EventsController : ControllerBase
{
    private IEventService _eventService;
    private IMapper _mapper;

    public EventsController(
        IEventService eventService,
        IMapper mapper)
    {
        _eventService = eventService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var users = _eventService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(long id)
    {
        var user = _eventService.GetById(id);
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes(MediaTypeNames.Application.Json)]
    public IActionResult Create(CreateEventRequest model)
    {
        var entity = _eventService.Create(model);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(long id, UpdateEventRequest model)
    {
        var entity = _eventService.Update(id, model);
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        _eventService.Delete(id);
        return Ok(new { message = "Event deleted" });
    }
}
