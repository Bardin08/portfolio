using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Portfolio.Api.Endpoints;
using Portfolio.Api.Extensions.DependencyInjection;
using Portfolio.Api.MongoDb;
using Portfolio.Api.MongoDb.Repositories;
using Portfolio.Api.Tracking;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

AddMongoDb(builder.Services, builder.Configuration);

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

static void AddMongoDb(IServiceCollection services, IConfiguration configuration)
{
    BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

    services.Configure<MongoDbOptions>(configuration.GetSection(MongoDbOptions.SectionName));

    services.AddScoped<MongoDbContext>();
    services.AddScoped<IPixelEventsRepository, PixelEventsRepository>();
}