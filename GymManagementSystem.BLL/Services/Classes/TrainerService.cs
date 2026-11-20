using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

public class TrainerService : ITrainerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IEnumerable<TrainerViewModel> GetAllTrainers()
    {
        var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
        return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
    }

    public bool CreateTrainer(CreateTrainerViewModel createTrainer)
    {
        var repository = _unitOfWork.GetRepository<Trainer>();

        if (repository.Any(t => t.Email == createTrainer.Email || t.Phone == createTrainer.Phone))
            return false;

        var trainer = _mapper.Map<Trainer>(createTrainer);

        try
        {
            repository.Add(trainer);
            return _unitOfWork.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public TrainerDetailsViewModel? GetTrainerDetails(int trainerId)
    {
        var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
        return trainer is null ? null : _mapper.Map<TrainerDetailsViewModel>(trainer);
    }

    public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
    {
        var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
        return trainer is null ? null : _mapper.Map<TrainerToUpdateViewModel>(trainer);
    }

    public bool UpdateTrainer(int trainerId, TrainerToUpdateViewModel trainerToUpdate)
    {
        var repository = _unitOfWork.GetRepository<Trainer>();

        var trainer = repository.GetById(trainerId);
        if (trainer is null) return false;

        if (repository.Any(t => t.Id != trainerId &&
                               (t.Email == trainerToUpdate.Email || t.Phone == trainerToUpdate.Phone)))
            return false;

        trainer.Email = trainerToUpdate.Email;
        trainer.Phone = trainerToUpdate.Phone;
        trainer.Specialties = trainerToUpdate.Specialties;
        trainer.Address.BuildingNumber = trainerToUpdate.BuildingNumber;
        trainer.Address.Street = trainerToUpdate.Street;
        trainer.Address.City = trainerToUpdate.City;

        try
        {
            repository.Update(trainer);
            return _unitOfWork.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool RemoveTrainer(int trainerId)
    {
        var repository = _unitOfWork.GetRepository<Trainer>();

        var trainer = repository.GetById(trainerId);
        if (trainer is null) return false;

        var hasActiveSessions = _unitOfWork.GetRepository<Session>()
            .Any(s => s.TrainerId == trainerId && s.EndDate > DateTime.Now);
        if (hasActiveSessions) return false;

        try
        {
            repository.Delete(trainer);
            return _unitOfWork.SaveChanges() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
