using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;

services.AddControllers();

services.AddCors(options =>
{
    options.AddPolicy(name: "EliteCGCors",
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:5184", "http://localhost:4200")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                      });
});

services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Elite API",
        Description = "Used For GitHub RestFul API - Metrics"
        //Contact = new OpenApiContact
        //{
        //    Name = "Your Name XYZ",
        //    Email = "xyz@gmail.com",
        //    Url = new Uri("https://example.com"),
        //},
        //License = new OpenApiLicense
        //{
        //    Name = "Use under OpenApiLicense",
        //}
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Elite CG API - V1");
});

app.MapControllers();

app.UseCors("EliteCGCors");

app.Run();
