using Chinook.ClientModels;

namespace Chinook.Services
{
    public interface ITrackService
    {
        Task<List<PlaylistTrack>> GetTracks(string userId, long artistId);
    }
}
