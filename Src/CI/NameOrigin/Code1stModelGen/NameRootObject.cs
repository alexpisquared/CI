namespace NameOrigin.Code1stModelGen;

public class FirstnameRootObject
{
  [Key]
  public string name { get; set; }
  public Country_Of_Origin[] country_of_origin { get; set; }
  public string name_sanitized { get; set; }
  public string gender { get; set; }
  public int samples { get; set; }
  public int accuracy { get; set; }
  public string country_of_origin_map_url { get; set; }
  public int credits_used { get; set; }
  public string duration { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? ChangedAt { get; set; }

  public virtual List<FirstnameCountryXRef> FirstnameCountryXRef { get; set; }
}