using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Applications.Products.Commands
{
    public record DeleteProductCommand(int id) : IRequest<string>;

    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, string>
    {
        private readonly AppDbContext _context;
        public DeleteProductHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(DeleteProductCommand req, CancellationToken cancel)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == req.id);

            if (product == null)
                throw new Exception($"{req.id} mahsulot topilmadi");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancel);

            return ($"{req.id} o'chirildi");
        }
    }
}
