using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers;

[Authorize]
public class SessionsController : Controller
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public IActionResult Index() => View(_sessionService.GetAllSessions());

    public IActionResult Details(int id)
    {
        var result = _sessionService.GetSessionById(id);
        if (!result.IsSuccess) return Failed(result.Error);

        return View(result.Value);
    }

    public IActionResult Create()
    {
        LoadDropDowns();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateSessionViewModel createSession)
    {
        if (!ModelState.IsValid)
        {
            LoadDropDowns();
            return View(createSession);
        }

        var result = _sessionService.CreateSession(createSession);
        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Session Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Error!);
        LoadDropDowns();
        return View(createSession);
    }

    public IActionResult Edit(int id)
    {
        var result = _sessionService.GetSessionToUpdate(id);
        if (!result.IsSuccess) return Failed(result.Error);

        LoadTrainersDropDown();
        return View(result.Value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, UpdateSessionViewModel updateSession)
    {
        if (!ModelState.IsValid)
        {
            LoadTrainersDropDown();
            return View(updateSession);
        }

        var result = _sessionService.UpdateSession(id, updateSession);
        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Session Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Error!);
        LoadTrainersDropDown();
        return View(updateSession);
    }

    public IActionResult Delete(int id)
    {
        var result = _sessionService.GetSessionById(id);
        if (!result.IsSuccess) return Failed(result.Error);

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var result = _sessionService.RemoveSession(id);
        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Session Deleted Successfully";
        else
            TempData["ErrorMessage"] = result.Error;

        return RedirectToAction(nameof(Index));
    }

    private IActionResult Failed(string? error)
    {
        TempData["ErrorMessage"] = error;
        return RedirectToAction(nameof(Index));
    }

    private void LoadDropDowns()
    {
        LoadTrainersDropDown();
        ViewBag.Categories = new SelectList(_sessionService.GetCategoriesForDropDown(), "Id", "Name");
    }

    private void LoadTrainersDropDown()
    {
        ViewBag.Trainers = new SelectList(_sessionService.GetTrainersForDropDown(), "Id", "Name");
    }
}
