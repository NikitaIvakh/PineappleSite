namespace Identity.Domain.Enum
{
    public enum ErrorCodes
    {
        UserNotFound = 404,
        UsersNotFound = 405,
        UsersConNotDeleted = 406,
        UserCanNotDeleted = 407,
        UpdateProdileError = 408,
        UserUpdateError = 409,
        UserCanNotBeCreated = 410,
        EmailAddressAlreadyExists = 411,
        InternalServerError = 500,
        AccountLoginError = 501,
        ErrorLoggingOutOfTheAccount = 502,
        RegistrationLoginError = 503,
        UserAlreadyExists = 504,
        ThisEmailAddressIsAlreadyExists = 505,
        LogoutError = 506, 
        LoginOrPasswordIsNotValid = 507,
    }
}