using CashBox.Repository.Dtos.ProductDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Applications.Products.Queries
{
    public record GetByIdQuery(int id) : IRequest<ProductDto>;
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ProductDto>
    {
        private readonly AppDbContext _context;
        public GetByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.id);

            if (product == null)
                throw new Exception("Mahsulot topilmadi");

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                OrganizationId = product.OrganizationId,
                DeliveredAt = product.DeliveredAt
            };
        }
    }
}
