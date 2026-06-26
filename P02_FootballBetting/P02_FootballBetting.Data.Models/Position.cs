using P02_FootballBetting.Data.Models;

public class Position
{
    public int PositionId { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Player> Players { get; set; } = new HashSet<Player>();
}