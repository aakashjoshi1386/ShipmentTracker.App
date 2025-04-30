namespace ShipmentTracker.App.Infrastructure.Seeding;
public static class DbSeeder
{
    public static void Seed(ShipmentTrackerAppDBContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Carriers.Any()) // seeding Carrier Master
        {
            context.Carriers.AddRange(
                new Carrier { Name = "DHL", IsActive = true },
                new Carrier { Name = "United Parcel Services (UPS)", IsActive = true },
                new Carrier { Name = "Trimble TMS", IsActive = true },
                new Carrier { Name = "CartonCloud", IsActive = true },
                new Carrier { Name = "Zeus", IsActive = false }
            );

            context.SaveChanges();
        }
    }
}
