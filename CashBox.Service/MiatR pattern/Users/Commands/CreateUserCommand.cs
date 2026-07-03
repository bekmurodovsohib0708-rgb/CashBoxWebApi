using CashBox.Repository.Dtos.UserDtos;
using MediatR;
using Repository.Data;
using RepositoryLayer.Entity;

namespace CashBox.Service.MiatR_pattern.Users.Commands
{
    public record CreateUserCommand(CreateUserDto dto) : IRequest<string>;
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly AppDbContext _context;
        public CreateUserHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                FullName = request.dto.FullName,
                ShortName = request.dto.ShortName,
                UserName = request.dto.UserName,
                Password = request.dto.Password,
                Pinfl = request.dto.Pinfl,
                PhoneNumber = request.dto.PhoneNumber,
                Address = request.dto.Address,
                OrganizationId = request.dto.OrganizationId,
                DateOfBirth = request.dto.DateOfBirth,
                PassportSeries = request.dto.PassportSeries,
                Email = request.dto.Email,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return ($"{user.Id} - user yaratildi");
        }
    }
}
