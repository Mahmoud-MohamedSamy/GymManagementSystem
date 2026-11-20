using GymManagementSystem.BLL.ViewModels.PlanViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IPlanService
{
    IEnumerable<PlanViewModel> GetAllPlans();
    PlanViewModel? GetPlanDetails(int planId);
    UpdatePlanViewModel? GetPlanToUpdate(int planId);
    bool UpdatePlan(int planId, UpdatePlanViewModel planToUpdate);
    bool ToggleStatus(int planId);
}
