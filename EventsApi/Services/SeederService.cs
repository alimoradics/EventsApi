namespace WebApi.Services;
using WebApi.Entities;
using WebApi.Models.Events;
using AutoMapper;
using System.Text.Json;

public interface IDatabaseSeeder
{
	void Seed();
}
public class SeederService : IDatabaseSeeder
{

	private EventContext _context;
	private readonly IMapper _mapper;

	public SeederService(
		EventContext context,
		IMapper mapper)
	{
		_context = context;
		_mapper = mapper;


	}

	public void Seed()
	{
		string eventsJson = System.IO.File.ReadAllText(@"Data" + Path.DirectorySeparatorChar + "data.json");
		List<CreateEventRequest> eventDtos = JsonSerializer.Deserialize<List<CreateEventRequest>>(eventsJson);
		List<Event> events = _mapper.Map<List<Event>>(eventDtos);

		_context.AddRange(events);
		_context.SaveChanges();
	}
}