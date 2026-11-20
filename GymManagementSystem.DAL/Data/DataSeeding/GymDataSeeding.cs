using System.Text.Json;
using GymManagementSystem.DAL.Entities;

namespace GymManagementSystem.DAL.Data.DataSeeding;

public static class GymDataSeeding
{
    public static bool SeedData(GymDbContext context)
    {
        try
        {
            var hasPlans = context.Plans.Any();
            var hasCategories = context.Categories.Any();

            if (hasPlans && hasCategories)
                return false;

            if (!hasPlans)
            {
                var plans = LoadJson<Plan>("plans.json");
                if (plans is { Count: > 0 })
                    context.Plans.AddRange(plans);
            }

            if (!hasCategories)
            {
                var categories = LoadJson<Category>("categories.json");
                if (categories is { Count: > 0 })
                    context.Categories.AddRange(categories);
            }

            return context.SaveChanges() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Data seeding failed: {ex.Message}");
            return false;
        }
    }

    private static List<T>? LoadJson<T>(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Data", "DataSeeding", fileName);
        if (!File.Exists(path))
            throw new FileNotFoundException($"Seed file not found: {path}");

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(
            json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
