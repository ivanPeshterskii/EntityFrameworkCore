using System.Diagnostics.Metrics;
using System.Drawing;
using System.Numerics;
using Microsoft.EntityFrameworkCore;

public class FootballBettingContext : DbContext
{
    public FootballBettingContext()
    {
    }

    public FootballBettingContext(DbContextOptions<FootballBettingContext> options)
        : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Town> Towns { get; set; } = null!;
    public DbSet<Color> Colors { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<Position> Positions { get; set; } = null!;
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<PlayerStatistic> PlayerStatistics { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Bet> Bets { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost;Database=FootballBetting;Trusted_Connection=True;Encrypt=False;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Countries -> Towns
        modelBuilder.Entity<Town>()
            .HasOne(t => t.Country)
            .WithMany(c => c.Towns)
            .HasForeignKey(t => t.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Towns -> Teams
        modelBuilder.Entity<Team>()
            .HasOne(t => t.Town)
            .WithMany(tw => tw.Teams)
            .HasForeignKey(t => t.TownId)
            .OnDelete(DeleteBehavior.Restrict);

        // Towns -> Players
        modelBuilder.Entity<Player>()
            .HasOne(p => p.Town)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TownId)
            .OnDelete(DeleteBehavior.Restrict);

        // Colors -> Teams като PrimaryKitColor
        modelBuilder.Entity<Team>()
            .HasOne(t => t.PrimaryKitColor)
            .WithMany(c => c.PrimaryKitTeams)
            .HasForeignKey(t => t.PrimaryKitColorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Colors -> Teams като SecondaryKitColor
        modelBuilder.Entity<Team>()
            .HasOne(t => t.SecondaryKitColor)
            .WithMany(c => c.SecondaryKitTeams)
            .HasForeignKey(t => t.SecondaryKitColorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Teams -> Players
        modelBuilder.Entity<Player>()
            .HasOne(p => p.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // Positions -> Players
        modelBuilder.Entity<Player>()
            .HasOne(p => p.Position)
            .WithMany(pos => pos.Players)
            .HasForeignKey(p => p.PositionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Teams -> Games като HomeTeam
        modelBuilder.Entity<Game>()
            .HasOne(g => g.HomeTeam)
            .WithMany(t => t.HomeGames)
            .HasForeignKey(g => g.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // Teams -> Games като AwayTeam
        modelBuilder.Entity<Game>()
            .HasOne(g => g.AwayTeam)
            .WithMany(t => t.AwayGames)
            .HasForeignKey(g => g.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // Games -> Bets
        modelBuilder.Entity<Bet>()
            .HasOne(b => b.Game)
            .WithMany(g => g.Bets)
            .HasForeignKey(b => b.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        // Users -> Bets
        modelBuilder.Entity<Bet>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bets)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Composite key: PlayerStatistic
        modelBuilder.Entity<PlayerStatistic>()
            .HasKey(ps => new { ps.GameId, ps.PlayerId });

        // Games -> PlayerStatistics
        modelBuilder.Entity<PlayerStatistic>()
            .HasOne(ps => ps.Game)
            .WithMany(g => g.PlayerStatistics)
            .HasForeignKey(ps => ps.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        // Players -> PlayerStatistics
        modelBuilder.Entity<PlayerStatistic>()
            .HasOne(ps => ps.Player)
            .WithMany(p => p.PlayerStatistics)
            .HasForeignKey(ps => ps.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Decimal precision
        modelBuilder.Entity<Team>()
            .Property(t => t.Budget)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Game>()
            .Property(g => g.HomeTeamBetRate)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Game>()
            .Property(g => g.AwayTeamBetRate)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Game>()
            .Property(g => g.DrawBetRate)
            .HasPrecision(18, 2);

        modelBuilder.Entity<User>()
            .Property(u => u.Balance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Bet>()
            .Property(b => b.Amount)
            .HasPrecision(18, 2);
    }
}
