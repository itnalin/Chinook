using Chinook.ClientModels;
using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class TrackService: ITrackService
    {
        private readonly IDbContextFactory<ChinookContext> DbFactory;
        public TrackService(IDbContextFactory<ChinookContext> dbFactory)
        {
            DbFactory = dbFactory;
        }
        public async Task<List<PlaylistTrack>> GetTracks(string userId, long artistId)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();

            return await dbContext.Tracks.Where(a => a.Album.ArtistId == artistId)
            .Include(a => a.Album)
            .Select(t => new PlaylistTrack()
            {
                AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                TrackId = t.TrackId,
                TrackName = t.Name,
                IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "My favorite tracks")).Any()
            })
            .ToListAsync();
        }
    }
}
