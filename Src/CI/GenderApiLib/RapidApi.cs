using System.Text.RegularExpressions;

namespace GenderApiLib;
public class RapidApi
{
  public static async Task<(TimeSpan ts, string finishReason, FirstnameRootObject? root)> CallOpenAI(IConfigurationRoot cfg, string firstName)
  {
    var stopwatch = Stopwatch.StartNew();
    var filename = $@"C:\g\CI\Src\CI\GenderApiLib\Cache.LastName\{firstName}.json";

    if (IsBadName(firstName))
      return (stopwatch.Elapsed, $"Bad name: '{firstName}'", null);

    try
    {
      var jsonPart = File.Exists(filename) ? await File.ReadAllTextAsync(filename) : await GetFromWeb(cfg, firstName);
      if (jsonPart is null)
        return (stopwatch.Elapsed, "jsonPart is null", null);

      if (!File.Exists(filename))
        await File.WriteAllTextAsync(filename, jsonPart);

      var dynOb2 = JsonConvert.DeserializeObject<FirstnameRootObject>(jsonPart);          //dynamic dynObj = JsonConvert.DeserializeObject(jsonPart) ?? "No way!";          //dynamic dynObj = JsonSerializer.Deserialize(json);

      try
      {
        return (stopwatch.Elapsed, "", dynOb2);
      }
      catch (Exception ex)
      {
        return (stopwatch.Elapsed, ex.Message, null);
      }
    }
    catch (Exception ex)
    {
      return (stopwatch.Elapsed, ex.Message, null);
    }
  }

  static bool IsBadName(string firstName)
  {
    var rv = true;

    if (new Regex("^[a-zA-Z]*$").Match(firstName).Success == false) return false;

    var badNames = new string[] {
    "bmo",
    "dice",
    "domain",
    "hr",
    "ibm",
    "info",
    "it",
    "madam",
      "monster",
    "no",
      "noreply",
      "sql",
      "stack",
    "the",
    "sir"};
    badNames.ToList().ForEach(name =>
    {
      if (firstName.Equals(name, StringComparison.OrdinalIgnoreCase))
        rv = false;
    });

    if(!rv) return false;

    var badParts = new string[] {
    "admin",
    "career",
    "cgi",
    "cibc",
    "contact",
    "custom",
    "data",
    "email",
    "glass",
    "human",
    "linke",
    "madam",
    "market",
    "option",
    "quest",
    "recru",
    "remove",
    "resou",
    "sales",
    "servi",
    "suppor",
    "subsc",
    "team",
    "tech",
    "sir"};
    badParts.ToList().ForEach(name =>
    {
      if (firstName.Contains(name, StringComparison.OrdinalIgnoreCase))
        rv = false;
    });

    if (!rv) return false;

    return rv;
  }

  static async Task<string> GetFromWeb(IConfigurationRoot cfg, string lastName)
  {
    var key = cfg?["RapidApi"] + "d0e4d602b1";
    var url = $"https://binaryfog-last-name-origin-v1.p.rapidapi.com/api/LastName/origin?lastName={lastName}";

    var client = new HttpClient();
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri(url),
      Headers = { { "X-RapidAPI-Key", key }, { "X-RapidAPI-Host", "binaryfog-last-name-origin-v1.p.rapidapi.com" }, },
    };

    string jsonPart = "";

    using (var response = await client.SendAsync(request))
    {
      _ = response.EnsureSuccessStatusCode();
      jsonPart = await response.Content.ReadAsStringAsync();
    }

    return jsonPart;
  }
}