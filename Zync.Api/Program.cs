using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("schooljob", new OpenApiInfo { Version = "schooljob", Title = "School Job" });
    //c.SwaggerDoc("hospitaljob", new OpenApiInfo { Version = "hospitaljob", Title = "Hospital Job" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        //c.RouteTemplate = AppContext.BaseDirectory + "swagger/{documentname}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        //c.SwaggerEndpoint(AppContext.BaseDirectory + "swagger/schooljob/swagger.json", "School Job");
        //c.SwaggerEndpoint(AppContext.BaseDirectory + "swagger/hospitaljob/swagger.json", "Hospital Job");
        //c.RoutePrefix = AppContext.BaseDirectory + "swagger";
        // Add a second prefix if needed...
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
