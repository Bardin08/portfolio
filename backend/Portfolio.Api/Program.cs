using Portfolio.Api.Endpoints;
using Portfolio.Api.Extensions.DependencyInjection;
using Portfolio.Api.Workspaces.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentEmailServices();
builder.Services.AddGoogleServices(builder.Configuration);
builder.Services.AddScoped<WorkspaceService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthEndpoints();
app.MapPortfolioEndpoints();

app.Run();