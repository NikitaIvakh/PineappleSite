using Microsoft.AspNetCore.Http;

namespace Identity.Domain.DTOs.Identities;

public sealed record UpdateUserProfileDto(
    string Id,
    string? FirstName,
    string? LastName,
    string? EmailAddress,
    string? UserName,
    string? Password,
    string? Description,
    int? Age,
    IFormFile? Avatar,
    string? ImageUrl,
    string? ImageLocalPath
);