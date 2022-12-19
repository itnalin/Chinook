using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class PlaylistService: IPlaylistService
    {
        private readonly IDbContextFactory<ChinookContext> DbFactory;
        public PlaylistService(IDbContextFactory<ChinookContext> dbFactory)
        {
            DbFactory = dbFactory;
        }
        public async Task<Playlist?> GetPlayList(long playlistId)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            return await dbContext.Playlists.Where(p => p.PlaylistId == playlistId).FirstOrDefaultAsync();
        }
        public async Task<Chinook.ClientModels.Playlist?> GetPlayList(string userId, long playlistId)
        {
            var DbContext = await DbFactory.CreateDbContextAsync();

            return await DbContext.Playlists
                       .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
                       .Where(p => p.PlaylistId == playlistId)
                       .Select(p => new ClientModels.Playlist()
                       {
                           Name = p.Name,
                           Tracks = p.Tracks.Select(t => new ClientModels.PlaylistTrack()
                           {
                               AlbumTitle = t.Album.Title,
                               ArtistName = t.Album.Artist.Name,
                               TrackId = t.TrackId,
                               TrackName = t.Name,
                               IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "My favorite tracks")).Any()
                           }).ToList()
                       })
                       .FirstOrDefaultAsync();
        }
        public async Task<List<Chinook.ClientModels.Playlist>> GetUserPlayLists(string userId)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();

            return await dbContext.UserPlaylists.Where(u => u.UserId == userId)
               .Include(p => p.Playlist)
               .Select(x => new ClientModels.Playlist()
               {
                   PlaylistId = x.PlaylistId,
                   Name = x.Playlist.Name
               }).ToListAsync();
        }
        public async Task<UserPlaylist?> GetUserPlayListByName(string userId, string name)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(name))
            {
                return null;
            }

            return await dbContext.UserPlaylists
               .Where(u => u.UserId == userId && u.Playlist.Name == name)
               .FirstOrDefaultAsync();           
        }
        public async Task<UserPlaylist?> GetUserPlayListById(string userId, long playlistId)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();

            if (string.IsNullOrEmpty(userId) || playlistId == 0)
            {
                return null;
            }

            return await dbContext.UserPlaylists.Where(u => u.UserId == userId && u.Playlist.PlaylistId == playlistId).FirstOrDefaultAsync();
        }
        public async Task<UserPlaylist> CreatePlaylist(string userId, string name)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            var userPlaylist = await this.GetUserPlayListByName(userId, name);
            var maxPlaylistId = dbContext.Playlists.Max(table => table.PlaylistId);

            if (userPlaylist == null)
            {
                userPlaylist = new UserPlaylist()
                {
                    UserId = userId,
                    Playlist = new Models.Playlist()
                    {
                        PlaylistId = maxPlaylistId + 1,
                        Name = name,
                    }
                };

                dbContext.UserPlaylists.Add(userPlaylist);
                dbContext.SaveChanges();
            }

            return userPlaylist;
        }
        public async void RemovePlaylist(string userId, long playlistId)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            var userPlaylist = await this.GetUserPlayListById(userId, playlistId);

            if (userPlaylist != null)
            {
                dbContext.Remove(userPlaylist);
                dbContext.SaveChanges();
            }
        }
        public async void AddTrackToPlaylist(long trackId, long playlistId)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            var track = dbContext.Tracks
                .FirstOrDefault(t => t.TrackId == trackId);

            var playlist = await dbContext.Playlists
                .Where(p => p.PlaylistId == playlistId)
                .Include(t => t.Tracks)
                .FirstOrDefaultAsync();

            if (playlist is not null)
            {
                if (track is not null)
                {
                    playlist.Tracks.Add(track);
                    dbContext.SaveChanges();
                }
            }
        }
        public async void RemoveTrackFromPlaylist(long trackId, long playlistId)
        {
            var DbContext = await DbFactory.CreateDbContextAsync();
            var Playlist = DbContext.Playlists
                .Where(p => p.PlaylistId == playlistId)
                .Include(t => t.Tracks)
                .FirstOrDefault();

            if (Playlist is not null)
            {
                var track = Playlist.Tracks.Where(t => t.TrackId == trackId).FirstOrDefault();

                if (track is not null)
                {
                    Playlist.Tracks.Remove(track);
                    DbContext.SaveChanges();
                }
            }
        }
    }
}
