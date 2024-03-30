namespace Order.Domain.DTOs
{
    public record class GetUserDto(string UserId, string FirstName, string LastName, string UserName, string EmailAddress, IList<string> Role, DateTime? CreatedTime, DateTime? ModifiedTime);
}