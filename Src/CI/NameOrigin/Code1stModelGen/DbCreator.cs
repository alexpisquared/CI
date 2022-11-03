namespace NameOrigin.Code1stModelGen;

public class DbCreator
{
  public static void Create()
  {
    try
    {
      using var db = new FirstnameRootObjectgingContext();

      _ = db.Database.EnsureCreated();

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
