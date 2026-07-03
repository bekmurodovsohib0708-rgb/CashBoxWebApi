using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.UserDtos
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(200)]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(300)]
        public string ShortName { get; set; } = null!;

        [Required]
        [StringLength(14)]
        public string Pinfl { get; set; } = null!;

        [Required]
        [StringLength(9)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;
        public int OrganizationId { get; set; }

        [Required]
        [StringLength(10)]
        public string? DateOfBirth { get; set; }

        [Required]
        [StringLength(9)]
        public string PassportSeries { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;
    }
}
