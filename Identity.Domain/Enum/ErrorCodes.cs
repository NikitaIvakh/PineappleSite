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
        InternalServerError = 500,
        AccountLoginError = 501,
        ErrorLoggingOutOfTheAccount = 502,
        RegistrationLoginError = 503,
        UserAlreadyExists = 504,
        ThisEmailAddressIsAlreadyExists = 505,
        LogoutError = 506,
    }
}