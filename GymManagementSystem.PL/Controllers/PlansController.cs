using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

[Authorize]
public class PlansController : Controller
{
    private readonly IPlanService _planService;

    public PlansController(IPlanService planService)
    {
        _planService = planService;
    }

    public IActionResult Index() => View(_planService.GetAllPlans());

    public IActionResult Details(int id)
    {
        var plan = _planService.GetPlanDetails(id);
        if (plan is null) return PlanNotFound();

        return View(plan);
    }

    public IActionResult Edit(int id)
    {
        var plan = _planService.GetPlanToUpdate(id);
        if (plan is null)
        {
            TempData["ErrorMessage"] = "Plan can't be edited (not found, or it has active memberships)";
            return RedirectToAction(nameof(Index));
        }

        return View(plan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, UpdatePlanViewModel planToUpdate)
    {
        if (!ModelState.IsValid)
            return View(planToUpdate);

        if (_planService.UpdatePlan(id, planToUpdate))
        {
            TempData["SuccessMessage"] = "Plan Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty,
            "Plan failed to update. It may have active memberships.");
        return View(planToUpdate);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Activate(int id)
    {
        if (_planService.ToggleStatus(id))
            TempData["SuccessMessage"] = "Plan Status Changed";
        else
            TempData["ErrorMessage"] = "Failed To Change Plan Status (it may have active memberships)";

        return RedirectToAction(nameof(Index));
    }

    private IActionResult PlanNotFound()
    {
        TempData["ErrorMessage"] = "Plan Not Found";
        return RedirectToAction(nameof(Index));
    }
}
