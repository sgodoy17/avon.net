namespace IdentiGo.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics;

    internal sealed class Configuration : DbMigrationsConfiguration<MainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MainContext context)
        {
             //Once thought of removing
            context.Database.Log = msg => Debug.WriteLine(msg);
            context.SaveChanges();
        }
    }
}
