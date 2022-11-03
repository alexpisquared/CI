namespace NameOrigin.Code1stModelGen;

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
