namespace EventsApiTests;

using WebApi.Controllers;
using WebApi.Services;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Models.Events;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;


public class EventsControllerTest
{
    IMapper _mapper;
    Moq.Mock<IEventService> _mockedEventService;
    public EventsControllerTest()
    {
        var myProfile = new EventMapper();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        _mapper = new Mapper(configuration);

        _mockedEventService = new Mock<IEventService>();
    }

    [Fact]
    public void GetById_ReturnsOkResult_WithExpectedValue()
    {
        // Arrange
        var fakeEventId = 1;

        var entity = CreateEvent(fakeEventId);

        _mockedEventService.Setup(service => service.GetById(fakeEventId)).Returns(entity);

        var controller = new EventsController(_mockedEventService.Object, _mapper);

        // Act
        var result = controller.GetById(fakeEventId);

        // Assert
        var parsed = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(parsed.Value, entity);
        _mockedEventService.Verify();
    }


    [Fact]
    public void Create_ReturnsCreatedAtActionResult_WithExpectedResult()
    {

        var request = CreateEventRequest();
        var entity = _mapper.Map<Event>(request);
        entity.Id = 1;

        _mockedEventService.Setup(service => service.Create(request)).Returns(entity);

        var controller = new EventsController(_mockedEventService.Object, _mapper);

        // Act
        var result = controller.Create(request);

        // Assert
        var parsed = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(parsed.Value, entity);
        _mockedEventService.Verify();
    }

    [Fact]
    public void GetAll_ReturnOkResult_WithExpectedValue()
    {

        var list = new List<Event>();
        list.Add(CreateEvent(1));
        list.Add(CreateEvent(2));

        _mockedEventService.Setup(service => service.GetAll()).Returns(list);

        var controller = new EventsController(_mockedEventService.Object, _mapper);

        // Act
        var result = controller.GetAll();

        // Assert
        var parsed = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(parsed.Value, list);
        _mockedEventService.Verify();
    }

    [Fact]
    public void Update_ReturnsOkResult_WithExpectedValue()
    {

        var request = CreateUpdateEventRequest();
        var id = 1;

        var entity = _mapper.Map<Event>(request);
        entity.Id = 1;

        _mockedEventService.Setup(service => service.Update(id, request)).Returns(entity);

        var controller = new EventsController(_mockedEventService.Object, _mapper);

        // Act
        var result = controller.Update(id, request);

        // Assert
        var parsed = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(parsed.Value, entity);
        _mockedEventService.Verify();
    }

    [Fact]
    public void Delete_ReturnsOkResult()
    {

        var id = 1;

       _mockedEventService.Setup(service => service.Delete(id));

        var controller = new EventsController(_mockedEventService.Object, _mapper);

        // Act
        var result = controller.Delete(id);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _mockedEventService.Verify();
    }

    private Event CreateEvent(int id)
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
