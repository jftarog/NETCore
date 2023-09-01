using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(FoodDbContext foodContext, SongDbContext songContext)
        {
            foodContext.FoodItems.Add(new FoodEntity() { Calories = 1000, Type = "Starter", Name = "Lasagne", Created = DateTime.Now });
            foodContext.FoodItems.Add(new FoodEntity() { Calories = 1100, Type = "Main", Name = "Hamburger", Created = DateTime.Now });
            foodContext.FoodItems.Add(new FoodEntity() { Calories = 1200, Type = "Dessert", Name = "Spaghetti", Created = DateTime.Now });
            foodContext.FoodItems.Add(new FoodEntity() { Calories = 1300, Type = "Starter", Name = "Pizza", Created = DateTime.Now });

            foodContext.SaveChanges();
            
            songContext.SongItems.Add(new SongEntity() {Name = "Stellar Stellar", Singer = "Hoshimachi Suisei", Length = 301, Created = DateTime.Now} );
            songContext.SongItems.Add(new SongEntity() {Name = "GHOST", Singer = "Hoshimachi Suisei", Length = 282, Created = DateTime.Now} );
            songContext.SongItems.Add(new SongEntity() {Name = "end of a life", Singer = "Calliope Mori", Length = 180, Created = DateTime.Now} );
            songContext.SongItems.Add(new SongEntity() {Name = "Cursed Night", Singer = "Calliope Mori", Length = 288, Created = DateTime.Now} );

            songContext.SaveChanges();
        }
    }
}
