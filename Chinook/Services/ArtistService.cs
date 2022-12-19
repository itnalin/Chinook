using Chinook.Models;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class ArtistService: IArtistService
    {
        private readonly IDbContextFactory<ChinookContext> DbFactory;
        public ArtistService(IDbContextFactory<ChinookContext> dbFactory)
        {
            DbFactory = dbFactory;
        }
        public async Task<List<Artist>> GetArtists()
        {
            var dbContext = await DbFactory.CreateDbContextAsync();            
            return dbContext.Artists.ToList();
        }
        public async Task<Artist?> GetArtists(long ArtistId)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            return dbContext.Artists.SingleOrDefault(a => a.ArtistId == ArtistId);
        }
        public async Task<List<Artist>> GetArtists(string name)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            return dbContext.Artists.Where(a => a.Name.Contains(name)).ToList();
        }
        public async Task<List<Album>> GetAlbumsForArtist(int artistId)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            return dbContext.Albums.Where(a => a.ArtistId == artistId).ToList();
        }
    }
}
