namespace GenderApiLib;
public class GenderApi
{
  public static async Task<(TimeSpan ts, string exMsg, FirstnameRootObject? root)> CallOpenAI(IConfigurationRoot cfg, string firstName, bool cacheOnly = false)
  {
    Trace.WriteLine($"■ ■ ■ cfg?[\"WhereAmI\"]: '{cfg?["WhereAmI"]}'.");

    var stopwatch = Stopwatch.StartNew();

    if (RapidApiHelpers.IsBadName(firstName))
      return (stopwatch.Elapsed, $"Bad name: '{firstName}'", null);

    var filename = $"""C:\g\CI\Src\CI\GenderApiLib\Cache.FirstName\{firstName}.json""";
    var noCache = $"""C:\g\CI\Src\CI\GenderApiLib\Cache.FirstName\_NoCache_.json""";

    try
    {
      var jsonPart = File.Exists(filename) ? await File.ReadAllTextAsync(filename) :
                                 cacheOnly ? await File.ReadAllTextAsync(noCache) : await GetFromWeb(cfg, firstName);
      if (jsonPart is null)
        return (stopwatch.Elapsed, "jsonPart is null", null);

      //if (jsonPart.Contains("errno")) return (stopwatch.Elapsed, "jsonPart has errno", null);
      //if (jsonPart.Contains("errmsg")) return (stopwatch.Elapsed, "jsonPart has errmsg", null);

      var root = JsonConvert.DeserializeObject<FirstnameRootObject>(jsonPart);          //dynamic dynObj = JsonConvert.DeserializeObject(jsonPart) ?? "No way!";          //dynamic dynObj = JsonSerializer.Deserialize(json);

      if (!cacheOnly && string.IsNullOrEmpty(root?.errmsg) && !File.Exists(filename))
        await File.WriteAllTextAsync(filename, jsonPart);

      try
      {
        return (stopwatch.Elapsed, "", root);
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

  static async Task<string> GetFromWeb(IConfigurationRoot cfg, string firstName)
  {
    var key = cfg?["GenderApi"] + "6c7ef4601";
    var url = $"https://gender-api.com/get-country-of-origin?name={firstName}&key={key}";

    using var httpClient = new HttpClient();
    using var requestMsg = new HttpRequestMessage(new HttpMethod("POST"), url);

    var response = await httpClient.SendAsync(requestMsg);//.Result;
    var jsonPart = await response.Content.ReadAsStringAsync();//.Result;
    return jsonPart;
  }
}