using LitNovel.Application;
using LitNovel.Infrastructure;
using LitNovel.WebAPI;
using LitNovel.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddWebAPI();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
