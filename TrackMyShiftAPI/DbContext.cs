using Microsoft.EntityFrameworkCore;

namespace TrackMyShiftAPI;

public class ShiftDbContext : DbContext
{
    public ShiftDbContext(DbContextOptions<ShiftDbContext> options) : base(options) { }

    public DbSet<Shift> Shifts { get; set; } = null!;
}
