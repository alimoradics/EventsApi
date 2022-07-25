namespace WebApi.Models;

using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Events;
using System.Globalization;

public class EventMapper : Profile
{
	public EventMapper()
	{
		CreateMap<CreateEventRequest, Event>()
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => this.ConverToDate(src.StartDate)))
			.ForMember(dest => dest.Price, opt => opt.MapFrom(src => Convert.ToDecimal(src.Price)))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => this.ConverToDate(src.EndDate)));

		CreateMap<UpdateEventRequest, Event>()
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => this.ConverToDate(src.StartDate)))
			.ForMember(dest => dest.Price, opt => opt.MapFrom(src => Convert.ToDecimal(src.Price)))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => this.ConverToDate(src.EndDate)));
	}

	private DateTime ConverToDate(string dateString)
	{
		return DateTime.ParseExact(dateString, "M/d/yyyy", CultureInfo.InvariantCulture);
	}
}
