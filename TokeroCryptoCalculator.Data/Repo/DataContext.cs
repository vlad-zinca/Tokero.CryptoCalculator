using Microsoft.EntityFrameworkCore;
using Tokero.CryptoCalculator.Data.Models;

namespace Tokero.CryptoCalculator.Data.Repo;

public class DataContext : DbContext
{
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Investment> Investments { get; set; }
    public DbSet<PriceHistory> PriceHistories { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
}