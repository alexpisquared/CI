namespace NameOrigin.Code1stModelGen;

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