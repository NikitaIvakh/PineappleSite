using AutoMapper;
using MediatR;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Interfaces;

namespace Product.Application.Features.Commands.Handlers
{
    public class DeleteProductDtoRequestHandler(IProductDbContext context, IMapper mapper) : IRequestHandler<DeleteProductDtoRequest, Unit>
    {
        private readonly IProductDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(DeleteProductDtoRequest request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.DeleteProduct.Id }, cancellationToken)
                ?? throw new Exception($"Продукт с идентификатором ({request.DeleteProduct.Id}) не существует");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}