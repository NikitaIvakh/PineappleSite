namespace Product.Domain.Enum
{
    public enum ErrorCodes
    {
        ProductAlreadyExists = 403,
        ProductNotFound = 404,
        ProductsNotFound = 405,
        ProductNotDeleted = 406,
        ProductsNotDeleted = 407,
        ProductNotCreated = 408,
        ProductNotUpdated = 304,
        ProductNotUpdatedNull = 305,
        InternalServerError = 500,
    }
}