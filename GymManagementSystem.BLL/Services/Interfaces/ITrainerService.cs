using GymManagementSystem.BLL.ViewModels.TrainerViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface ITrainerService
{
    IEnumerable<TrainerViewModel> GetAllTrainers();
    bool CreateTrainer(CreateTrainerViewModel createTrainer);
    TrainerDetailsViewModel? GetTrainerDetails(int trainerId);
    TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId);
    bool UpdateTrainer(int trainerId, TrainerToUpdateViewModel trainerToUpdate);
    bool RemoveTrainer(int trainerId);
}
