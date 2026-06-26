using P02_FootballBetting.Data.Models;

public class Town
{
    public int TownId { get; set; }
    public string Name { get; set; } = null!;

    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;

    public ICollection<Team> Teams { get; set; } = new HashSet<Team>();
    public ICollection<Player> Players { get; set; } = new HashSet<Player>();
}

