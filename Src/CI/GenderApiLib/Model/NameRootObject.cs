namespace GenderApiLib.Model;

public class FirstnameRootObject
{
  public string name { get; set; }
  public Country_Of_Origin[] country_of_origin { get; set; }
  public string name_sanitized { get; set; }
  public string gender { get; set; }
  public int samples { get; set; }
  public int accuracy { get; set; }
  public string country_of_origin_map_url { get; set; }
  public int credits_used { get; set; }
  public string duration { get; set; }
}