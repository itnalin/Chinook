using Chinook.Models;
using System.Threading.Tasks;

namespace Chinook.Services
{
    public interface IArtistService
    {
        Task<List<Artist>> GetArtists();
        Task<Artist?> GetArtists(long ArtistId);
        Task<List<Artist>> GetArtists(string name);
        Task<List<Album>> GetAlbumsForArtist(int artistId);
    }
}
