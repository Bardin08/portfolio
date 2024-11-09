using Portfolio.Api.Configuration;
using Portfolio.Api.Endpoints;
using Portfolio.Api.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

builder.Services.AddCors(corsOptions =>
    corsOptions.AddPolicy("CorsPolicy",
        policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentEmailServices(builder.Configuration);
// builder.Services.AddGoogleServices(builder.Configuration);
// builder.Services.AddScoped<WorkspaceService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.MapHealthEndpoints();
app.MapPortfolioEndpoints();

app.Run();