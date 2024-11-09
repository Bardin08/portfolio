namespace Portfolio.Api.Models.Requests;

public record ServicesRequest(
    string Name,
    string Email,
    string Description,
    string Link
);