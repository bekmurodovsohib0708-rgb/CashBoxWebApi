using CashBox.Repository.Dtos.OrganizationDtos;
using CashBox.Service.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using RepositoryLayer.Entity;

namespace CashBox.Service.MiatR_pattern.Organizations.Commands
{
    public record CreateCommand(CreateOrganizationDto dto) : IRequest<OrganizationDto>;
    public class CreateOrganizationHandler : IRequestHandler<CreateCommand, OrganizationDto>
    {
        private readonly AppDbContext _context;
        private readonly AccountService _account;
        public CreateOrganizationHandler(AppDbContext context, AccountService account)
        {
            _context = context;
            _account = account;
        }

        public async Task<OrganizationDto> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var org = new Organization
            {
                Inn = request.dto.Inn,
                FullName = request.dto.FullName,
                ShortName = request.dto.ShortName,
                RegionId = request.dto.RegionId,
                DistrictId = request.dto.DistrictId,
                CreatedAt = DateTime.UtcNow,
                CreateUserId = _account.UserId
            };

            await _context.Organizations.AddAsync(org);
            await _context.SaveChangesAsync();

            var result = await _context.Organizations
                .Include(o => o.Region)
                .Include(o => o.District)
                .FirstAsync(o => o.Id == org.Id, cancellationToken);

            return new OrganizationDto
            {
                Id = org.Id,
                Inn = org.Inn,
                FullName = org.FullName,
                ShortName = org.ShortName,
                RegionId = org.RegionId,
                DistrictId = org.DistrictId,
                DistrictName = result.Region.FullName,
                RegionName = result.Region.FullName
            };
        }
    }
}
