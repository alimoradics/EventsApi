using System.Text.Json.Serialization;
using WebApi.Helpers;
using WebApi.Services;
using WebApi.Entities;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddDbContext<EventContext>();
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    services.AddScoped<IEventService, EventService>();
    services.AddScoped<IDatabaseSeeder, SeederService>();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        options.EnableAnnotations();
        options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "basic",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Basic Authorization header."
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
    });


}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
    seeder.Seed();
}

{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseMiddleware<BasicAuthenticationMiddleware>();
    app.MapControllers();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}

app.Run();

public partial class Program { }

