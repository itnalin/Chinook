using Chinook.Models;

namespace Chinook.Services
{
    public interface IArtistService
    {
        Task<List<Artist>> GetArtists();
        Task<List<Artist>> GetArtists(string name);
        Task<List<Album>> GetAlbumsForArtist(int artistId);
    }
}
