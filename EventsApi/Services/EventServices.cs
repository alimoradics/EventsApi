namespace WebApi.Services;

using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Events;

public interface IEventService
{
    IEnumerable<Event> GetAll();
    Event GetById(long id);
    Event Create(CreateEventRequest model);
    Event Update(long id, UpdateEventRequest model);
    void Delete(long id);
}

public class EventService : IEventService
{
    private EventContext _context;
    private readonly IMapper _mapper;

    public EventService(
        EventContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<Event> GetAll()
    {
        return _context.Events;
    }

    public Event GetById(long id)
    {
        return getEvent(id);
    }

    public Event Create(CreateEventRequest model)
    {

        var eventEntity = _mapper.Map<Event>(model);
        _context.Events.Add(eventEntity);
        _context.SaveChanges();
        return eventEntity;
    }

    public Event Update(long id, UpdateEventRequest model)
    {
        var eventEntity = getEvent(id);

        _mapper.Map(model, eventEntity);
        _context.Events.Update(eventEntity);
        _context.SaveChanges();

        return eventEntity;
    }

    public void Delete(long id)
    {
        var eventEntity = getEvent(id);
        _context.Events.Remove(eventEntity);
        _context.SaveChanges();
    }

    // helper methods

    private Event getEvent(long id)
    {
        var eventEntity = _context.Events.Find(id);
        if (eventEntity == null) throw new KeyNotFoundException("Event not found");
        return eventEntity;
    }
}