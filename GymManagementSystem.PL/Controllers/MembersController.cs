using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class MembersController : Controller
{
    private readonly IMemberService _memberService;
    private readonly IAttachmentService _attachmentService;

    public MembersController(IMemberService memberService, IAttachmentService attachmentService)
    {
        _memberService = memberService;
        _attachmentService = attachmentService;
    }

    public IActionResult Index() => View(_memberService.GetAllMembers());

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateMember(CreateMemberViewModel createMember)
    {
        if (!ModelState.IsValid)
            return View(nameof(Create), createMember);

        var result = _memberService.CreateMember(createMember);
        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = "Member Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, result.Error!);
        return View(nameof(Create), createMember);
    }

    [HttpGet]
    public IActionResult Picture(string fileName)
    {
        var result = _attachmentService.GetFile(fileName, AttachmentFolders.Members);
        if (!result.IsSuccess)
            return NotFound();

        return File(result.Value!.Content, result.Value.ContentType);
    }

    public IActionResult MemberDetails(int id)
    {
        var member = _memberService.GetMemberDetails(id);
        if (member is null) return MemberNotFound();

        return View(member);
    }

    public IActionResult HealthRecordDetails(int id)
    {
        var healthRecord = _memberService.GetMemberHealthRecordDetails(id);
        if (healthRecord is null) return MemberNotFound();

        return View(healthRecord);
    }

    public IActionResult EditMember(int id)
    {
        var member = _memberService.GetMemberToUpdate(id);
        if (member is null) return MemberNotFound();

        return View(member);
    }

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

    public IActionResult Delete(int id)
    {
        if (_memberService.GetMemberToUpdate(id) is null) return MemberNotFound();

        return View();
    }

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
