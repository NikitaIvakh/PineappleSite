namespace Identity.Domain.Enum;

public enum StatusCode
{
    Ok = 200,
    NotFound = 404,
    Deleted = 205,
    Created = 201,
    Modify = 203,
    NoAction = 204,
    InternalServerError = 500,
}