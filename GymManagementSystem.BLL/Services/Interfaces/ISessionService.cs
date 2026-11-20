using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface ISessionService
{
    IEnumerable<SessionViewModel> GetAllSessions();
    Result<SessionViewModel> GetSessionById(int sessionId);
    Result CreateSession(CreateSessionViewModel createSession);
    Result<UpdateSessionViewModel> GetSessionToUpdate(int sessionId);
    Result UpdateSession(int sessionId, UpdateSessionViewModel updateSession);
    Result RemoveSession(int sessionId);

    IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown();
    IEnumerable<CategorySelectViewModel> GetCategoriesForDropDown();
}
