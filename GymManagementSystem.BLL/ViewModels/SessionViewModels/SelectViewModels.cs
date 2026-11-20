namespace GymManagementSystem.BLL.ViewModels.SessionViewModels;

// Feeds the Trainer / Category dropdowns.
public class TrainerSelectViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class CategorySelectViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
