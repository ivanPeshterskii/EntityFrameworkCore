namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            string result = ExportAlbumsInfo(context, 9);

            Console.WriteLine(result);

            Console.ReadKey();
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate,
                    ProducerName = a.Producer.Name,
                    AlbumPrice = a.Songs.Sum(s => s.Price),
                    Songs = a.Songs
                        .OrderByDescending(s => s.Name)
                        .ThenBy(s => s.Writer.Name)
                        .Select(s => new
                        {
                            SongName = s.Name,
                            Price = s.Price,
                            WriterName = s.Writer.Name
                        })
                        .ToArray()
                })
                .OrderByDescending(a => a.AlbumPrice)
                .ToArray();

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate:MM/dd/yyyy}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine("-Songs:");

                int songNumber = 1;

                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{songNumber}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price:F2}");
                    sb.AppendLine($"---Writer: {song.WriterName}");

                    songNumber++;
                }

                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();

            TimeSpan givenDuration = TimeSpan.FromSeconds(duration);

            var songs = context.Songs
                .Where(s => s.Duration > givenDuration)
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Writer.Name)
                .Select(s => new
                {
                    SongName = s.Name,
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration,
                    Performers = s.SongPerformers
                        .Select(sp => new
                        {
                            FullName = sp.Performer.FirstName + " " + sp.Performer.LastName
                        })
                        .OrderBy(p => p.FullName)
                        .ToArray()
                })
                .ToArray();

            int songNumber = 1;

            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{songNumber}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.WriterName}");

                foreach (var performer in song.Performers)
                {
                    sb.AppendLine($"---Performer: {performer.FullName}");
                }

                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration:c}");

                songNumber++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
