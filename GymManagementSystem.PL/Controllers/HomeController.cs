using GymManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

public class HomeController : Controller
{
    private readonly IAnalyticsService _analyticsService;

    public HomeController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public IActionResult Index() => View(_analyticsService.GetAnalyticsData());

    public IActionResult Error() => View();
}
