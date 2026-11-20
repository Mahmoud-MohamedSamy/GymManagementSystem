using AutoMapper;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.BLL.ViewModels.TrainerViewModels;
using GymManagementSystem.DAL.Entities;

namespace GymManagementSystem.BLL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        MapMembers();
        MapTrainers();
        MapPlans();
        MapSessions();
    }

    private void MapMembers()
    {
        CreateMap<Member, MemberViewModel>();

        CreateMap<Member, MemberDetailsViewModel>()
            .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToShortDateString()))
            .ForMember(d => d.Address, o => o.MapFrom(s =>
                s.Address.BuildingNumber.ToString() + ", " + s.Address.Street + ", " + s.Address.City));

        CreateMap<CreateMemberViewModel, Member>()
            .ForMember(d => d.Address, o => o.MapFrom(s => new Address
            {
                BuildingNumber = s.BuildingNumber,
                Street = s.Street,
                City = s.City
            }))
            .ForMember(d => d.HealthRecord, o => o.MapFrom(s => s.HealthRecordViewModel));

        CreateMap<HealthRecordViewModel, HealthRecord>();
        CreateMap<HealthRecord, HealthRecordViewModel>();

        CreateMap<Member, MemberToUpdateViewModel>()
            .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
            .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
            .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City));
    }

    private void MapTrainers()
    {
        CreateMap<Trainer, TrainerViewModel>();

        CreateMap<Trainer, TrainerDetailsViewModel>()
            .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToShortDateString()))
            .ForMember(d => d.Address, o => o.MapFrom(s =>
                s.Address.BuildingNumber.ToString() + ", " + s.Address.Street + ", " + s.Address.City));

        CreateMap<CreateTrainerViewModel, Trainer>()
            .ForMember(d => d.Address, o => o.MapFrom(s => new Address
            {
                BuildingNumber = s.BuildingNumber,
                Street = s.Street,
                City = s.City
            }));

        CreateMap<Trainer, TrainerToUpdateViewModel>()
            .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
            .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
            .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City));
    }

    private void MapPlans()
    {
        CreateMap<Plan, PlanViewModel>();

        CreateMap<Plan, UpdatePlanViewModel>()
            .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Name));
    }

    private void MapSessions()
    {
        CreateMap<Session, SessionViewModel>()
            .ForMember(d => d.TrainerName, o => o.MapFrom(s => s.Trainer.Name))
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.CategoryName))
            .ForMember(d => d.AvailableSlots, o => o.MapFrom(s => s.Capacity - s.Bookings.Count));

        CreateMap<CreateSessionViewModel, Session>();
        CreateMap<Session, UpdateSessionViewModel>();

        CreateMap<Trainer, TrainerSelectViewModel>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name + " (" + s.Specialties.ToString() + ")"));

        CreateMap<Category, CategorySelectViewModel>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.CategoryName));
    }
}
