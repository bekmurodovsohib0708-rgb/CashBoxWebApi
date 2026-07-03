using RepositoryLayer.Entity;

namespace CashBox.Repository.Dtos.OrganizationDtos
{
    public class OrganizationDto
    {
        public int Id { get; set; }
        public string Inn { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string RegionName { get; set; } = null!;
        public string DistrictName { get; set; } = null!;
        public int RegionId { get; set; }
        public int DistrictId { get; set; }
    }
}
