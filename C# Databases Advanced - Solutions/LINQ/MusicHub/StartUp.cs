using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MusicHub
{
    using System;

    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            // ReSharper disable once PossibleNullReferenceException
            var albums = context
                .Producers
                .FirstOrDefault(x => x.Id == producerId)
                .Albums
                .Select(a => new
                {
                    a.Name,
                    ReleaseDate = a.ReleaseDate.ToString
                        ("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    TotalPrice = a.Price,
                    Songs = a
                        .Songs
                        .Select(s => new
                        {
                            s.Name,
                            s.Price,
                            WriterName = s.Writer.Name
                        })
                        .OrderByDescending
                            (s => s.Name)
                        .ThenBy(s => s.WriterName)
                })
                .OrderByDescending(x => x.TotalPrice)
                .ToList();

            var result = new StringBuilder();

            foreach (var album in albums)
            {
                result.AppendLine($"-AlbumName: {album.Name}");
                result.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                result.AppendLine($"-ProducerName: {album.ProducerName}");
                result.AppendLine($"-Songs:");
                int songCounter = 0;

                foreach (var song in album.Songs)
                {
                    songCounter++;
                    result.AppendLine($"---#{songCounter}");
                    result.AppendLine($"---SongName: {song.Name}");
                    result.AppendLine($"---Price: {song.Price:f2}");
                    result.AppendLine($"---Writer: {song.WriterName}");
                }

                result.AppendLine($"-AlbumPrice: {album.TotalPrice:f2}");
            }

            return result.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var sortedSongs = context
                .Songs
                .ToList()
                .Where(x => x.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    WriterName = s.Writer.Name,
                    AlbumProducerName = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c"),
                    PerformerName =
                        s
                            .SongPerformers
                            .Select(sp => sp.Performer.FirstName +
                                          " " + sp.Performer.LastName)
                            .FirstOrDefault()
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.WriterName)
                .ThenBy(x => x.PerformerName)
                .ToList();

            var result = new StringBuilder();
            int songCounter = 0;

            foreach (var song in sortedSongs)
            {
                songCounter++;
                result.AppendLine($"-Song #{songCounter}");
                result.AppendLine($"---SongName: {song.Name}");
                result.AppendLine($"---Writer: {song.WriterName}");
                result.AppendLine($"---Performer: {song.PerformerName}");
                result.AppendLine($"---AlbumProducer: {song.AlbumProducerName}");
                result.AppendLine($"---Duration: {song.Duration}");
            }

            return result.ToString().Trim();
        }
    }
}
