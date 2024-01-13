namespace Coupon.Application.Exceptions
{
    public class NotFoundException(string name, object key) : ApplicationException($"{name} ({key}) не найдено!")
    {

    }
}