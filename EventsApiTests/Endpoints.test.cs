namespace EventsApiTests;

using WebApi.Controllers;
using WebApi.Services;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Models.Events;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using System.Text;
using FluentAssertions;
using FluentAssertions.Extensions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

public class EndpointsTest : IClassFixture<WebApplicationFactory<Program>>
{
    IMapper _mapper;
    HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    
    public EndpointsTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
       

        var myProfile = new EventMapper();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        _mapper = new Mapper(configuration);
    }

    [Fact]
    public async void GET_events()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.SendAsync(GetAuthenticatedRequestMessage(HttpMethod.Get, "/events"));
        Console.Out.WriteLine(response);
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }

    [Fact]
    public async void GET_events_WithWrongAuthentication_ReturnsStatusCode401()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.SendAsync(GetAuthenticatedRequestMessage(HttpMethod.Get, "/events", "wrong", "wrong"));
        Console.Out.WriteLine(response);
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

    }

    [Fact]
    public async void GET_events_WithNoAuthentication_ReturnsStatusCode400()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/events");
        Console.Out.WriteLine(response);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }

    [Fact]
    public async void POST_Event()
    {
        // Arrange
        var request = CreateEventRequest();
        var requestJson = JsonSerializer.Serialize(request);
        var expected = _mapper.Map<Event>(request);

        var requestMessage = GetAuthenticatedRequestMessage(HttpMethod.Post, "/events");
        requestMessage.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.SendAsync(requestMessage);
        var responseContent = JsonSerializer.Deserialize<Event>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        expected.Should().BeEquivalentTo(responseContent, options =>
            options.Excluding(o => o.Id));
    }

    [Fact]
    public async void POST_Event_WithInvalidBody_ReturnsStatusCode400()
    {
        // Arrange
        var request = CreateEventRequest();
        request.City = null;
        var requestJson = JsonSerializer.Serialize(request);
        var expected = _mapper.Map<Event>(request);

        var requestMessage = GetAuthenticatedRequestMessage(HttpMethod.Post, "/events");
        requestMessage.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.SendAsync(requestMessage);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
    }

    [Fact]
    public async void PUT_Event_By_Id()
    {
        // Arrange
        var request = CreateUpdateEventRequest();
        var requestJson = JsonSerializer.Serialize(request);
        var expected = _mapper.Map<Event>(request);

        var requestMessage = GetAuthenticatedRequestMessage(HttpMethod.Put, "/events/1");
        requestMessage.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.SendAsync(requestMessage);
        var responseContent = JsonSerializer.Deserialize<Event>(await response.Content.ReadAsStringAsync());

        var getResponse = await _client.SendAsync(GetAuthenticatedRequestMessage(HttpMethod.Get, "/events/1"));
        var getResponseContent = JsonSerializer.Deserialize<Event>(await getResponse.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        expected.Should().BeEquivalentTo(getResponseContent, options =>
            options.Excluding(o => o.Id));
        Assert.Equal(1, getResponseContent.Id);
    }

    [Fact]
    public async void GET_Event_By_Id()
    {
        // Act
        var response = await _client.SendAsync(GetAuthenticatedRequestMessage(HttpMethod.Get, "/events/1"));
        var responseContent = JsonSerializer.Deserialize<Event>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, responseContent.Id);
    }

    [Fact]
    public async void DELETE_Event_By_Id()
    {
        // Act
        var response = await _client.SendAsync(GetAuthenticatedRequestMessage(HttpMethod.Delete, "/events/1"));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private HttpRequestMessage GetAuthenticatedRequestMessage(HttpMethod method, string requestUri)
    {
        var requestMessage = new HttpRequestMessage(method, requestUri);
        var authenticationString = "test:test";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

        return GetAuthenticatedRequestMessage(method, requestUri, "test", "test");
    }

    private HttpRequestMessage GetAuthenticatedRequestMessage(HttpMethod method, string requestUri, string username, string password)
    {
        var requestMessage = new HttpRequestMessage(method, requestUri);
        var authenticationString = $"{username}:{password}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

        return requestMessage;
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
