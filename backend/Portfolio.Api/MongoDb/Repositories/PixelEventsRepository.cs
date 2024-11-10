using MongoDB.Driver;
using Portfolio.Api.Analytics.Models;

namespace Portfolio.Api.MongoDb.Repositories;

internal class PixelEventsRepository(MongoDbContext dbContext) : IPixelEventsRepository
{
    private readonly IMongoCollection<PixelEvent> _collection =
        dbContext.GetCollection<PixelEvent>("portfolio.pixel.events");

    public async Task Add(PixelEvent @event) => await _collection.InsertOneAsync(@event);
}

internal interface IPixelEventsRepository
{
    Task Add(PixelEvent @event);
}