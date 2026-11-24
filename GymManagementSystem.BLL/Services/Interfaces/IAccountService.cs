using GymManagementSystem.DAL.Entities;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IAccountService
{
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
}
