using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IMemberService
{
    IEnumerable<MemberViewModel> GetAllMembers();
    Result CreateMember(CreateMemberViewModel createMember);
    MemberDetailsViewModel? GetMemberDetails(int memberId);
    HealthRecordViewModel? GetMemberHealthRecordDetails(int memberId);
    MemberToUpdateViewModel? GetMemberToUpdate(int memberId);
    bool UpdateMember(int memberId, MemberToUpdateViewModel memberToUpdate);
    bool RemoveMember(int memberId);
}
