using GymManagementSystem.DAL.Entities.Enums;

namespace GymManagementSystem.BLL.ViewModels.TrainerViewModels;

// Index list row.
public class TrainerViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public Specialties Specialties { get; set; }
}

// Details page.
public class TrainerDetailsViewModel : TrainerViewModel
{
    public string DateOfBirth { get; set; } = null!;
    public string Address { get; set; } = null!;
}
