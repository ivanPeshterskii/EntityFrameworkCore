using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace P02_FootballBetting.Data.Models
{
    public class Position
    {
        public Position()
        {
            this.Players = new HashSet<Player>();
        }

        public int PositionId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<Player> Players { get; set; }
    }
}
