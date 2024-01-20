using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Interfaces;

namespace Product.Application.Features.Commands.Handlers
{
    public class UpdateProductDtoRequestHandler(IProductDbContext context, IMapper mapper) : IRequestHandler<UpdateProductDtoRequest, Unit>
    {
        private readonly IProductDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UpdateProductDtoRequest request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(key => key.Id == request.UpdateProduct.Id, cancellationToken) ??
                throw new Exception($"Продукта с идентификатором ({request.UpdateProduct.Id}) не существует");

            _mapper.Map(request.UpdateProduct, product);
            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}