namespace Product.Application.Exceptions
{
    public class NotFoundException(string message, object key) : ApplicationException($"{message} ({key}) не найдено")
    {

    }
}