namespace Identity.Domain.DTOs.Identities
{
    public record class GetUserDto(string UserId, string FirstName, string LastName, string UserName, string EmailAddress, DateTime? CreatedTime, DateTime? ModifiedTime);
}