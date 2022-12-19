using Chinook.Models;

namespace Chinook.Services
{
    public interface IPlaylistService
    {
        Task<Playlist?> GetPlayList(long playlistId);
        Task<Chinook.ClientModels.Playlist?> GetPlayList(string userId, long playlistId);
        Task<List<Chinook.ClientModels.Playlist>> GetUserPlayLists(string userId);
        Task<UserPlaylist?> GetUserPlayListByName(string userId, string name);
        Task<UserPlaylist?> GetUserPlayListById(string userId, long playlistId);
        Task<UserPlaylist> CreatePlaylist(string userId, string name);
        void RemovePlaylist(string userId, long playlistId);
        void AddTrackToPlaylist(long trackId, long playlistId);
        void RemoveTrackFromPlaylist(long trackId, long playlistId);
    }
}
