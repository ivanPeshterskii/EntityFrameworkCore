namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class MusicHubDbContext : DbContext
    {
        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Song> Songs { get; set; } = null!;

        public DbSet<Album> Albums { get; set; } = null!;

        public DbSet<Performer> Performers { get; set; } = null!;

        public DbSet<Producer> Producers { get; set; } = null!;

        public DbSet<Writer> Writers { get; set; } = null!;

        public DbSet<SongPerformer> SongsPerformers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SongPerformer>()
                .HasKey(k => new { k.SongId, k.PerformerId });

            builder.Entity<SongPerformer>()
                .HasOne(s => s.Song)
                .WithMany(s => s.SongPerformers)
                .HasForeignKey(fk => fk.SongId);

            builder.Entity<SongPerformer>()
                .HasOne(p => p.Performer)
                .WithMany(s => s.PerformerSongs)
                .HasForeignKey(fk => fk.PerformerId);

            builder.Entity<Song>()
                .HasOne(r => r.Writer)
                .WithMany(s => s.Songs)
                .HasForeignKey(fk => fk.WriterId);

            builder.Entity<Song>()
                .HasOne(a => a.Album)
                .WithMany(s => s.Songs)
                .HasForeignKey(fk => fk.AlbumId);

            builder.Entity<Album>()
                .HasOne(p => p.Producer)
                .WithMany(a => a.Albums)
                .HasForeignKey(fk => fk.ProducerId);
        }
    }
}
