using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem.DAL.Entities;


public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
