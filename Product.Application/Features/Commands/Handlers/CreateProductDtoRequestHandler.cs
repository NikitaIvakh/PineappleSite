using AutoMapper;
using MediatR;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Interfaces;
using Product.Core.Entities.Producrs;

namespace Product.Application.Features.Commands.Handlers
{
    public class CreateProductDtoRequestHandler(IProductDbContext context, IMapper mapper) : IRequestHandler<CreateProductDtoRequest, int>
    {
        private readonly IProductDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<int> Handle(CreateProductDtoRequest request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<ProductEntity>(request.CreateProduct);

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}