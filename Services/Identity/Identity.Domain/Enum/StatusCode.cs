namespace Identity.Domain.Enum;

public enum StatusCode
{
    Ok = 200,
    Created = 201,
    Modify = 203,
    NoAction = 204,
    Deleted = 205,
    Refreshed = 206,
    NotFound = 404,
    InternalServerError = 500,
}