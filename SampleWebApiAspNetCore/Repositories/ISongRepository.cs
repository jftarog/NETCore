using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface ISongRepository
    {
        SongEntity GetSingle(int id);
        void Add(SongEntity item);
        void Delete(int id);
        SongEntity Update(int id, SongEntity item);
        IQueryable<SongEntity> GetAll(QueryParameters queryParameters);
        ICollection<SongEntity> GetRandomSong();
        int Count();
        bool Save();
    }
}
