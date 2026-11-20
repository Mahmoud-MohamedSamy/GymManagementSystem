using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class AnalyticsService : IAnalyticsService
{
    private readonly IUnitOfWork _unitOfWork;

    public AnalyticsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public AnalyticsViewModel GetAnalyticsData()
    {
        var now = DateTime.Now;
        var sessions = _unitOfWork.GetRepository<Session>();

        var activeMembers = _unitOfWork.GetRepository<Membership>()
            .GetAll(m => m.EndDate > now)
            .Select(m => m.MemberId)
            .Distinct()
            .Count();

        return new AnalyticsViewModel
        {
            TotalMembers = _unitOfWork.GetRepository<Member>().Count(),
            ActiveMembers = activeMembers,
            TotalTrainers = _unitOfWork.GetRepository<Trainer>().Count(),
            UpcomingSessions = sessions.Count(s => s.StartDate > now),
            OngoingSessions = sessions.Count(s => s.StartDate <= now && s.EndDate >= now),
            CompletedSessions = sessions.Count(s => s.EndDate < now)
        };
    }
}
