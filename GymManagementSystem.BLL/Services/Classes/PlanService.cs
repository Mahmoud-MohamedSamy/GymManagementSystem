using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class PlanService : IPlanService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IEnumerable<PlanViewModel> GetAllPlans()
    {
        var plans = _unitOfWork.GetRepository<Plan>().GetAll();
        return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
    }

    public PlanViewModel? GetPlanDetails(int planId)
    {
        var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
        return plan is null ? null : _mapper.Map<PlanViewModel>(plan);
    }

    public UpdatePlanViewModel? GetPlanToUpdate(int planId)
    {
        var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
        if (plan is null || HasActiveMemberships(planId)) return null;

        return _mapper.Map<UpdatePlanViewModel>(plan);
    }

    public bool UpdatePlan(int planId, UpdatePlanViewModel planToUpdate)
    {
        var repository = _unitOfWork.GetRepository<Plan>();

        var plan = repository.GetById(planId);
        if (plan is null) return false;

        if (!string.IsNullOrWhiteSpace(planToUpdate.PlanName) && planToUpdate.PlanName != plan.Name)
            return false;

        if (HasActiveMemberships(planId)) return false;

        plan.Description = planToUpdate.Description;
        plan.Price = planToUpdate.Price;
        plan.DurationDays = planToUpdate.DurationDays;

        try
        {
            repository.Update(plan);
            return _unitOfWork.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool ToggleStatus(int planId)
    {
        var repository = _unitOfWork.GetRepository<Plan>();

        var plan = repository.GetById(planId);
        if (plan is null) return false;

        if (plan.IsActive && HasActiveMemberships(planId)) return false;

        plan.IsActive = !plan.IsActive;

        try
        {
            repository.Update(plan);
            return _unitOfWork.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private bool HasActiveMemberships(int planId)
        => _unitOfWork.GetRepository<Membership>()
                      .Any(m => m.PlanId == planId && m.EndDate > DateTime.Now);
}
