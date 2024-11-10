namespace Portfolio.Api.MongoDb;

internal record MongoDbOptions
{
    public static string SectionName => "MongoDb";

    public required string ConnectionString { get; init; }
    public required string DatabaseName { get; init; }
}