using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.DTOs.Products;
using Product.Application.Features.Requests.Queries;
using Product.Application.Interfaces;

namespace Product.Application.Features.Commands.Queries
{
    public class GetProductListRequestHandler(IProductDbContext context, IMapper mapper) : IRequestHandler<GetProductListRequest, IReadOnlyCollection<ProductDto>>
    {
        private readonly IProductDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<IReadOnlyCollection<ProductDto>> Handle(GetProductListRequest request, CancellationToken cancellationToken)
        {
            var products = await _context.Products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return _mapper.Map<IReadOnlyCollection<ProductDto>>(products);
        }
    }
}