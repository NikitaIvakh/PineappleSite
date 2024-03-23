namespace Identity.Domain.DTOs.Identities
{
    public record class GetAllUsersDto(string UserId, string FirstName, string LastName, string UserName, string EmailAddress, IList<string> Role, DateTime? CreatedTime, DateTime? ModifiedTime);
}