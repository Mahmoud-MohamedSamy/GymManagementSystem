using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers;

public class SessionsController : Controller
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    // GET /Sessions
    public IActionResult Index() => View(_sessionService.GetAllSessions());

    // GET /Sessions/Details/5
    public IActionResult Details(int id)
    {
        var result = _sessionService.GetSessionById(id);
        if (!result.IsSuccess) return Failed(result.Error);

        return View(result.Value);
    }

    // GET /Sessions/Create
    public IActionResult Create()
    {
        LoadDropDowns();
        return View();
    }

    // POST /Sessions/Create
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

    // GET /Sessions/Edit/5
    public IActionResult Edit(int id)
    {
        var result = _sessionService.GetSessionToUpdate(id);
        if (!result.IsSuccess) return Failed(result.Error);

        LoadTrainersDropDown();
        return View(result.Value);
    }

    // POST /Sessions/Edit/5
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

    // GET /Sessions/Delete/5  (confirmation page)
    public IActionResult Delete(int id)
    {
        var result = _sessionService.GetSessionById(id);
        if (!result.IsSuccess) return Failed(result.Error);

        return View();
    }

    // POST /Sessions/DeleteConfirmed/5
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
