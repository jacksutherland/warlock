using System.Data.Entity;

namespace Warlock.Models
{
    public class DataContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Warlock.Models.DataContext>());

        public DataContext() : base("name=DataContext")
        {
        }

        public DbSet<Series> Series { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Collection> Collections { get; set; }
    }
}
