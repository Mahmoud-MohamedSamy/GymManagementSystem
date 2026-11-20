using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

public class TrainersController : Controller
{
    private readonly ITrainerService _trainerService;

    public TrainersController(ITrainerService trainerService)
    {
        _trainerService = trainerService;
    }

    // GET /Trainers
    public IActionResult Index() => View(_trainerService.GetAllTrainers());

    // GET /Trainers/Create
    public IActionResult Create() => View();

    // POST /Trainers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateTrainerViewModel createTrainer)
    {
        if (!ModelState.IsValid)
            return View(createTrainer);

        if (_trainerService.CreateTrainer(createTrainer))
        {
            TempData["SuccessMessage"] = "Trainer Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty,
            "Trainer failed to create. The email or phone number may already be in use.");
        return View(createTrainer);
    }

    // GET /Trainers/Details/5
    public IActionResult Details(int id)
    {
        var trainer = _trainerService.GetTrainerDetails(id);
        if (trainer is null) return TrainerNotFound();

        return View(trainer);
    }

    // GET /Trainers/Edit/5
    public IActionResult Edit(int id)
    {
        var trainer = _trainerService.GetTrainerToUpdate(id);
        if (trainer is null) return TrainerNotFound();

        return View(trainer);
    }

    // POST /Trainers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, TrainerToUpdateViewModel trainerToUpdate)
    {
        if (!ModelState.IsValid)
            return View(trainerToUpdate);

        if (_trainerService.UpdateTrainer(id, trainerToUpdate))
        {
            TempData["SuccessMessage"] = "Trainer Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty,
            "Trainer failed to update. The email or phone number may already be in use.");
        return View(trainerToUpdate);
    }

    // GET /Trainers/Delete/5  (confirmation page)
    public IActionResult Delete(int id)
    {
        if (_trainerService.GetTrainerToUpdate(id) is null) return TrainerNotFound();

        return View();
    }

    // POST /Trainers/DeleteConfirmed/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (_trainerService.RemoveTrainer(id))
            TempData["SuccessMessage"] = "Trainer Deleted Successfully";
        else
            TempData["ErrorMessage"] = "Trainer can't be deleted (not found, or still has sessions)";

        return RedirectToAction(nameof(Index));
    }

    private IActionResult TrainerNotFound()
    {
        TempData["ErrorMessage"] = "Trainer Not Found";
        return RedirectToAction(nameof(Index));
    }
}
