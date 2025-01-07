using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TelgeProject.Models
{
    public class addUserViewModel
    {
        public int userId { get; set; }
        //[Required(ErrorMessage = "Full Name is required.")]
        public string? FullName { get; set; }

        //[Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? Email { get; set; }

        //[Required(ErrorMessage = "Password is required.")]
        //[MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? PasswordHash { get; set; }

        //[Required(ErrorMessage = "Contact number is required.")]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Contact number must be exactly 10 digits.")]
        public string? Contact { get; set; }

        //[Required(ErrorMessage = "Role is required.")]
        public int? Role { get; set; }
        public int ReportingPerson { get; set; }
        public int FkUserId { get; set; }

        //[Required(ErrorMessage = "Responsibilities are required.")]
        public string? Responsibilities { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ReportingPersonList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Responsibilitieslist { get; set; } = new List<SelectListItem>();
    }
}
