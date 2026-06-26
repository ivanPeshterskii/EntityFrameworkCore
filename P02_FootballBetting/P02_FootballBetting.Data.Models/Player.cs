using P02_FootballBetting.Data.Models;

public class Player
{
    public int PlayerId { get; set; }
    public string Name { get; set; } = null!;
    public int SquadNumber { get; set; }
    public bool IsInjured { get; set; }

    public int TownId { get; set; }
    public Town Town { get; set; } = null!;

    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;

    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;

    public ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new HashSet<PlayerStatistic>();
}
