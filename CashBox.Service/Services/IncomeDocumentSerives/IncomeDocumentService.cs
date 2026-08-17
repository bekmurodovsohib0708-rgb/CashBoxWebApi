using CashBox.Core;
using CashBox.Repository.Dtos.IncomeDocumentDtos;
using CashBox.Repository.Entity;
using CashBox.Service.Services.IncomeDocumentServes.QueryObjects;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Services.IncomeDocumentServices
{
    public class IncomeDocumentService : IIncomeDocumentService
    {
        private readonly AppDbContext _context;
        private readonly AccountService _account;
        public IncomeDocumentService(AppDbContext context, AccountService account)
        {
            _context = context;
            _account = account;
        }

        public async Task<List<IncomeDocumentListDto>> GetListAsync(IncomeDocumentFilterDto filter)
        {
            var result = await _context.IncomeDocuments
                .Where(x => x.StatusId != StatusIdConst.DELETE)
                .Select(u => new IncomeDocumentListDto
                {
                    Id = u.Id,
                    SupplierId = u.SupplierId,
                    SupplierName = u.Supplier.Code,
                    DocSum = u.DocSum,
                    DocOn = u.DocOn,
                    StatusName = u.Status.ShortName,
                    StatusId = u.StatusId
                }).SortFilter(filter)
            .ToListAsync();
            return result;
        }

        public async Task<IncomeDocumentDto> GetAsync(long id)
        {
            var incomeDocument = await _context.IncomeDocuments
                .Include(x => x.Tables)
                .Include(x => x.Supplier)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.StatusId != StatusIdConst.DELETE
                    && x.OrganizationId == _account.OrganizationId && x.Id == id);

            if (incomeDocument == null)
                throw new KeyNotFoundException();

            return new IncomeDocumentDto
            {
                Id = incomeDocument.Id,
                DocOn = incomeDocument.DocOn,
                SupplierId = incomeDocument.SupplierId,
                SupplierName = incomeDocument.Supplier.Code,
                DocSum = incomeDocument.DocSum,
                StatusName = incomeDocument.Status.ShortName,
                Tables = incomeDocument.Tables.Select(x => new IncomeDocumentTableDto
                {
                    Id = x.Id,
                    OwnerId = x.OwnerId,
                    ProductId = x.ProductId,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    TotalSum = x.TotalSum,
                }).ToList()
            };
        }

        public async Task<long> CreateAsync(CreateIncomeDocumentDlDto dto)
        {
            var entity = new IncomeDocument
            {
                DocOn = DateTime.SpecifyKind(dto.DocOn, DateTimeKind.Utc),
                SupplierId = dto.SupplierId,
                DocSum = dto.Tables.Sum(x => x.Price * x.Quantity),
                StatusId = StatusIdConst.CREATED,
                CreatedAt = DateTime.UtcNow,
                CreateUserId = _account.UserId,
                OrganizationId = _account.OrganizationId,

                Tables = dto.Tables.Select(x => new IncomeDocumentTable
                {
                    ProductId = x.ProductId,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    TotalSum = x.Price * x.Quantity,
                }).ToList()
            };
            await _context.IncomeDocuments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(UpdateIncomeDocumentDlDto dto)
        {
            var entity = await _context.IncomeDocuments
                .Include(x => x.Tables)
                .FirstOrDefaultAsync(x => x.StatusId != StatusIdConst.DELETE
                    && x.OrganizationId == _account.OrganizationId && x.Id == dto.Id);

            if (entity == null)
                throw new KeyNotFoundException($"{dto.Id} topilmadi");

            entity.SupplierId = dto.SupplierId;
            entity.DocOn = DateTime.SpecifyKind(dto.DocOn, DateTimeKind.Utc);
            entity.DocSum = dto.Tables.Sum(x => x.Price * x.Quantity);

            entity.StatusId = StatusIdConst.MODIFIED;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedUserId = _account.UserId;

            _context.IncomeDocumentTables.RemoveRange(entity.Tables);

            entity.Tables = dto.Tables.Select(x => new IncomeDocumentTable
            {
                OwnerId = entity.Id,
                ProductId = x.ProductId,
                Price = x.Price,
                Quantity = x.Quantity,
                TotalSum = x.Price * x.Quantity,
            }).ToList();

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.IncomeDocuments.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException();

            entity.StatusId = StatusIdConst.DELETE;
            await _context.SaveChangesAsync();
        }

        public async Task<long> Accept(long id)
        {
            var entity = await _context.IncomeDocuments.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException();

            entity.StatusId = StatusIdConst.ACCEPT;
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<long> NotAccept(long id)
        {
            var entity = await _context.IncomeDocuments.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException();

            entity.StatusId = StatusIdConst.NOT_ACCEPT;
            await _context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
