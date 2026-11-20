using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IEnumerable<MemberViewModel> GetAllMembers()
    {
        var members = _unitOfWork.GetRepository<Member>().GetAll();
        return _mapper.Map<IEnumerable<MemberViewModel>>(members);
    }

    public bool CreateMember(CreateMemberViewModel createMember)
    {
        var repository = _unitOfWork.GetRepository<Member>();

        if (repository.Any(m => m.Email == createMember.Email || m.Phone == createMember.Phone))
            return false;

        var member = _mapper.Map<Member>(createMember);
        member.HealthRecord.LastUpdate = DateTime.Now;

        try
        {
            repository.Add(member);
            return _unitOfWork.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public MemberDetailsViewModel? GetMemberDetails(int memberId)
    {
        var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
        if (member is null) return null;

        var viewModel = _mapper.Map<MemberDetailsViewModel>(member);

        var membership = _unitOfWork.GetRepository<Membership>()
            .GetAll(m => m.MemberId == memberId && m.EndDate > DateTime.Now)
            .FirstOrDefault();

        if (membership is not null)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(membership.PlanId);
            viewModel.PlanName = plan?.Name;
            viewModel.MembershipStartDate = membership.StartDate.ToShortDateString();
            viewModel.MembershipEndDate = membership.EndDate.ToShortDateString();
        }

        return viewModel;
    }

    public HealthRecordViewModel? GetMemberHealthRecordDetails(int memberId)
    {
        var member = _unitOfWork.MemberRepository.GetWithHealthRecord(memberId);
        if (member?.HealthRecord is null) return null;

        return _mapper.Map<HealthRecordViewModel>(member.HealthRecord);
    }

    public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
    {
        var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
        return member is null ? null : _mapper.Map<MemberToUpdateViewModel>(member);
    }

    public bool UpdateMember(int memberId, MemberToUpdateViewModel memberToUpdate)
    {
        var repository = _unitOfWork.GetRepository<Member>();

        var member = repository.GetById(memberId);
        if (member is null) return false;

        if (repository.Any(m => m.Id != memberId &&
                               (m.Email == memberToUpdate.Email || m.Phone == memberToUpdate.Phone)))
            return false;

        member.Email = memberToUpdate.Email;
        member.Phone = memberToUpdate.Phone;
        member.Address.BuildingNumber = memberToUpdate.BuildingNumber;
        member.Address.Street = memberToUpdate.Street;
        member.Address.City = memberToUpdate.City;

        try
        {
            repository.Update(member);
            return _unitOfWork.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool RemoveMember(int memberId)
    {
        var member = _unitOfWork.MemberRepository.GetWithHealthRecord(memberId);
        if (member is null) return false;

        var hasActiveBookings = _unitOfWork.GetRepository<Booking>()
            .Any(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now);
        if (hasActiveBookings) return false;

        try
        {
            _unitOfWork.GetRepository<Member>().Delete(member);
            return _unitOfWork.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
