using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class SessionService : ISessionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IEnumerable<SessionViewModel> GetAllSessions()
    {
        var sessions = _unitOfWork.SessionRepository.GetAllWithTrainerAndCategory();
        return _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
    }

    public Result<SessionViewModel> GetSessionById(int sessionId)
    {
        var session = _unitOfWork.SessionRepository.GetByIdWithTrainerAndCategory(sessionId);
        if (session is null)
            return Result.Fail<SessionViewModel>("Session not found.", ResultStatus.NotFound);

        return Result.Ok(_mapper.Map<SessionViewModel>(session));
    }

    public Result CreateSession(CreateSessionViewModel createSession)
    {
        if (createSession.Capacity < 1 || createSession.Capacity > 25)
            return Result.Fail("Capacity must be between 1 and 25.");

        if (createSession.EndDate <= createSession.StartDate)
            return Result.Fail("End time must be after start time.");

        if (createSession.StartDate <= DateTime.Now)
            return Result.Fail("Start time must be in the future.");

        var trainer = _unitOfWork.GetRepository<Trainer>().GetById(createSession.TrainerId);
        if (trainer is null)
            return Result.Fail("Trainer not found.", ResultStatus.NotFound);

        var category = _unitOfWork.GetRepository<Category>().GetById(createSession.CategoryId);
        if (category is null)
            return Result.Fail("Category not found.", ResultStatus.NotFound);

        var specialtyCheck = CheckSpecialtyMatchesCategory(trainer, category);
        if (!specialtyCheck.IsSuccess) return specialtyCheck;

        if (_unitOfWork.SessionRepository.TrainerHasOverlap(
                trainer.Id, createSession.StartDate, createSession.EndDate))
            return Result.Fail($"{trainer.Name} already has a session in that time slot.");

        var session = _mapper.Map<Session>(createSession);

        try
        {
            _unitOfWork.SessionRepository.Add(session);
            return _unitOfWork.SaveChanges() > 0
                ? Result.Ok()
                : Result.Fail("Failed to create the session.");
        }
        catch (Exception)
        {
            return Result.Fail("Failed to create the session.");
        }
    }

    public Result<UpdateSessionViewModel> GetSessionToUpdate(int sessionId)
    {
        var session = _unitOfWork.SessionRepository.GetById(sessionId);
        if (session is null)
            return Result.Fail<UpdateSessionViewModel>("Session not found.", ResultStatus.NotFound);

        if (session.StartDate <= DateTime.Now)
            return Result.Fail<UpdateSessionViewModel>("Only upcoming sessions can be edited.");

        return Result.Ok(_mapper.Map<UpdateSessionViewModel>(session));
    }

    public Result UpdateSession(int sessionId, UpdateSessionViewModel updateSession)
    {
        var session = _unitOfWork.SessionRepository.GetByIdWithTrainerAndCategory(sessionId);
        if (session is null)
            return Result.Fail("Session not found.", ResultStatus.NotFound);

        if (session.StartDate <= DateTime.Now)
            return Result.Fail("Only upcoming sessions can be edited.");

        if (updateSession.EndDate <= updateSession.StartDate)
            return Result.Fail("End time must be after start time.");

        if (updateSession.StartDate <= DateTime.Now)
            return Result.Fail("Start time must be in the future.");

        var trainer = _unitOfWork.GetRepository<Trainer>().GetById(updateSession.TrainerId);
        if (trainer is null)
            return Result.Fail("Trainer not found.", ResultStatus.NotFound);

        var specialtyCheck = CheckSpecialtyMatchesCategory(trainer, session.Category);
        if (!specialtyCheck.IsSuccess) return specialtyCheck;

        if (_unitOfWork.SessionRepository.TrainerHasOverlap(
                trainer.Id, updateSession.StartDate, updateSession.EndDate, excludeSessionId: sessionId))
            return Result.Fail($"{trainer.Name} already has a session in that time slot.");

        session.Description = updateSession.Description;
        session.StartDate = updateSession.StartDate;
        session.EndDate = updateSession.EndDate;
        session.TrainerId = updateSession.TrainerId;

        try
        {
            _unitOfWork.SessionRepository.Update(session);
            return _unitOfWork.SaveChanges() > 0
                ? Result.Ok()
                : Result.Fail("Failed to update the session.");
        }
        catch (Exception)
        {
            return Result.Fail("Failed to update the session.");
        }
    }

    public Result RemoveSession(int sessionId)
    {
        var session = _unitOfWork.SessionRepository.GetByIdWithTrainerAndCategory(sessionId);
        if (session is null)
            return Result.Fail("Session not found.", ResultStatus.NotFound);

        var isOngoing = session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now;
        if (isOngoing)
            return Result.Fail("An ongoing session can't be deleted.");

        try
        {
            _unitOfWork.SessionRepository.Delete(session);
            return _unitOfWork.SaveChanges() > 0
                ? Result.Ok()
                : Result.Fail("Failed to delete the session.");
        }
        catch (Exception)
        {
            return Result.Fail("Failed to delete the session.");
        }
    }

    public IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown()
    {
        var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
        return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
    }

    public IEnumerable<CategorySelectViewModel> GetCategoriesForDropDown()
    {
        var categories = _unitOfWork.GetRepository<Category>().GetAll();
        return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
    }

    private static Result CheckSpecialtyMatchesCategory(Trainer trainer, Category category)
        => trainer.Specialties.ToString() == category.CategoryName
            ? Result.Ok()
            : Result.Fail($"{trainer.Name} is a {trainer.Specialties} trainer and can't lead a {category.CategoryName} session.");
}
