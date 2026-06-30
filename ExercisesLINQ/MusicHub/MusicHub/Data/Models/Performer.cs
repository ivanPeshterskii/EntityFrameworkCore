using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models;

public class Performer
{
    public Performer()
    {
        this.PerformerSongs = new HashSet<SongPerformer>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string LastName { get; set; } = null!;

    [Required]
    public int Age { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal NetWorth { get; set; }

    public ICollection<SongPerformer> PerformerSongs { get; set; }
}
