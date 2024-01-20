using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.DTOs.Products;
using Product.Application.Features.Requests.Queries;
using Product.Application.Interfaces;

namespace Product.Application.Features.Commands.Queries
{
    public class GetProductDetailsRequestHandler(IProductDbContext context, IMapper mapper) : IRequestHandler<GetProductDetailsRequest, ProductDto>
    {
        private readonly IProductDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ProductDto> Handle(GetProductDetailsRequest request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(key => key.Id == request.Id, cancellationToken);
            return _mapper.Map<ProductDto>(product);
        }
    }
}