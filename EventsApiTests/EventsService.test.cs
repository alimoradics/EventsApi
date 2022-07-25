namespace EventsApiTests;

using WebApi.Controllers;
using WebApi.Services;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Models.Events;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


public class EventsServiceTest
{
    IMapper _mapper;
    Moq.Mock<EventContext> _mockedRepo;
    Moq.Mock<DbSet<Event>> _mockedSet;
    public EventsServiceTest()
    {
        var myProfile = new EventMapper();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        _mapper = new Mapper(configuration);

        _mockedRepo = new Mock<EventContext>();
        _mockedSet = new Mock<DbSet<Event>>();
    }

    [Fact]
    public void Create_CallsAddOnSet()
    {
        // Arrange

        var request = CreateEventRequest();

        _mockedRepo.Setup(repo => repo.Events).Returns(_mockedSet.Object);

        var service = new EventService(_mockedRepo.Object, _mapper);

        // Act
        var result = service.Create(request);

        // Assert
        Assert.IsType<Event>(result);
        _mockedRepo.Verify();
        _mockedSet.Verify(set => set.Add(It.IsAny<Event>()), Times.Once());
    }

    [Fact]
    public void Update_Throws_WhenSetIsEmpty()
    {
        // Arrange

        var request = CreateUpdateEventRequest();
        var id = 1;
        _mockedRepo.Setup(repo => repo.Events).Returns(_mockedSet.Object);

        var service = new EventService(_mockedRepo.Object, _mapper);

        // Act
        var act = () => service.Update(1, request);

        // Assert
        Assert.Throws<KeyNotFoundException>(act);
    }


    [Fact]
    public void Delete_Throws_WhenSetIsEmpty()
    {
        // Arrange

        var id = 1;
        _mockedRepo.Setup(repo => repo.Events).Returns(_mockedSet.Object);

        var service = new EventService(_mockedRepo.Object, _mapper);

        // Act
        var act = () => service.Delete(id);

        // Assert
        Assert.Throws<KeyNotFoundException>(act);
    }

    [Fact]
    public void GetById_Throws_WhenSetIsEmpty()
    {
        // Arrange

        var id = 1;

        _mockedRepo.Setup(repo => repo.Events).Returns(_mockedSet.Object);

        var service = new EventService(_mockedRepo.Object, _mapper);

        // Act
        var act = () => service.GetById(id);

        // Assert
        Assert.Throws<KeyNotFoundException>(act);
    }

    private Event CreateEvent(long id)
    {
        return new Event()
        {
            City = "fakeCity",
            Color = "fakeColor",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now,
            Price = 1.2M,
            Status = Status.Daily.ToString(),
            Id = id
        };
    }

    private CreateEventRequest CreateEventRequest()
    {
        return new CreateEventRequest()
        {
            City = "fakeCity",
            Color = "fakeColor",
            StartDate = "4/13/2013",
            EndDate = "4/13/2013",
            Price = "1.2",
            Status = Status.Daily.ToString()
        };
    }

    private UpdateEventRequest CreateUpdateEventRequest()
    {
        return new UpdateEventRequest()
        {
            City = "fakeCity",
            Color = "fakeColor",
            StartDate = "4/13/2013",
            EndDate = "4/13/2013",
            Price = "1.2",
            Status = Status.Daily.ToString()
        };
    }
}
