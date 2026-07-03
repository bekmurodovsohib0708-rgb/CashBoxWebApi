using CashBox.Repository.Dtos.ProductDtos;
using CashBox.Service.Services;
using MediatR;
using Repository.Data;

namespace CashBox.Service.Applications.Products.Commands
{
    public record UpdateProductCommand(UpdateProductDto dto, int id) : IRequest<ProductDto>;

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly AppDbContext _context;
        private readonly AccountService _account;
        public UpdateProductHandler(AppDbContext context, AccountService account)
        {
            _context = context;
            _account = account;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.id, cancellationToken);

            if (product == null)
                throw new Exception($"{request.id} - mahsulot topilmadi");

            if (request.dto == null)
                throw new Exception("DTO bo'sh");

            if (!string.IsNullOrWhiteSpace(request.dto.Name))
                product.Name = request.dto.Name;
            if (!string.IsNullOrWhiteSpace(request.dto.Code))
                product.Code = request.dto.Code;
            if (request.dto.OrganizationId.HasValue)
                product.OrganizationId = request.dto.OrganizationId.Value;
            if (request.dto.DeliveredAt.HasValue)
                product.DeliveredAt = request.dto.DeliveredAt.Value;

            product.ModifiedAt = DateTime.UtcNow;
            product.ModifiedUserId = _account.UserId;

            await _context.SaveChangesAsync(cancellationToken);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                OrganizationId = product.OrganizationId,
                DeliveredAt = product.DeliveredAt,
            };
        }
    }
}
