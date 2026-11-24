using GymManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IAnalyticsService _analyticsService;

    public HomeController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public IActionResult Index() => View(_analyticsService.GetAnalyticsData());

    [AllowAnonymous]
    public IActionResult Error() => View();
}
