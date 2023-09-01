using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class SongSqlRepository : ISongRepository
    {
        private readonly SongDbContext _songDbContext;

        public SongSqlRepository(SongDbContext songDbContext)
        {
            _songDbContext = songDbContext;
        }

        public SongEntity GetSingle(int id)
        {
            return _songDbContext.SongItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(SongEntity item)
        {
            _songDbContext.SongItems.Add(item);
        }

        public void Delete(int id)
        {
            SongEntity songItem = GetSingle(id);
            _songDbContext.SongItems.Remove(songItem);
        }

        public SongEntity Update(int id, SongEntity item)
        {
            _songDbContext.SongItems.Update(item);
            return item;
        }

        public IQueryable<SongEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<SongEntity> _allItems = _songDbContext.SongItems.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Length.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _songDbContext.SongItems.Count();
        }

        public bool Save()
        {
            return (_songDbContext.SaveChanges() >= 0);
        }

        public ICollection<SongEntity> GetRandomSong()
        {
            List<SongEntity> toReturn = new List<SongEntity>();

            toReturn.Add(GetRandomItem("Starter"));
            toReturn.Add(GetRandomItem("Main"));
            toReturn.Add(GetRandomItem("Dessert"));

            return toReturn;
        }

        private SongEntity GetRandomItem(string singer)
        {
            return _songDbContext.SongItems
                .Where(x => x.Singer == singer)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}
