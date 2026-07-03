using CashBox.Repository.Dtos.ProductDtos;
using CashBox.Repository.Entity;
using CashBox.Service.Services;
using MediatR;
using Repository.Data;

namespace CashBox.Service.Applications.Products.Commands
{
    public record CreateProductCommand(CreateProductDto dto) : IRequest<int>;

    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly AppDbContext _context;
        private readonly AccountService _account;
        public CreateProductHandler(AppDbContext context, AccountService account)
        {
            _context = context;
            _account = account;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (request.dto is null)
                throw new ArgumentNullException(nameof(request.dto));

            var orgId = _account.OrganizationId;

            if (orgId == 0)
                throw new UnauthorizedAccessException("OrganizationId topilmadi");

            if (request.dto.OrganizationId != orgId)
                throw new UnauthorizedAccessException("Boshqa tashkilot uchun mahsulot yarata olmaysiz");

            var product = new Product
            {
                Name = request.dto.Name,
                Code = request.dto.Code,
                OrganizationId = request.dto.OrganizationId,
                DeliveredAt = request.dto.DeliveredAt,

                CreatedAt = DateTime.UtcNow,
                CreatedUserId = _account.UserId
            };
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return (product.Id);
        }
    }
}
