using NameOrigin.ModelGen;

namespace NameOrigin.Code1stModelGen;
public class Program
{
    public static void Main__()
    {
        try
        {
            using var db = new FirstnameRootObjectgingContext();

            db.Database.EnsureCreated();

            var name = DateTimeOffset.Now.ToString();

            var FirstnameRootObject = new FirstnameRootObject { name = name, name_sanitized = "..", gender = "..", samples = 3, accuracy = 2, country_of_origin_map_url = "..", credits_used = 1, duration = ".." };
            _ = db.FirstnameRootObjects.Add(FirstnameRootObject);
            _ = db.SaveChanges();

            // Display all FirstnameRootObjects from the database
            var query = from b in db.FirstnameRootObjects
                        select b;

            WriteLine("All FirstnameRootObjects in the database:");
            foreach (var item in query)
            {
                WriteLine(item.name);
            }

        }
        catch (Exception ex)
        {
            WriteLine(ex.Message);
            WriteLine(ex.InnerException?.Message);
        }
    }
}

public class FirstnameRootObjectgingContext : DbContext
{
    //public FirstnameRootObjectgingContext(DbContextOptions options) : base(options)  {  }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //   //.SetBasePath(Directory.GetCurrentDirectory())
            //   //.AddJsonFile("appsettings.json")
            //   .Build();
            var connectionString = @"Server=.\SqlExpRess;Database=QStatsDbg2;Trusted_Connection=True;"; // configuration.GetConnectionString("DbCoreConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    public DbSet<FirstnameRootObject> FirstnameRootObjects { get; set; }
    public DbSet<Country_Of_Origin> Country_Of_Origins { get; set; }
    public DbSet<FirstnameCountryXRef> FirstnameCountryXRefs { get; set; }
}
