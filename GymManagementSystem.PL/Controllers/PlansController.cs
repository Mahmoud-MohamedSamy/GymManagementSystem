using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

public class PlansController : Controller
{
    private readonly IPlanService _planService;

    public PlansController(IPlanService planService)
    {
        _planService = planService;
    }

    // GET /Plans
    public IActionResult Index() => View(_planService.GetAllPlans());

    // GET /Plans/Details/1
    public IActionResult Details(int id)
    {
        var plan = _planService.GetPlanDetails(id);
        if (plan is null) return PlanNotFound();

        return View(plan);
    }

    // GET /Plans/Edit/1
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

    // POST /Plans/Edit/1
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

    // POST /Plans/Activate/1  (toggles IsActive - plans are never hard-deleted)
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
