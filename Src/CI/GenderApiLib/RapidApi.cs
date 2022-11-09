namespace GenderApiLib;
public class RapidApi
{
  public static async Task<(TimeSpan ts, string finishReason, FirstnameRootObject? root)> CallOpenAI(IConfigurationRoot cfg, string firstName)
  {
    var stopwatch = Stopwatch.StartNew();
    var filename = $@"C:\g\CI\Src\CI\GenderApiLib\Cache.LastName\{firstName}.json";

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