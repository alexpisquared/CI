namespace NameOrigin.ModelGen;

public class Country_Of_Origin
{
  [Key]
  public string country { get; set; }
  public string country_name { get; set; }
  public float probability { get; set; }
  public string continental_region { get; set; }
  public string statistical_region { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? ChangedAt { get; set; }

  public virtual List<FirstnameCountryXRef> FirstnameCountryXRef { get; set; }
}

public class FirstnameCountryXRef
{
  public int Id { get; set; }
  public string name { get; set; }
  public string country { get; set; }
  public float probability { get; set; }
  public string Note { get; set; }
}