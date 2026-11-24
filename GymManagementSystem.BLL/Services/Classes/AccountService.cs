using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem.BLL.Services.Classes;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<ApplicationUser?> GetUserByEmailAsync(string email)
        => _userManager.FindByEmailAsync(email);
}
