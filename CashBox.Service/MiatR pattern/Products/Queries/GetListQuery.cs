using CashBox.Repository.Dtos.ProductDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Applications.Products.Queries
{
    public record GetListQuery(ProductFilterDto dto) : IRequest<List<ProductListDto>>;

    public class GetlistHandler : IRequestHandler<GetListQuery, List<ProductListDto>>
    {
        private readonly AppDbContext _context;
        public GetlistHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductListDto>> Handle(GetListQuery req, CancellationToken cancel)
        {
            var list = await _context.Products
                .Select(x => new ProductListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    DeliveredAt = x.DeliveredAt
                }).ToListAsync();

            return list;
        }
    }
}
