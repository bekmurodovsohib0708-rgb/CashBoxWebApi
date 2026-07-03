using CashBox.Repository.Dtos.OrganizationDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.MiatR_pattern.Organizations.Queries
{
    public record GetIdQuery(int id) : IRequest<OrganizationDto>;
    public class GetByIdQueryHandler : IRequestHandler<GetIdQuery, OrganizationDto>
    {
        private readonly AppDbContext _context;
        public GetByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OrganizationDto> Handle(GetIdQuery request, CancellationToken cancellationToken)
        {
            var org = await _context.Organizations
                .Include(o => o.Region)
                .Include(o => o.District)
                .FirstOrDefaultAsync(o => o.Id == request.id, cancellationToken);

            if (org == null)
                throw new Exception($"{request.id} not fount");

            return new OrganizationDto
            {
                Id = org.Id,
                Inn = org.Inn,
                FullName = org.FullName,
                ShortName = org.ShortName,
                RegionId = org.RegionId,
                DistrictId = org.DistrictId,
                RegionName = org.Region.FullName,
                DistrictName = org.District.FullName
            };
        }
    }
}
