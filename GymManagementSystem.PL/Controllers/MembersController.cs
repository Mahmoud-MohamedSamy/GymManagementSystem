using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

public class MembersController : Controller
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    // GET /Members
    public IActionResult Index() => View(_memberService.GetAllMembers());

    // GET /Members/Create
    public IActionResult Create() => View();

    // POST /Members/CreateMember
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateMember(CreateMemberViewModel createMember)
    {
        if (!ModelState.IsValid)
            return View(nameof(Create), createMember);

        if (_memberService.CreateMember(createMember))
        {
            TempData["SuccessMessage"] = "Member Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty,
            "Member failed to create. The email or phone number may already be in use.");
        return View(nameof(Create), createMember);
    }

    // GET /Members/MemberDetails/5
    public IActionResult MemberDetails(int id)
    {
        var member = _memberService.GetMemberDetails(id);
        if (member is null) return MemberNotFound();

        return View(member);
    }

    // GET /Members/HealthRecordDetails/5
    public IActionResult HealthRecordDetails(int id)
    {
        var healthRecord = _memberService.GetMemberHealthRecordDetails(id);
        if (healthRecord is null) return MemberNotFound();

        return View(healthRecord);
    }

    // GET /Members/EditMember/5
    public IActionResult EditMember(int id)
    {
        var member = _memberService.GetMemberToUpdate(id);
        if (member is null) return MemberNotFound();

        return View(member);
    }

    // POST /Members/EditMember/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditMember(int id, MemberToUpdateViewModel memberToUpdate)
    {
        if (!ModelState.IsValid)
            return View(memberToUpdate);

        if (_memberService.UpdateMember(id, memberToUpdate))
        {
            TempData["SuccessMessage"] = "Member Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty,
            "Member failed to update. The email or phone number may already be in use.");
        return View(memberToUpdate);
    }

    // GET /Members/Delete/5  (confirmation page)
    public IActionResult Delete(int id)
    {
        if (_memberService.GetMemberToUpdate(id) is null) return MemberNotFound();

        return View();
    }

    // POST /Members/DeleteConfirmed/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (_memberService.RemoveMember(id))
            TempData["SuccessMessage"] = "Member Deleted Successfully";
        else
            TempData["ErrorMessage"] = "Member can't be deleted (not found, or still has active bookings)";

        return RedirectToAction(nameof(Index));
    }

    private IActionResult MemberNotFound()
    {
        TempData["ErrorMessage"] = "Member Not Found";
        return RedirectToAction(nameof(Index));
    }
}
