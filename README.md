# Power Fitness – Gym Management System (ASP.NET Core MVC)

Built from the *Gym Management System* spec and Route MVC Sessions 01–04, using the
Fourth-Session frontend (Razor-ified) for the UI.

## Run it
1. Open `GymManagementSystem.sln` (needs the .NET 10 SDK + SQL Server / LocalDB).
2. Check `ConnectionStrings:DefaultConnection` in `GymManagementSystem.PL/appsettings.json`
   (LocalDB: `Server=(localdb)\MSSQLLocalDB;Database=GymManagementSystem;Trusted_Connection=True;`).
3. Create the first migration (Package Manager Console, Default project = DAL, Startup = PL):
   ```
   Add-Migration InitialCreate
   ```
   or CLI from the solution folder:
   ```
   dotnet ef migrations add InitialCreate -p GymManagementSystem.DAL -s GymManagementSystem.PL
   ```
4. Run the PL project. At startup it applies pending migrations and seeds the 4 Plans and
   the 4 Categories from JSON (only when those tables are empty).

On an older SDK, change `<TargetFramework>` and the EF Core package versions in the 3 csproj files.

## Where each session lives
| Session | Topic | Files |
|---|---|---|
| 01 | MVC, views, tag helpers | `PL/Views/_ViewStart`, `_ViewImports`, `Shared/_Layout`, all `.cshtml` using tag helpers |
| 02 | N-Tier, Repository, DI, full DB | 3 projects (PL → BLL → DAL); `DAL/Entities`, `DAL/Configurations`, `GymDbContext`; DI in `PL/Program.cs` |
| 03 | Generic repo, Service layer, Member module, Plan + Trainer | `DAL/Repositories` (`GenericRepository<T>`, `MemberRepository`), `BLL/Services` (Member/Trainer/Plan), ViewModels, TempData flash messages, client-side validation |
| 04 | Unit of Work, AutoMapper, Result pattern, Session module, seeding, analytics | `UnitOfWork`, `BLL/Mapping/MappingProfile`, `BLL/Common/Result`, `SessionService`, `GymDataSeeding` + `plans.json`/`categories.json`, `AnalyticsService` + Home dashboard |

Not included (not part of sessions 1–4): Memberships, Sessions Schedule (bookings), Identity/Login.
The entities, tables and relationships for them already exist, so those modules can plug in later.

## Decisions worth knowing
- Where the spec PDF and the slides differ, the PDF/DB diagram wins: `Members` and `Trainers` are separate tables
  (GymUser is an abstract, unmapped base), `HealthRecord` shares the `Members` table, `Address` is flattened
  into `BuildingNumber/Street/City`, `Membership` and `Booking` use composite keys.
- Category names are seeded to equal the `Specialties` enum names so "trainer specialty must match category" is a simple comparison.
- `AutoMapper` is pinned to 14.0.0 (newer major versions need a license key).
- Street/City are `varchar(30)` and Name `varchar(50)` per the spec, while the supplied ViewModels allow longer input.
  Longer values pass validation but fail on save (the services return a friendly failure message).
- `CreateSessionViewModel.Capacity` is kept as supplied (`Range(0, 25)`); `SessionService` enforces 1–25.
